﻿// <auto-generated />
using System;
using FirstEF6App;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GameServer.Migrations
{
    [DbContext(typeof(ColorChessContext))]
    [Migration("20231109141832_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("GameStatistic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("GameMode")
                        .HasColumnType("int");

                    b.Property<string>("PlayerScore")
                        .HasColumnType("longtext");

                    b.Property<string>("UsersId")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("GameStatistics");
                });

            modelBuilder.Entity("LogEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<int?>("Type_Event")
                        .HasColumnType("int");

                    b.Property<string>("UsersId")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("LogEvents");
                });

            modelBuilder.Entity("User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UserStatistic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Draw")
                        .HasColumnType("int");

                    b.Property<int>("Lose")
                        .HasColumnType("int");

                    b.Property<int>("MaxScore")
                        .HasColumnType("int");

                    b.Property<int>("Rate")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("Win")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserStatistics");
                });
#pragma warning restore 612, 618
        }
    }
}