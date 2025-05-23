﻿// <auto-generated />
using System;
using BlockChainIdentity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlockChainIdentity.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BlockChainIdentity.Domain.Entities.Client", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClientSecret")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Statement")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TokenLifeTimeInSeconds")
                        .HasColumnType("int");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("Client", (string)null);
                });

            modelBuilder.Entity("BlockChainIdentity.Domain.Entities.ClientResource", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("ClientId")
                        .HasColumnType("bigint");

                    b.Property<long>("ResourceId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("ResourceId");

                    b.ToTable("ClientResource", (string)null);
                });

            modelBuilder.Entity("BlockChainIdentity.Domain.Entities.Resource", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Resource", (string)null);
                });

            modelBuilder.Entity("BlockChainIdentity.Domain.Entities.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("BlockChainIdentity.Domain.Entities.RpcUrl", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("Chain")
                        .HasColumnType("int");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RpcUrl", (string)null);
                });

            modelBuilder.Entity("BlockChainIdentity.Domain.Entities.Token", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ExpireAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nonce")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NotBefore")
                        .HasColumnType("datetime2");

                    b.Property<string>("RequestId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Resources")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WalletAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("statement")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("version")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("WalletAddress");

                    b.ToTable("Token", (string)null);
                });

            modelBuilder.Entity("BlockChainIdentity.Domain.Entities.Wallet", b =>
                {
                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ChainId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Address");

                    b.HasIndex("Address", "ChainId")
                        .IsUnique();

                    b.ToTable("Wallet", (string)null);
                });

            modelBuilder.Entity("BlockChainIdentity.Domain.Entities.WalletRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.Property<string>("WalletAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("WalletAddress");

                    b.ToTable("WalletRole", (string)null);
                });

            modelBuilder.Entity("BlockChainIdentity.Domain.Entities.ClientResource", b =>
                {
                    b.HasOne("BlockChainIdentity.Domain.Entities.Client", "Client")
                        .WithMany("ClientResources")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlockChainIdentity.Domain.Entities.Resource", "Resource")
                        .WithMany("ClientResources")
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Resource");
                });

            modelBuilder.Entity("BlockChainIdentity.Domain.Entities.Token", b =>
                {
                    b.HasOne("BlockChainIdentity.Domain.Entities.Wallet", "Wallet")
                        .WithMany("Tokens")
                        .HasForeignKey("WalletAddress")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("BlockChainIdentity.Domain.Entities.WalletRole", b =>
                {
                    b.HasOne("BlockChainIdentity.Domain.Entities.Role", "Role")
                        .WithMany("WalletRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlockChainIdentity.Domain.Entities.Wallet", "Wallet")
                        .WithMany("WalletRoles")
                        .HasForeignKey("WalletAddress")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("BlockChainIdentity.Domain.Entities.Client", b =>
                {
                    b.Navigation("ClientResources");
                });

            modelBuilder.Entity("BlockChainIdentity.Domain.Entities.Resource", b =>
                {
                    b.Navigation("ClientResources");
                });

            modelBuilder.Entity("BlockChainIdentity.Domain.Entities.Role", b =>
                {
                    b.Navigation("WalletRoles");
                });

            modelBuilder.Entity("BlockChainIdentity.Domain.Entities.Wallet", b =>
                {
                    b.Navigation("Tokens");

                    b.Navigation("WalletRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
