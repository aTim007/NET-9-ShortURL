﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShortUrlGenerator;

#nullable disable

namespace ShortUrlGenerator.Migrations
{
    [DbContext(typeof(UrlContext))]
    [Migration("20221215082000_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ShortUrlGenerator.Models.Url", b =>
                {
                    b.Property<string>("ShortUrl")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("FullUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ShortUrl");

                    b.ToTable("Urls");
                });
#pragma warning restore 612, 618
        }
    }
}