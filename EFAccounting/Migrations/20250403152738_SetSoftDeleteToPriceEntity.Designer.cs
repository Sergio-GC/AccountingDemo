﻿// <auto-generated />
using System;
using EFAccounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFAccounting.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20250403152738_SetSoftDeleteToPriceEntity")]
    partial class SetSoftDeleteToPriceEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("EFAccounting.Entities.Kid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateOnly?>("BirthDate")
                        .HasColumnType("date");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Kids");
                });

            modelBuilder.Entity("EFAccounting.Entities.Price", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<float>("Value")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Price");
                });

            modelBuilder.Entity("EFAccounting.Entities.WDay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<TimeOnly?>("Arrival")
                        .HasColumnType("time(0)");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<TimeOnly?>("Departure")
                        .HasColumnType("time(0)");

                    b.Property<int>("KidId")
                        .HasColumnType("int");

                    b.Property<int>("PriceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("KidId");

                    b.HasIndex("PriceId");

                    b.ToTable("Wdays");
                });

            modelBuilder.Entity("KidKid", b =>
                {
                    b.Property<int>("SiblingFromId")
                        .HasColumnType("int");

                    b.Property<int>("SiblingToId")
                        .HasColumnType("int");

                    b.HasKey("SiblingFromId", "SiblingToId");

                    b.HasIndex("SiblingToId");

                    b.ToTable("KidSiblings", (string)null);
                });

            modelBuilder.Entity("EFAccounting.Entities.WDay", b =>
                {
                    b.HasOne("EFAccounting.Entities.Kid", "Kid")
                        .WithMany()
                        .HasForeignKey("KidId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFAccounting.Entities.Price", "Price")
                        .WithMany()
                        .HasForeignKey("PriceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kid");

                    b.Navigation("Price");
                });

            modelBuilder.Entity("KidKid", b =>
                {
                    b.HasOne("EFAccounting.Entities.Kid", null)
                        .WithMany()
                        .HasForeignKey("SiblingFromId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFAccounting.Entities.Kid", null)
                        .WithMany()
                        .HasForeignKey("SiblingToId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
