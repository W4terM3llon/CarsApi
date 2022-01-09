﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NIS_project.Data;

#nullable disable

namespace NIS_project.Migrations
{
    [DbContext(typeof(NIS_projectContext))]
    [Migration("20220109093343_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("NIS_project.Models.Car", b =>
                {
                    b.Property<int>("DbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DbId"), 1L, 1);

                    b.Property<int>("EngineDbId")
                        .HasColumnType("int");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ManufacturerDbId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.HasKey("DbId");

                    b.HasIndex("EngineDbId");

                    b.HasIndex("ManufacturerDbId");

                    b.ToTable("Car");
                });

            modelBuilder.Entity("NIS_project.Models.Engine", b =>
                {
                    b.Property<int>("DbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DbId"), 1L, 1);

                    b.Property<int>("HP")
                        .HasColumnType("int");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ManufacturerDbId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DbId");

                    b.HasIndex("ManufacturerDbId");

                    b.ToTable("Engine");
                });

            modelBuilder.Entity("NIS_project.Models.Manufacturer", b =>
                {
                    b.Property<int>("DbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DbId"), 1L, 1);

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Since")
                        .HasColumnType("datetime2");

                    b.HasKey("DbId");

                    b.ToTable("Manufacturer");
                });

            modelBuilder.Entity("NIS_project.Models.Car", b =>
                {
                    b.HasOne("NIS_project.Models.Engine", "Engine")
                        .WithMany()
                        .HasForeignKey("EngineDbId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NIS_project.Models.Manufacturer", "Manufacturer")
                        .WithMany("Cars")
                        .HasForeignKey("ManufacturerDbId");

                    b.Navigation("Engine");

                    b.Navigation("Manufacturer");
                });

            modelBuilder.Entity("NIS_project.Models.Engine", b =>
                {
                    b.HasOne("NIS_project.Models.Manufacturer", "Manufacturer")
                        .WithMany("Engines")
                        .HasForeignKey("ManufacturerDbId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Manufacturer");
                });

            modelBuilder.Entity("NIS_project.Models.Manufacturer", b =>
                {
                    b.Navigation("Cars");

                    b.Navigation("Engines");
                });
#pragma warning restore 612, 618
        }
    }
}
