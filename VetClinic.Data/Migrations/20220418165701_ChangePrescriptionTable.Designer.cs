﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VetClinic.Data;

#nullable disable

namespace VetClinic.Data.Migrations
{
    [DbContext(typeof(VetClinicDbContext))]
    [Migration("20220418165701_ChangePrescriptionTable")]
    partial class ChangePrescriptionTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("VetClinic.Data.Models.Appointment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<string>("DoctorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Hour")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PetId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PrescriptionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("DoctorId");

                    b.HasIndex("PetId");

                    b.HasIndex("PrescriptionId");

                    b.HasIndex("ServiceId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("VetClinic.Data.Models.Client", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("VetClinic.Data.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("VetClinic.Data.Models.Doctor", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("ProfileImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("VetClinic.Data.Models.DoctorService", b =>
                {
                    b.Property<string>("DoctorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("DoctorId", "ServiceId");

                    b.HasIndex("ServiceId");

                    b.ToTable("DoctorServices");
                });

            modelBuilder.Entity("VetClinic.Data.Models.Pet", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Breed")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PetTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("PetTypeId");

                    b.ToTable("Pets");
                });

            modelBuilder.Entity("VetClinic.Data.Models.PetDoctor", b =>
                {
                    b.Property<string>("PetId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DoctorId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PetId", "DoctorId");

                    b.HasIndex("DoctorId");

                    b.ToTable("PetDoctors");
                });

            modelBuilder.Entity("VetClinic.Data.Models.PetService", b =>
                {
                    b.Property<string>("PetId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("PetId", "ServiceId");

                    b.HasIndex("ServiceId");

                    b.ToTable("PetServices");
                });

            modelBuilder.Entity("VetClinic.Data.Models.PetType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("PetTypes");
                });

            modelBuilder.Entity("VetClinic.Data.Models.Prescription", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AppointmentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("DoctorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("bit");

                    b.Property<string>("PetId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AppointmentId");

                    b.HasIndex("DoctorId");

                    b.HasIndex("PetId");

                    b.ToTable("Prescriptions");
                });

            modelBuilder.Entity("VetClinic.Data.Models.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("VetClinic.Data.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("VetClinic.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("VetClinic.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VetClinic.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("VetClinic.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VetClinic.Data.Models.Appointment", b =>
                {
                    b.HasOne("VetClinic.Data.Models.Client", "Client")
                        .WithMany("Appointments")
                        .HasForeignKey("ClientId");

                    b.HasOne("VetClinic.Data.Models.Doctor", "Doctor")
                        .WithMany("Appointments")
                        .HasForeignKey("DoctorId");

                    b.HasOne("VetClinic.Data.Models.Pet", "Pet")
                        .WithMany("Appointments")
                        .HasForeignKey("PetId");

                    b.HasOne("VetClinic.Data.Models.Prescription", "Prescription")
                        .WithMany()
                        .HasForeignKey("PrescriptionId");

                    b.HasOne("VetClinic.Data.Models.Service", "Service")
                        .WithMany("Appointments")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Doctor");

                    b.Navigation("Pet");

                    b.Navigation("Prescription");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("VetClinic.Data.Models.Client", b =>
                {
                    b.HasOne("VetClinic.Data.Models.User", null)
                        .WithOne()
                        .HasForeignKey("VetClinic.Data.Models.Client", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("VetClinic.Data.Models.Doctor", b =>
                {
                    b.HasOne("VetClinic.Data.Models.Department", "Department")
                        .WithMany("Doctors")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VetClinic.Data.Models.User", null)
                        .WithOne()
                        .HasForeignKey("VetClinic.Data.Models.Doctor", "UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Department");
                });

            modelBuilder.Entity("VetClinic.Data.Models.DoctorService", b =>
                {
                    b.HasOne("VetClinic.Data.Models.Doctor", "Doctor")
                        .WithMany("DoctorServices")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VetClinic.Data.Models.Service", "Service")
                        .WithMany("DoctorServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("VetClinic.Data.Models.Pet", b =>
                {
                    b.HasOne("VetClinic.Data.Models.Client", "Client")
                        .WithMany("Pets")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VetClinic.Data.Models.PetType", "PetType")
                        .WithMany("Pets")
                        .HasForeignKey("PetTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("PetType");
                });

            modelBuilder.Entity("VetClinic.Data.Models.PetDoctor", b =>
                {
                    b.HasOne("VetClinic.Data.Models.Doctor", "Doctor")
                        .WithMany("PetDoctors")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VetClinic.Data.Models.Pet", "Pet")
                        .WithMany("PetDoctors")
                        .HasForeignKey("PetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Pet");
                });

            modelBuilder.Entity("VetClinic.Data.Models.PetService", b =>
                {
                    b.HasOne("VetClinic.Data.Models.Pet", "Pet")
                        .WithMany("PetServices")
                        .HasForeignKey("PetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VetClinic.Data.Models.Service", "Service")
                        .WithMany("PetServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Pet");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("VetClinic.Data.Models.Prescription", b =>
                {
                    b.HasOne("VetClinic.Data.Models.Appointment", "Appointment")
                        .WithMany()
                        .HasForeignKey("AppointmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VetClinic.Data.Models.Doctor", "Doctor")
                        .WithMany("Prescriptions")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VetClinic.Data.Models.Pet", "Pet")
                        .WithMany("Prescriptions")
                        .HasForeignKey("PetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Appointment");

                    b.Navigation("Doctor");

                    b.Navigation("Pet");
                });

            modelBuilder.Entity("VetClinic.Data.Models.Service", b =>
                {
                    b.HasOne("VetClinic.Data.Models.Department", "Department")
                        .WithMany("Services")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("VetClinic.Data.Models.Client", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Pets");
                });

            modelBuilder.Entity("VetClinic.Data.Models.Department", b =>
                {
                    b.Navigation("Doctors");

                    b.Navigation("Services");
                });

            modelBuilder.Entity("VetClinic.Data.Models.Doctor", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("DoctorServices");

                    b.Navigation("PetDoctors");

                    b.Navigation("Prescriptions");
                });

            modelBuilder.Entity("VetClinic.Data.Models.Pet", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("PetDoctors");

                    b.Navigation("PetServices");

                    b.Navigation("Prescriptions");
                });

            modelBuilder.Entity("VetClinic.Data.Models.PetType", b =>
                {
                    b.Navigation("Pets");
                });

            modelBuilder.Entity("VetClinic.Data.Models.Service", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("DoctorServices");

                    b.Navigation("PetServices");
                });
#pragma warning restore 612, 618
        }
    }
}
