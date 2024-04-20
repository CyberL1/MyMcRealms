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
    [Migration("20240213104200_Connections")]
    partial class Connections
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MyMcRealms.Entities.Connection", b =>
                {
                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("PendingUpdate")
                        .HasColumnType("boolean");

                    b.Property<int>("WorldId")
                        .HasColumnType("integer");

                    b.HasIndex("WorldId");

                    b.ToTable("Connections");
                });

            modelBuilder.Entity("MyMcRealms.Entities.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DaysLeft")
                        .HasColumnType("integer");

                    b.Property<string>("RemoteId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StartDate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SubscriptionType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

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

                    b.Property<int>("DaysLeft")
                        .HasColumnType("integer");

                    b.Property<bool>("Expired")
                        .HasColumnType("boolean");

                    b.Property<bool>("ExpiredTrial")
                        .HasColumnType("boolean");

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

                    b.Property<string[]>("Players")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("RemoteSubscriptionId")
                        .IsRequired()
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

            modelBuilder.Entity("MyMcRealms.Entities.Connection", b =>
                {
                    b.HasOne("MyMcRealms.Entities.World", "World")
                        .WithMany()
                        .HasForeignKey("WorldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("World");
                });
#pragma warning restore 612, 618
        }
    }
}
