using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ShortUrlGenerator
{
    public static class AsyncEnumerableExtensions
    {
        private static readonly ActivitySource AsyncEnumerableSource = new("Skillaz.AsyncEnumerable");

        public static async IAsyncEnumerable<IReadOnlyList<TSource>> Batch<TSource>(this IAsyncEnumerable<TSource> source, int batchSize)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (batchSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }

            var batch = new List<TSource>();
            await foreach (var item in source)
            {
                batch.Add(item);
                if (batch.Count == batchSize)
                {
                    yield return batch;
                    batch = new List<TSource>();
                }
            }

            if (batch.Count > 0)
            {
                yield return batch;
            }
        }

        public static async IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            await foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        public static async IAsyncEnumerable<TSource> Take<TSource>(this IAsyncEnumerable<TSource> source, long count)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count needs to be greater than or equal to zero");
            }

            var currentCount = 0;
            await foreach (var item in source)
            {
                if (currentCount < count)
                {
                    yield return item;
                    currentCount++;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Группирует элементы, пока соседние элементы совпадают по ключу.
        /// </summary>
        /// <remarks>
        /// Логика портирована из аналогичного метода MoreLinq.
        /// </remarks>
        /// <param name="source">Асинхронное перечисление элементов</param>
        /// <param name="keySelector">Функция получения ключа для сравнения</param>
        /// <typeparam name="TSource">Тип элементов перечисления</typeparam>
        /// <typeparam name="TKey">Тип сравниваемого ключа</typeparam>
        /// <returns></returns>
        public static async IAsyncEnumerable<(TKey Group, IReadOnlyList<TSource> Elements)> GroupAdjacent<TSource, TKey>(
            this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            await using var iterator = source.GetAsyncEnumerator();

            var group = default(TKey);
            var members = (List<TSource>)null;

            while (await iterator.MoveNextAsync())
            {
                var key = keySelector(iterator.Current);
                var element = iterator.Current;
                if (members != null && group.CompareTo(key) == 0)
                {
                    members.Add(element);
                }
                else
                {
                    if (members != null)
                    {
                        yield return (group, members);
                    }

                    group = key;
                    members = new List<TSource> { element };
                }
            }

            if (members != null)
            {
                yield return (group, members);
            }
        }

        public static async Task<TSource> FirstOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken token = default)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            await foreach (var item in source.WithCancellation(token))
            {
                return item;
            }

            return default;
        }

        public static async Task<T[]> ToArrayAsync<T>(this IAsyncEnumerable<T> source,
            CancellationToken cancellationToken = default,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            using var span = AsyncEnumerableSource.StartActivity($"ToArrayAsync {typeof(T).Name}");

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var res = new List<T>();
            var needToLog = true;
            await foreach (var item in source.WithCancellation(cancellationToken))
            {
                res.Add(item);
                if (res.Count > 1000 && needToLog)
                {
                    needToLog = false;
                    span?.SetTag("caller_member_name", memberName);
                    span?.SetTag("caller_file_path", sourceFilePath);
                    span?.SetTag("caller_line_number", sourceLineNumber);
                    span?.SetTag("big_list", true);
                }
            }

            if (!needToLog)
            {
                span?.SetTag("big_list_item_count", res.Count);
            }

            return res.ToArray();
        }

        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source,
            CancellationToken cancellationToken = default,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            using var span = AsyncEnumerableSource.StartActivity($"ToListAsync {typeof(T).Name}");
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var res = new List<T>();
            var needToLog = true;
            await foreach (var item in source.WithCancellation(cancellationToken))
            {
                res.Add(item);
                if (res.Count > 1000 && needToLog)
                {
                    needToLog = false;
                    span?.SetTag("big_list_item_count", res.Count);
                    span?.SetTag("caller_member_name", memberName);
                    span?.SetTag("caller_file_path", sourceFilePath);
                    span?.SetTag("caller_line_number", sourceLineNumber);
                    span?.SetTag("big_list", true);
                }
            }

            if (!needToLog)
            {
                span?.SetTag("big_list_item_count", res.Count);
            }

            return res;
        }

        public static async Task ParallelForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> func, CancellationToken cancellationToken = default)
        {
            await ParallelForEachAsync(enumerable, Producers<T>.EnumerableProducer, func, Environment.ProcessorCount, null, cancellationToken);
        }

        public static async Task ParallelForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> func, int degreeOfParallel, CancellationToken cancellationToken = default)
        {
            await ParallelForEachAsync(enumerable, Producers<T>.EnumerableProducer, func, degreeOfParallel, null, cancellationToken);
        }

        public static async Task ParallelForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> func, int degreeOfParallel, int bufferSize, CancellationToken cancellationToken = default)
        {
            await ParallelForEachAsync(enumerable, Producers<T>.EnumerableProducer, func, degreeOfParallel, bufferSize, cancellationToken);
        }

        public static async Task ParallelForEachAsync<T>(this IAsyncEnumerable<T> enumerable, Func<T, Task> func, CancellationToken cancellationToken = default)
        {
            await ParallelForEachAsync(enumerable, Producers<T>.AsyncEnumerableProducer, func, Environment.ProcessorCount, null, cancellationToken);
        }

        public static async Task ParallelForEachAsync<T>(this IAsyncEnumerable<T> enumerable, Func<T, Task> func, int degreeOfParallel, CancellationToken cancellationToken = default)
        {
            await ParallelForEachAsync(enumerable, Producers<T>.AsyncEnumerableProducer, func, degreeOfParallel, null, cancellationToken);
        }

        public static async Task ParallelForEachAsync<T>(this IAsyncEnumerable<T> enumerable, Func<T, Task> func, int degreeOfParallel, int bufferSize, CancellationToken cancellationToken = default)
        {
            await ParallelForEachAsync(enumerable, Producers<T>.AsyncEnumerableProducer, func, degreeOfParallel, bufferSize, cancellationToken);
        }

        private static async Task ParallelForEachAsync<TArgs, T>(TArgs args, Func<TArgs, Channel<T>, Activity, CancellationToken, ValueTask> producer,
            Func<T, Task> func, int degreeOfParallel, int? bufferSize = null, CancellationToken cancellationToken = default)
        {
            var itemTypeName = typeof(T).Name;
            using var parallelSpan = AsyncEnumerableSource.StartActivity($"Parallel for each {itemTypeName}");
            parallelSpan?.SetTag("degree_of_parallel", degreeOfParallel);

            if (degreeOfParallel <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(degreeOfParallel));
            }

            var channelCapacity = bufferSize ?? degreeOfParallel;
            if (channelCapacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(channelCapacity));
            }

            var channel = Channel.CreateBounded<T>(new BoundedChannelOptions(channelCapacity)
            {
                SingleWriter = true,
                SingleReader = false
            });

            var tasks = Enumerable.Range(0, degreeOfParallel).Select(async index =>
            {
                try
                {
                    while (await channel.Reader.WaitToReadAsync(cancellationToken))
                    {
                        if (channel.Reader.TryRead(out var item))
                        {
                            try
                            {
                                using var parallelItemSpan = AsyncEnumerableSource.StartActivity($"Process parallel item {itemTypeName}");
                                parallelItemSpan?.SetTag("parallel_item_index", index);
                                await func(item);
                            }
                            catch (Exception e)
                            {
                                channel.Writer.TryComplete(e);
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex) when (!(ex is TaskCanceledException || ex is OperationCanceledException))
                {
                    // ignored
                }
            }).ToArray();

            try
            {
                await producer(args, channel, parallelSpan, cancellationToken);
            }
            catch (ChannelClosedException ex)
            {
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }

                throw;
            }

            if (!channel.Reader.Completion.IsCompleted)
            {
                channel.Writer.TryComplete();
            }

            await Task.WhenAll(tasks);
            await channel.Reader.Completion;
        }

        private static class Producers<T>
        {
            public static readonly Func<IAsyncEnumerable<T>, Channel<T>, Activity, CancellationToken, ValueTask> AsyncEnumerableProducer = async (enumerable, channel, span, token) =>
            {
                var totalItems = 0;
                await foreach (var item in enumerable.WithCancellation(token))
                {
                    totalItems++;
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    if (!channel.Reader.Completion.IsCompleted)
                    {
                        await channel.Writer.WriteAsync(item);
                    }
                }

                span?.SetTag("parallel_total_items", totalItems);
            };

            public static readonly Func<IEnumerable<T>, Channel<T>, Activity, CancellationToken, ValueTask> EnumerableProducer = async (enumerable, channel, span, token) =>
            {
                var totalItems = 0;
                foreach (var item in enumerable)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    if (!channel.Reader.Completion.IsCompleted)
                    {
                        await channel.Writer.WriteAsync(item);
                    }
                }

                span?.SetTag("parallel_total_items", totalItems);
            };
        }
    }
}