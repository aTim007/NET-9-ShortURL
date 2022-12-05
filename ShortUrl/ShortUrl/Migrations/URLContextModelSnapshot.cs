﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShortUrl.Models;

#nullable disable

namespace ShortUrl.Migrations
{
    [DbContext(typeof(URLContext))]
    partial class URLContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ShortUrl.Models.URL", b =>
                {
                    b.Property<string>("ShortURL")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("FullURL")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("ShortURL");

                    b.ToTable("Urls");
                });
#pragma warning restore 612, 618
        }
    }
}
