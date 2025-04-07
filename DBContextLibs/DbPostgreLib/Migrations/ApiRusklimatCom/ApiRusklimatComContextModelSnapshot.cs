﻿// <auto-generated />
using DbcLib;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbPostgreLib.Migrations.ApiRusklimatCom
{
    [DbContext(typeof(ApiRusklimatComContext))]
    partial class ApiRusklimatComContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SharedLib.CategoryRusklimatModelDB", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Parent")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Parent");

                    b.ToTable("Categories", "public");
                });

            modelBuilder.Entity("SharedLib.ProductInformationRusklimatModelDB", b =>
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

                    b.Property<string>("ProductId1")
                        .HasColumnType("text");

                    b.Property<string>("TypeInfo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("ProductId1");

                    b.ToTable("ProductsInformation", "public");
                });

            modelBuilder.Entity("SharedLib.ProductPropertyRusklimatModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<string>("ProductId1")
                        .HasColumnType("text");

                    b.Property<string>("PropertyKey")
                        .HasColumnType("text");

                    b.Property<string>("Unit")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProductId1");

                    b.ToTable("ProductsProperties", "public");
                });

            modelBuilder.Entity("SharedLib.ProductRusklimatModelDB", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Brand")
                        .HasColumnType("text");

                    b.Property<string>("CategoryId")
                        .HasColumnType("text");

                    b.Property<decimal>("ClientPrice")
                        .HasColumnType("numeric");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("Exclusive")
                        .HasColumnType("boolean");

                    b.Property<decimal?>("InternetPrice")
                        .HasColumnType("numeric");

                    b.Property<string>("NSCode")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("RemainsId")
                        .HasColumnType("integer");

                    b.Property<string>("VendorCode")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RemainsId")
                        .IsUnique();

                    b.ToTable("Products", "public");
                });

            modelBuilder.Entity("SharedLib.PropertyRusklimatModelDB", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Sort")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("PropertiesCatalog", "public");
                });

            modelBuilder.Entity("SharedLib.RemainsRusklimatModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<string>("Total")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Remains", "public");
                });

            modelBuilder.Entity("SharedLib.UnitRusklimatModelDB", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IntAbbr")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("NameFull")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Units", "public");
                });

            modelBuilder.Entity("SharedLib.WarehouseRemainsRusklimatModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ParentId")
                        .HasColumnType("integer");

                    b.Property<string>("RemainValue")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("ParentId");

                    b.ToTable("WarehousesRemains", "public");
                });

            modelBuilder.Entity("SharedLib.ProductInformationRusklimatModelDB", b =>
                {
                    b.HasOne("SharedLib.ProductRusklimatModelDB", "Product")
                        .WithMany("Information")
                        .HasForeignKey("ProductId1");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("SharedLib.ProductPropertyRusklimatModelDB", b =>
                {
                    b.HasOne("SharedLib.ProductRusklimatModelDB", "Product")
                        .WithMany("Properties")
                        .HasForeignKey("ProductId1");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("SharedLib.ProductRusklimatModelDB", b =>
                {
                    b.HasOne("SharedLib.RemainsRusklimatModelDB", "Remains")
                        .WithOne("Product")
                        .HasForeignKey("SharedLib.ProductRusklimatModelDB", "RemainsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Remains");
                });

            modelBuilder.Entity("SharedLib.WarehouseRemainsRusklimatModelDB", b =>
                {
                    b.HasOne("SharedLib.RemainsRusklimatModelDB", "Parent")
                        .WithMany("WarehousesRemains")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("SharedLib.ProductRusklimatModelDB", b =>
                {
                    b.Navigation("Information");

                    b.Navigation("Properties");
                });

            modelBuilder.Entity("SharedLib.RemainsRusklimatModelDB", b =>
                {
                    b.Navigation("Product");

                    b.Navigation("WarehousesRemains");
                });
#pragma warning restore 612, 618
        }
    }
}
