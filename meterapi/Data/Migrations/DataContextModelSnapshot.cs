﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using meterapi.Data;

#nullable disable

namespace meterapi.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("meterapi.Models.Meter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MeterAId")
                        .HasColumnType("int");

                    b.Property<int>("RpId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Meters");
                });

            modelBuilder.Entity("meterapi.Models.MeterData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("AllPhaseConsumption")
                        .HasColumnType("real");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<float>("GasConsumption")
                        .HasColumnType("real");

                    b.Property<int>("MeterId")
                        .HasColumnType("int");

                    b.Property<float>("TotalConsumptionDay")
                        .HasColumnType("real");

                    b.Property<float>("TotalConsumptionNight")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("MeterId");

                    b.ToTable("MeterDatas");
                });

            modelBuilder.Entity("meterapi.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TokenCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TokenExpires")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("meterapi.Models.UserMeter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MeterAId")
                        .HasColumnType("int");

                    b.Property<int>("MeterId")
                        .HasColumnType("int");

                    b.Property<int>("RpId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MeterId");

                    b.HasIndex("UserId");

                    b.ToTable("UserMeters");
                });

            modelBuilder.Entity("meterapi.Models.MeterData", b =>
                {
                    b.HasOne("meterapi.Models.Meter", "Meter")
                        .WithMany("MeterDatas")
                        .HasForeignKey("MeterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meter");
                });

            modelBuilder.Entity("meterapi.Models.UserMeter", b =>
                {
                    b.HasOne("meterapi.Models.Meter", "Meter")
                        .WithMany("UserMeter")
                        .HasForeignKey("MeterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("meterapi.Models.User", "User")
                        .WithMany("UserMeters")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meter");

                    b.Navigation("User");
                });

            modelBuilder.Entity("meterapi.Models.Meter", b =>
                {
                    b.Navigation("MeterDatas");

                    b.Navigation("UserMeter");
                });

            modelBuilder.Entity("meterapi.Models.User", b =>
                {
                    b.Navigation("UserMeters");
                });
#pragma warning restore 612, 618
        }
    }
}