﻿// <auto-generated />
using System;
using Informing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Informing.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Informing.Domain.Entities.BaseParameter", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"));

                    b.Property<int>("category")
                        .HasColumnType("int");

                    b.Property<DateTime>("created")
                        .HasColumnType("datetime2");

                    b.Property<string>("createdBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("field")
                        .HasColumnType("int");

                    b.Property<long>("kernelBaseParameterId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("lastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("lastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("value")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.HasKey("id");

                    b.HasIndex("field")
                        .IsUnique();

                    b.ToTable("BaseParameter", (string)null);
                });

            modelBuilder.Entity("Informing.Domain.Entities.Contact", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"));

                    b.Property<DateTime>("created")
                        .HasColumnType("datetime2");

                    b.Property<string>("createdBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("emailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fullname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("lastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("lastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("userId")
                        .IsUnique();

                    b.ToTable("Contact", (string)null);
                });

            modelBuilder.Entity("Informing.Domain.Entities.ContactInformation", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"));

                    b.Property<long>("contactId")
                        .HasColumnType("bigint");

                    b.Property<long>("infromationId")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.HasIndex("contactId");

                    b.HasIndex("infromationId");

                    b.ToTable("ContactInformation", (string)null);
                });

            modelBuilder.Entity("Informing.Domain.Entities.Device", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"));

                    b.Property<long?>("Contactid")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("created")
                        .HasColumnType("datetime2");

                    b.Property<string>("createdBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("deviceToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("enabled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("lastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("lastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("platformType")
                        .HasColumnType("int");

                    b.Property<string>("version")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("Contactid");

                    b.HasIndex("deviceToken")
                        .IsUnique();

                    b.ToTable("Device", (string)null);
                });

            modelBuilder.Entity("Informing.Domain.Entities.Group", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"));

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("id");

                    b.HasIndex("title")
                        .IsUnique();

                    b.ToTable("Group", (string)null);
                });

            modelBuilder.Entity("Informing.Domain.Entities.GroupContact", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"));

                    b.Property<long>("contactId")
                        .HasColumnType("bigint");

                    b.Property<long>("groupId")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.HasIndex("contactId");

                    b.HasIndex("groupId");

                    b.ToTable("GroupContact", (string)null);
                });

            modelBuilder.Entity("Informing.Domain.Entities.GroupInformation", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"));

                    b.Property<long>("groupId")
                        .HasColumnType("bigint");

                    b.Property<long>("infromationId")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.HasIndex("groupId");

                    b.HasIndex("infromationId");

                    b.ToTable("GroupInformation", (string)null);
                });

            modelBuilder.Entity("Informing.Domain.Entities.Information", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"));

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("created")
                        .HasColumnType("datetime2");

                    b.Property<string>("createdBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("enabled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("lastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("lastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("type")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("title");

                    b.ToTable("Information", (string)null);
                });

            modelBuilder.Entity("Informing.Domain.Entities.InformationLog", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"));

                    b.Property<long>("contactId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("created")
                        .HasColumnType("datetime2");

                    b.Property<string>("createdBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("informationId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("lastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("lastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("type")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("contactId");

                    b.HasIndex("informationId");

                    b.ToTable("InformationLog", (string)null);
                });

            modelBuilder.Entity("Informing.Domain.Entities.Template", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"));

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("informingSendType")
                        .HasColumnType("int");

                    b.Property<int>("informingType")
                        .HasColumnType("int");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Template", (string)null);
                });

            modelBuilder.Entity("Informing.Domain.Entities.ContactInformation", b =>
                {
                    b.HasOne("Informing.Domain.Entities.Contact", "contact")
                        .WithMany()
                        .HasForeignKey("contactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Informing.Domain.Entities.Information", "information")
                        .WithMany("contactInformations")
                        .HasForeignKey("infromationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("contact");

                    b.Navigation("information");
                });

            modelBuilder.Entity("Informing.Domain.Entities.Device", b =>
                {
                    b.HasOne("Informing.Domain.Entities.Contact", null)
                        .WithMany("devices")
                        .HasForeignKey("Contactid");
                });

            modelBuilder.Entity("Informing.Domain.Entities.GroupContact", b =>
                {
                    b.HasOne("Informing.Domain.Entities.Contact", "contact")
                        .WithMany("groupContacts")
                        .HasForeignKey("contactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Informing.Domain.Entities.Group", "group")
                        .WithMany("groupContacts")
                        .HasForeignKey("groupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("contact");

                    b.Navigation("group");
                });

            modelBuilder.Entity("Informing.Domain.Entities.GroupInformation", b =>
                {
                    b.HasOne("Informing.Domain.Entities.Group", "group")
                        .WithMany()
                        .HasForeignKey("groupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Informing.Domain.Entities.Information", "information")
                        .WithMany("groupInformations")
                        .HasForeignKey("infromationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("group");

                    b.Navigation("information");
                });

            modelBuilder.Entity("Informing.Domain.Entities.InformationLog", b =>
                {
                    b.HasOne("Informing.Domain.Entities.Contact", "contact")
                        .WithMany("informationLogs")
                        .HasForeignKey("contactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Informing.Domain.Entities.Information", "information")
                        .WithMany("informationLogs")
                        .HasForeignKey("informationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("contact");

                    b.Navigation("information");
                });

            modelBuilder.Entity("Informing.Domain.Entities.Contact", b =>
                {
                    b.Navigation("devices");

                    b.Navigation("groupContacts");

                    b.Navigation("informationLogs");
                });

            modelBuilder.Entity("Informing.Domain.Entities.Group", b =>
                {
                    b.Navigation("groupContacts");
                });

            modelBuilder.Entity("Informing.Domain.Entities.Information", b =>
                {
                    b.Navigation("contactInformations");

                    b.Navigation("groupInformations");

                    b.Navigation("informationLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
