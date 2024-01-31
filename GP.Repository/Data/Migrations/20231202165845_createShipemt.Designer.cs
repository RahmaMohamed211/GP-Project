﻿// <auto-generated />
using System;
using GP.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GP.Repository.Data.Migrations
{
    [DbContext(typeof(StoreContext))]
    [Migration("20231202165845_createShipemt")]
    partial class createShipemt
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("GP.Core.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("GP.Core.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("NameOfCity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("City");
                });

            modelBuilder.Entity("GP.Core.Entities.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Contient")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameCountry")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("GP.Core.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("PictureUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("ProductPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ProductWeight")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("ShipmentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ShipmentId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("GP.Core.Entities.Shipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTimeOffset>("DateOfRecieving")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("Dateofcreation")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("FromCityID")
                        .HasColumnType("int");

                    b.Property<decimal>("Reward")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ToCityId")
                        .HasColumnType("int");

                    b.Property<decimal>("Weight")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("FromCityID");

                    b.HasIndex("ToCityId");

                    b.ToTable("Shipment");
                });

            modelBuilder.Entity("GP.Core.Entities.City", b =>
                {
                    b.HasOne("GP.Core.Entities.Country", null)
                        .WithMany("cities")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GP.Core.Entities.Product", b =>
                {
                    b.HasOne("GP.Core.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GP.Core.Entities.Shipment", null)
                        .WithMany("Products")
                        .HasForeignKey("ShipmentId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("GP.Core.Entities.Shipment", b =>
                {
                    b.HasOne("GP.Core.Entities.City", "FromCity")
                        .WithMany("ShipmentsFrom")
                        .HasForeignKey("FromCityID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("GP.Core.Entities.City", "ToCity")
                        .WithMany("ShipmentsTo")
                        .HasForeignKey("ToCityId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("FromCity");

                    b.Navigation("ToCity");
                });

            modelBuilder.Entity("GP.Core.Entities.City", b =>
                {
                    b.Navigation("ShipmentsFrom");

                    b.Navigation("ShipmentsTo");
                });

            modelBuilder.Entity("GP.Core.Entities.Country", b =>
                {
                    b.Navigation("cities");
                });

            modelBuilder.Entity("GP.Core.Entities.Shipment", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
