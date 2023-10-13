﻿// <auto-generated />
using System;
using ChawlaClinic.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChawlaClinic.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231003155140_payment secure token added")]
    partial class paymentsecuretokenadded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ChawlaClinic.DAL.Entities.Patient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("AddedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("AddedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("AgeMonths")
                        .HasColumnType("int");

                    b.Property<int>("AgeYears")
                        .HasColumnType("int");

                    b.Property<string>("CaseNo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("DiscountId")
                        .HasColumnType("int");

                    b.Property<string>("Disease")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateOnly>("FirstVisit")
                        .HasColumnType("date");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("varchar(1)");

                    b.Property<string>("GuardianName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("ModifiedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(1)");

                    b.HasKey("Id");

                    b.HasIndex("DiscountId");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("ChawlaClinic.DAL.Entities.PatientDiscount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AddedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("AddedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("ModifiedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("PatientDiscounts");
                });

            modelBuilder.Entity("ChawlaClinic.DAL.Entities.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("AddedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("AddedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("AmountPaid")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("ModifiedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("char(36)");

                    b.Property<DateOnly>("PaymentDate")
                        .HasColumnType("date");

                    b.Property<string>("SecureToken")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("TokenID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("ChawlaClinic.DAL.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AddedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("AddedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("JwtToken")
                        .HasColumnType("longtext");

                    b.Property<int?>("ModifiedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChawlaClinic.DAL.Entities.Patient", b =>
                {
                    b.HasOne("ChawlaClinic.DAL.Entities.PatientDiscount", "PatientDiscount")
                        .WithMany("Patients")
                        .HasForeignKey("DiscountId");

                    b.Navigation("PatientDiscount");
                });

            modelBuilder.Entity("ChawlaClinic.DAL.Entities.Payment", b =>
                {
                    b.HasOne("ChawlaClinic.DAL.Entities.Patient", "Patient")
                        .WithMany("Payments")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("ChawlaClinic.DAL.Entities.Patient", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("ChawlaClinic.DAL.Entities.PatientDiscount", b =>
                {
                    b.Navigation("Patients");
                });
#pragma warning restore 612, 618
        }
    }
}
