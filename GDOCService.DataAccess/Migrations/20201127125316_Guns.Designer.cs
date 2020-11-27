﻿// <auto-generated />
using System;
using GDOCService.DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GDOCService.DataAccess.Migrations
{
    [DbContext(typeof(GDOCContext))]
    [Migration("20201127125316_Guns")]
    partial class Guns
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("GDOCService.DataAccess.Models.Guns", b =>
                {
                    b.Property<int>("Gunsid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Inactive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Gunsid");

                    b.ToTable("Guns");
                });

            modelBuilder.Entity("GDOCService.DataAccess.Models.Stores", b =>
                {
                    b.Property<int>("StoreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CompanyID")
                        .HasColumnType("int");

                    b.Property<string>("StoreCode")
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Address")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("CityID")
                        .HasColumnType("text");

                    b.Property<string>("CountryID")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("EMailAddress")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<bool>("Inactive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("PhoneNum")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("StateID")
                        .HasColumnType("text");

                    b.HasKey("StoreId", "CompanyID", "StoreCode");

                    b.ToTable("Stores");
                });
#pragma warning restore 612, 618
        }
    }
}
