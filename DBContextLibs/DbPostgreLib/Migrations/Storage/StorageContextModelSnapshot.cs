﻿// <auto-generated />
using System;
using DbcLib;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations.Storage
{
    [DbContext(typeof(StorageContext))]
    partial class StorageContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SharedLib.FileTagModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NormalizedNameUpper")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("OwnerFileId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("NormalizedNameUpper");

                    b.HasIndex("OwnerFileId");

                    b.ToTable("FilesTags");
                });

            modelBuilder.Entity("SharedLib.StorageCloudParameterModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ApplicationName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("OwnerPrimaryKey")
                        .HasColumnType("integer");

                    b.Property<string>("PrefixPropertyName")
                        .HasColumnType("text");

                    b.Property<string>("SerializedDataJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("TypeName");

                    b.HasIndex("ApplicationName", "Name");

                    b.HasIndex("PrefixPropertyName", "OwnerPrimaryKey");

                    b.ToTable("CloudProperties");
                });

            modelBuilder.Entity("SharedLib.StorageFileModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ApplicationName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AuthorIdentityId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ContentType")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("FileLength")
                        .HasColumnType("bigint");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NormalizedFileNameUpper")
                        .HasColumnType("text");

                    b.Property<int?>("OwnerPrimaryKey")
                        .HasColumnType("integer");

                    b.Property<string>("PointId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PrefixPropertyName")
                        .HasColumnType("text");

                    b.Property<string>("ReferrerMain")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorIdentityId");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("FileName");

                    b.HasIndex("NormalizedFileNameUpper");

                    b.HasIndex("PointId");

                    b.HasIndex("ReferrerMain");

                    b.HasIndex("ApplicationName", "Name");

                    b.HasIndex("PrefixPropertyName", "OwnerPrimaryKey");

                    b.ToTable("CloudFiles");
                });

            modelBuilder.Entity("SharedLib.FileTagModelDB", b =>
                {
                    b.HasOne("SharedLib.StorageFileModelDB", "OwnerFile")
                        .WithMany("Tags")
                        .HasForeignKey("OwnerFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OwnerFile");
                });

            modelBuilder.Entity("SharedLib.StorageFileModelDB", b =>
                {
                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
