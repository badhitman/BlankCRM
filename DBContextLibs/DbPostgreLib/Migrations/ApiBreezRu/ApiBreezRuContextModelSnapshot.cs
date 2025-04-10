﻿// <auto-generated />
using System;
using DbcLib;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations.ApiBreezRu
{
    [DbContext(typeof(ApiBreezRuContext))]
    partial class ApiBreezRuContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SharedLib.BrandBreezRuModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CHPU")
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Order")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Brands", "public");
                });

            modelBuilder.Entity("SharedLib.BreezRuLeftoverModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Articul")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "articul");

                    b.Property<string>("BasePrice")
                        .HasColumnType("text");

                    b.Property<string>("CodeNC")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "nc");

                    b.Property<string>("CurrencyBasePrice")
                        .HasColumnType("text");

                    b.Property<string>("CurrencyRIC")
                        .HasColumnType("text");

                    b.Property<DateTime>("LoadedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Quantity")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "quantity");

                    b.Property<string>("RIC")
                        .HasColumnType("text");

                    b.Property<string>("Stock")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "stock");

                    b.Property<string>("TimeLastUpdate")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "time");

                    b.Property<string>("Title")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "title");

                    b.HasKey("Id");

                    b.HasIndex("Articul");

                    b.HasIndex("CodeNC");

                    b.HasIndex("CurrencyBasePrice");

                    b.HasIndex("CurrencyRIC");

                    b.HasIndex("LoadedDateTime");

                    b.HasIndex("Quantity");

                    b.HasIndex("Stock");

                    b.HasIndex("TimeLastUpdate");

                    b.ToTable("Leftovers", "public");
                });

            modelBuilder.Entity("SharedLib.CategoryBreezRuModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CHPU")
                        .HasColumnType("text");

                    b.Property<int>("Key")
                        .HasColumnType("integer");

                    b.Property<string>("Level")
                        .HasColumnType("text");

                    b.Property<string>("Order")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categories", "public");
                });

            modelBuilder.Entity("SharedLib.ImageProductBreezRuModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("ProductId");

                    b.ToTable("ImagesProducts", "public");
                });

            modelBuilder.Entity("SharedLib.ProductBreezRuModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AccessoryNC")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "nc_accessory");

                    b.Property<string>("Article")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "articul");

                    b.Property<string>("BimModel")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "bim_model");

                    b.Property<string>("Booklet")
                        .HasColumnType("text");

                    b.Property<string>("Brand")
                        .HasColumnType("text");

                    b.Property<string>("CategoryId")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "category_id");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Manual")
                        .HasColumnType("text");

                    b.Property<string>("NC")
                        .HasColumnType("text");

                    b.Property<string>("NarujNC")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "nc_naruj");

                    b.Property<string>("PriceCurrencyRIC")
                        .HasColumnType("text");

                    b.Property<string>("PriceRIC")
                        .HasColumnType("text");

                    b.Property<string>("Series")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<string>("UTP")
                        .HasColumnType("text");

                    b.Property<string>("VideoYoutube")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "video_youtube");

                    b.Property<string>("VnutrNC")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "nc_vnutr");

                    b.HasKey("Id");

                    b.HasIndex("AccessoryNC");

                    b.HasIndex("Article");

                    b.HasIndex("Brand");

                    b.HasIndex("CategoryId");

                    b.HasIndex("NC");

                    b.HasIndex("NarujNC");

                    b.HasIndex("PriceRIC");

                    b.HasIndex("Series");

                    b.HasIndex("Title");

                    b.HasIndex("UTP");

                    b.HasIndex("VnutrNC");

                    b.ToTable("Products", "public");
                });

            modelBuilder.Entity("SharedLib.PropertyTechProductBreezRuModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Analog")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Filter")
                        .HasColumnType("text");

                    b.Property<string>("FilterType")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "filter_type");

                    b.Property<string>("First")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Group")
                        .HasColumnType("text");

                    b.Property<int>("IdChar")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "id_char");

                    b.Property<int>("Key")
                        .HasColumnType("integer");

                    b.Property<string>("Order")
                        .HasColumnType("text");

                    b.Property<int>("ParentId")
                        .HasColumnType("integer");

                    b.Property<string>("Required")
                        .HasColumnType("text");

                    b.Property<string>("Show")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SubCategory")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "supcat");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TypeParameter")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "type");

                    b.Property<string>("Unit")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Analog");

                    b.HasIndex("First");

                    b.HasIndex("IdChar");

                    b.HasIndex("ParentId");

                    b.HasIndex("Show");

                    b.HasIndex("SubCategory");

                    b.HasIndex("TypeParameter");

                    b.HasIndex("Value");

                    b.ToTable("PropsTechsProducts", "public");
                });

            modelBuilder.Entity("SharedLib.TechCategoryBreezRuModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("TechsCategories", "public");
                });

            modelBuilder.Entity("SharedLib.TechProductBreezRuModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AccessoryNC")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "nc_accessory");

                    b.Property<string>("NC")
                        .HasColumnType("text");

                    b.Property<string>("NarujNC")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "nc_naruj");

                    b.Property<string>("VnutrNC")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "nc_vnutr");

                    b.HasKey("Id");

                    b.HasIndex("AccessoryNC");

                    b.HasIndex("NC");

                    b.HasIndex("NarujNC");

                    b.HasIndex("VnutrNC");

                    b.ToTable("TechsProducts", "public");
                });

            modelBuilder.Entity("SharedLib.TechPropertyCategoryBreezRuModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DataType")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "data_type");

                    b.Property<string>("Filter")
                        .HasColumnType("text");

                    b.Property<string>("FilterType")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "filter_type");

                    b.Property<string>("Group")
                        .HasColumnType("text");

                    b.Property<string>("Order")
                        .HasColumnType("text");

                    b.Property<int>("ParentId")
                        .HasColumnType("integer");

                    b.Property<string>("Required")
                        .HasColumnType("text");

                    b.Property<int>("TechId")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "tech_id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Unit")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("PropsTechsCategories", "public");
                });

            modelBuilder.Entity("SharedLib.ImageProductBreezRuModelDB", b =>
                {
                    b.HasOne("SharedLib.ProductBreezRuModelDB", "Product")
                        .WithMany("Images")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("SharedLib.PropertyTechProductBreezRuModelDB", b =>
                {
                    b.HasOne("SharedLib.TechProductBreezRuModelDB", "Parent")
                        .WithMany("Properties")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("SharedLib.TechPropertyCategoryBreezRuModelDB", b =>
                {
                    b.HasOne("SharedLib.TechCategoryBreezRuModelDB", "Parent")
                        .WithMany("Properties")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("SharedLib.ProductBreezRuModelDB", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("SharedLib.TechCategoryBreezRuModelDB", b =>
                {
                    b.Navigation("Properties");
                });

            modelBuilder.Entity("SharedLib.TechProductBreezRuModelDB", b =>
                {
                    b.Navigation("Properties");
                });
#pragma warning restore 612, 618
        }
    }
}
