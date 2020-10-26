﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartFridgeApp.Infrastructure;

namespace SmartFridgeApp.Infrastructure.Migrations
{
    [DbContext(typeof(SmartFridgeAppContext))]
    [Migration("20201026220426_ChangedForeignKey")]
    partial class ChangedForeignKey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SmartFridgeApp.Domain.Models.FoodProducts.Category", b =>
                {
                    b.Property<short>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.HasKey("CategoryId");

                    b.ToTable("Categories","app");
                });

            modelBuilder.Entity("SmartFridgeApp.Domain.Models.FoodProducts.FoodProduct", b =>
                {
                    b.Property<short>("FoodProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<short?>("CategoryId")
                        .HasColumnType("smallint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.HasKey("FoodProductId");

                    b.HasIndex("CategoryId");

                    b.ToTable("FoodProducts","app");
                });

            modelBuilder.Entity("SmartFridgeApp.Domain.Models.Fridges.Fridge", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnName("Address")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Desc")
                        .HasColumnName("Desc")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<bool>("IsWelcomed")
                        .HasColumnName("IsWelcomed")
                        .HasColumnType("bit")
                        .HasMaxLength(10);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Fridges","app");
                });

            modelBuilder.Entity("SmartFridgeApp.Domain.Models.Recipes.Recipe", b =>
                {
                    b.Property<Guid>("RecipeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnName("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasMaxLength(5000);

                    b.Property<string>("FoodProducts")
                        .HasColumnName("FoodProducts")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<short?>("RecipeCategoryId")
                        .HasColumnType("smallint");

                    b.HasKey("RecipeId");

                    b.HasIndex("RecipeCategoryId");

                    b.ToTable("Recipes","app");
                });

            modelBuilder.Entity("SmartFridgeApp.Domain.Models.Recipes.RecipeCategory", b =>
                {
                    b.Property<short>("RecipeCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.HasKey("RecipeCategoryId");

                    b.ToTable("RecipeCategories","app");
                });

            modelBuilder.Entity("SmartFridgeApp.Infrastructure.InternalCommands.InternalCommand", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EnqueueDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ProcessedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("InternalCommands","app");
                });

            modelBuilder.Entity("SmartFridgeApp.Infrastructure.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OccurredOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ProcessedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages","app");
                });

            modelBuilder.Entity("SmartFridgeApp.Domain.Models.FoodProducts.FoodProduct", b =>
                {
                    b.HasOne("SmartFridgeApp.Domain.Models.FoodProducts.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");
                });

            modelBuilder.Entity("SmartFridgeApp.Domain.Models.Fridges.Fridge", b =>
                {
                    b.OwnsMany("SmartFridgeApp.Domain.Models.Users.User", "_users", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnName("Email")
                                .HasColumnType("nvarchar(250)")
                                .HasMaxLength(250);

                            b1.Property<Guid>("FridgeId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnName("Name")
                                .HasColumnType("nvarchar(100)")
                                .HasMaxLength(100);

                            b1.Property<DateTime>("_createdAt")
                                .HasColumnName("CreatedAt")
                                .HasColumnType("datetime2");

                            b1.HasKey("Id");

                            b1.HasIndex("FridgeId");

                            b1.ToTable("Users","app");

                            b1.WithOwner()
                                .HasForeignKey("FridgeId");

                            b1.OwnsMany("SmartFridgeApp.Domain.Models.FridgeItems.FridgeItem", "_fridgeItems", b2 =>
                                {
                                    b2.Property<long>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("bigint")
                                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                                    b2.Property<DateTime>("EnteredAt")
                                        .HasColumnName("EnteredAt")
                                        .HasColumnType("datetime2");

                                    b2.Property<DateTime>("ExpirationDate")
                                        .HasColumnName("ExpirationDate")
                                        .HasColumnType("datetime2");

                                    b2.Property<short?>("FoodProductId")
                                        .HasColumnType("smallint");

                                    b2.Property<bool>("IsConsumed")
                                        .HasColumnName("IsConsumed")
                                        .HasColumnType("bit");

                                    b2.Property<string>("Note")
                                        .HasColumnName("Note")
                                        .HasColumnType("nvarchar(1000)")
                                        .HasMaxLength(1000);

                                    b2.Property<Guid>("UserId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.HasKey("Id");

                                    b2.HasIndex("FoodProductId");

                                    b2.HasIndex("UserId");

                                    b2.ToTable("FridgeItems","app");

                                    b2.HasOne("SmartFridgeApp.Domain.Models.FoodProducts.FoodProduct", "FoodProduct")
                                        .WithMany()
                                        .HasForeignKey("FoodProductId");

                                    b2.WithOwner()
                                        .HasForeignKey("UserId");

                                    b2.OwnsOne("SmartFridgeApp.Domain.Shared.AmountValue", "AmountValue", b3 =>
                                        {
                                            b3.Property<long>("FridgeItemId")
                                                .ValueGeneratedOnAdd()
                                                .HasColumnType("bigint")
                                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                                            b3.Property<string>("Unit")
                                                .IsRequired()
                                                .HasColumnName("Unit")
                                                .HasColumnType("nvarchar(max)");

                                            b3.Property<float>("Value")
                                                .HasColumnName("Value")
                                                .HasColumnType("real");

                                            b3.HasKey("FridgeItemId");

                                            b3.ToTable("FridgeItems");

                                            b3.WithOwner()
                                                .HasForeignKey("FridgeItemId");
                                        });
                                });
                        });
                });

            modelBuilder.Entity("SmartFridgeApp.Domain.Models.Recipes.Recipe", b =>
                {
                    b.HasOne("SmartFridgeApp.Domain.Models.Recipes.RecipeCategory", "RecipeCategory")
                        .WithMany()
                        .HasForeignKey("RecipeCategoryId");
                });
#pragma warning restore 612, 618
        }
    }
}
