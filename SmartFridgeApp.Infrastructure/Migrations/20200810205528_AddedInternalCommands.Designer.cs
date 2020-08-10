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
    [Migration("20200810205528_AddedInternalCommands")]
    partial class AddedInternalCommands
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SmartFridgeApp.Domain.Models.FoodProducts.Category", b =>
                {
                    b.Property<short>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(25);

                    b.HasKey("CategoryId");

                    b.ToTable("Categories","app");
                });

            modelBuilder.Entity("SmartFridgeApp.Domain.Models.FoodProducts.FoodProduct", b =>
                {
                    b.Property<short>("FoodProductId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<short?>("CategoryId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(40);

                    b.HasKey("FoodProductId");

                    b.HasIndex("CategoryId");

                    b.ToTable("FoodProducts","app");
                });

            modelBuilder.Entity("SmartFridgeApp.Domain.Models.Fridges.Fridge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnName("Address")
                        .HasMaxLength(100);

                    b.Property<string>("Desc")
                        .HasColumnName("Desc")
                        .HasMaxLength(250);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Fridges","app");
                });

            modelBuilder.Entity("SmartFridgeApp.Domain.Models.Recipes.Recipe", b =>
                {
                    b.Property<int>("RecipeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnName("Description")
                        .HasMaxLength(5000);

                    b.Property<string>("FoodProducts")
                        .HasColumnName("FoodProducts");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(100);

                    b.Property<string>("Type")
                        .HasColumnName("Type");

                    b.HasKey("RecipeId");

                    b.ToTable("Recipes","app");
                });

            modelBuilder.Entity("SmartFridgeApp.Infrastructure.InternalCommands.InternalCommand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Data");

                    b.Property<DateTime?>("ProcessedDate");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("InternalCommands");
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
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnName("Email")
                                .HasMaxLength(250);

                            b1.Property<int>("FridgeId");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnName("Name")
                                .HasMaxLength(100);

                            b1.Property<DateTime>("_createdAt")
                                .HasColumnName("CreatedAt");

                            b1.HasKey("Id");

                            b1.HasIndex("FridgeId");

                            b1.ToTable("Users","app");

                            b1.HasOne("SmartFridgeApp.Domain.Models.Fridges.Fridge")
                                .WithMany("_users")
                                .HasForeignKey("FridgeId")
                                .OnDelete(DeleteBehavior.Cascade);

                            b1.OwnsMany("SmartFridgeApp.Domain.Models.FridgeItems.FridgeItem", "_fridgeItems", b2 =>
                                {
                                    b2.Property<long>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                                    b2.Property<DateTime>("EnteredAt")
                                        .HasColumnName("EnteredAt");

                                    b2.Property<DateTime>("ExpirationDate")
                                        .HasColumnName("ExpirationDate");

                                    b2.Property<short?>("FoodProductId");

                                    b2.Property<bool>("IsConsumed")
                                        .HasColumnName("IsConsumed");

                                    b2.Property<string>("Note")
                                        .HasColumnName("Note")
                                        .HasMaxLength(1000);

                                    b2.Property<int>("UserId");

                                    b2.HasKey("Id");

                                    b2.HasIndex("FoodProductId");

                                    b2.HasIndex("UserId");

                                    b2.ToTable("FridgeItems","app");

                                    b2.HasOne("SmartFridgeApp.Domain.Models.FoodProducts.FoodProduct", "FoodProduct")
                                        .WithMany()
                                        .HasForeignKey("FoodProductId");

                                    b2.HasOne("SmartFridgeApp.Domain.Models.Users.User")
                                        .WithMany("_fridgeItems")
                                        .HasForeignKey("UserId")
                                        .OnDelete(DeleteBehavior.Cascade);

                                    b2.OwnsOne("SmartFridgeApp.Domain.Shared.AmountValue", "AmountValue", b3 =>
                                        {
                                            b3.Property<long>("FridgeItemId")
                                                .ValueGeneratedOnAdd()
                                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                                            b3.Property<string>("Unit")
                                                .IsRequired()
                                                .HasColumnName("Unit");

                                            b3.Property<float>("Value")
                                                .HasColumnName("Value");

                                            b3.HasKey("FridgeItemId");

                                            b3.ToTable("FridgeItems","app");

                                            b3.HasOne("SmartFridgeApp.Domain.Models.FridgeItems.FridgeItem")
                                                .WithOne("AmountValue")
                                                .HasForeignKey("SmartFridgeApp.Domain.Shared.AmountValue", "FridgeItemId")
                                                .OnDelete(DeleteBehavior.Cascade);
                                        });
                                });
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
