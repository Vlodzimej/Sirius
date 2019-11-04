﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Sirius.DAL;
using System;

namespace Sirius.Migrations
{
    [DbContext(typeof(SiriusContext))]
    partial class SiriusContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("Sirius.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<Guid?>("ParentId");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Sirius.Models.Dimension", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Dimensions");
                });

            modelBuilder.Entity("Sirius.Models.Invoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<DateTime>("CreateDate");

                    b.Property<double>("Factor");

                    b.Property<bool>("IsFixed");

                    b.Property<bool>("IsTemporary");

                    b.Property<string>("Name");

                    b.Property<Guid>("TypeId");

                    b.Property<Guid>("UserId");

                    b.Property<Guid>("VendorId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VendorId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("Sirius.Models.InvoiceType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alias");

                    b.Property<int>("Factor");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("InvoiceTypes");
                });

            modelBuilder.Entity("Sirius.Models.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CategoryId");

                    b.Property<Guid>("DimensionId");

                    b.Property<bool>("IsCountless");

                    b.Property<double>("MinimumLimit");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("DimensionId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Sirius.Models.Register", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<decimal>("Cost");

                    b.Property<Guid>("InvoiceId");

                    b.Property<Guid>("ItemId");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.HasIndex("ItemId");

                    b.ToTable("Registers");
                });

            modelBuilder.Entity("Sirius.Models.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Sirius.Models.Setting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alias");

                    b.Property<string>("Name");

                    b.Property<Guid>("TypeId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("Sirius.Models.StorageRegister", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<decimal>("Cost");

                    b.Property<DateTime>("CreateDate");

                    b.Property<Guid>("ItemId");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("StorageRegisters");
                });

            modelBuilder.Entity("Sirius.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("FinishDate");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsConfirmed");

                    b.Property<string>("LastName");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<Guid>("RoleId");

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Sirius.Models.Vendor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Contact");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("Sirius.Models.Invoice", b =>
                {
                    b.HasOne("Sirius.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sirius.Models.Vendor", "Vendor")
                        .WithMany()
                        .HasForeignKey("VendorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sirius.Models.Item", b =>
                {
                    b.HasOne("Sirius.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sirius.Models.Dimension", "Dimension")
                        .WithMany()
                        .HasForeignKey("DimensionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sirius.Models.Register", b =>
                {
                    b.HasOne("Sirius.Models.Invoice", "Invoice")
                        .WithMany("Registers")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sirius.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sirius.Models.StorageRegister", b =>
                {
                    b.HasOne("Sirius.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sirius.Models.User", b =>
                {
                    b.HasOne("Sirius.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
