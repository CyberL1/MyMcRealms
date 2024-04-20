﻿// <auto-generated />
using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyMcRealms.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyMcRealms.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240317120329_Worlds_Subscription_field")]
    partial class Worlds_Subscription_field
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MyMcRealms.Entities.Backup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BackupId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("LastModifiedDate")
                        .HasColumnType("bigint");

                    b.Property<JsonDocument>("Metadata")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("Size")
                        .HasColumnType("integer");

                    b.Property<int>("WorldId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("WorldId");

                    b.ToTable("Backups");
                });

            modelBuilder.Entity("MyMcRealms.Entities.Configuration", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Key");

                    b.ToTable("Configuration");
                });

            modelBuilder.Entity("MyMcRealms.Entities.Connection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("PendingUpdate")
                        .HasColumnType("boolean");

                    b.Property<int>("WorldId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("WorldId");

                    b.ToTable("Connections");
                });

            modelBuilder.Entity("MyMcRealms.Entities.Invite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("InvitationId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RecipeintUUID")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("WorldId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("WorldId");

                    b.ToTable("Invites");
                });

            modelBuilder.Entity("MyMcRealms.Entities.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Accepted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Online")
                        .HasColumnType("boolean");

                    b.Property<bool>("Operator")
                        .HasColumnType("boolean");

                    b.Property<string>("Permission")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Uuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("WorldId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("WorldId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("MyMcRealms.Entities.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SubscriptionType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("WorldId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("WorldId")
                        .IsUnique();

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("MyMcRealms.Entities.World", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ActiveSlot")
                        .HasColumnType("integer");

                    b.Property<int>("MaxPlayers")
                        .HasColumnType("integer");

                    b.Property<bool>("Member")
                        .HasColumnType("boolean");

                    b.Property<int?>("MinigameId")
                        .HasColumnType("integer");

                    b.Property<string>("MinigameImage")
                        .HasColumnType("text");

                    b.Property<string>("MinigameName")
                        .HasColumnType("text");

                    b.Property<string>("Motd")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Owner")
                        .HasColumnType("text");

                    b.Property<string>("OwnerUUID")
                        .HasColumnType("text");

                    b.Property<JsonDocument[]>("Slots")
                        .IsRequired()
                        .HasColumnType("jsonb[]");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("WorldType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Worlds");
                });

            modelBuilder.Entity("MyMcRealms.Entities.Backup", b =>
                {
                    b.HasOne("MyMcRealms.Entities.World", "World")
                        .WithMany()
                        .HasForeignKey("WorldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("World");
                });

            modelBuilder.Entity("MyMcRealms.Entities.Connection", b =>
                {
                    b.HasOne("MyMcRealms.Entities.World", "World")
                        .WithMany()
                        .HasForeignKey("WorldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("World");
                });

            modelBuilder.Entity("MyMcRealms.Entities.Invite", b =>
                {
                    b.HasOne("MyMcRealms.Entities.World", "World")
                        .WithMany()
                        .HasForeignKey("WorldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("World");
                });

            modelBuilder.Entity("MyMcRealms.Entities.Player", b =>
                {
                    b.HasOne("MyMcRealms.Entities.World", "World")
                        .WithMany("Players")
                        .HasForeignKey("WorldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("World");
                });

            modelBuilder.Entity("MyMcRealms.Entities.Subscription", b =>
                {
                    b.HasOne("MyMcRealms.Entities.World", "World")
                        .WithOne("Subscription")
                        .HasForeignKey("MyMcRealms.Entities.Subscription", "WorldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("World");
                });

            modelBuilder.Entity("MyMcRealms.Entities.World", b =>
                {
                    b.Navigation("Players");

                    b.Navigation("Subscription");
                });
#pragma warning restore 612, 618
        }
    }
}
