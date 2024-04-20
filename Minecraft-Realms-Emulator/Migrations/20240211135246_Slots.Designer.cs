﻿// <auto-generated />
using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Minecraft_Realms_Emulator.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Minecraft_Realms_Emulator.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240211135246_Slots")]
    partial class Slots
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Minecraft_Realms_Emulator.Entities.Subscription", b =>
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

            modelBuilder.Entity("Minecraft_Realms_Emulator.Entities.World", b =>
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
#pragma warning restore 612, 618
        }
    }
}
