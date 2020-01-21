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
    [Migration("20200121184527_DatabaseConfigChanges1")]
    partial class DatabaseConfigChanges1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SmartFridgeApp.Domain.FoodProducts.FoodProduct", b =>
                {
                    b.Property<int>("FoodProductId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("FoodProductId");

                    b.ToTable("FoodProducts");
                });

            modelBuilder.Entity("SmartFridgeApp.Domain.Fridges.Fridge", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .HasColumnName("Address");

                    b.Property<string>("Desc")
                        .HasColumnName("Desc");

                    b.Property<string>("Name")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.ToTable("Fridges","app");
                });

            modelBuilder.Entity("SmartFridgeApp.Domain.RecipeFoodProducts.RecipeFoodProduct", b =>
                {
                    b.Property<int>("FoodProductId");

                    b.Property<int>("RecipeId");

                    b.HasKey("FoodProductId", "RecipeId");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeFoodProduct");
                });

            modelBuilder.Entity("SmartFridgeApp.Domain.Recipes.Recipe", b =>
                {
                    b.Property<int>("RecipeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("RecipeId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("SmartFridgeApp.Domain.Fridges.Fridge", b =>
                {
                    b.OwnsMany("SmartFridgeApp.Domain.Users.User", "_users", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd();

                            b1.Property<string>("Email")
                                .HasColumnName("Email");

                            b1.Property<Guid>("FridgeId");

                            b1.Property<string>("Name")
                                .HasColumnName("Name");

                            b1.Property<DateTime>("_createdAt")
                                .HasColumnName("CreatedAt");

                            b1.Property<bool>("_welcomeEmailWasSent")
                                .HasColumnName("WelcomeEmailWasSent");

                            b1.HasKey("Id");

                            b1.HasIndex("FridgeId");

                            b1.ToTable("Users","app");

                            b1.HasOne("SmartFridgeApp.Domain.Fridges.Fridge")
                                .WithMany("_users")
                                .HasForeignKey("FridgeId")
                                .OnDelete(DeleteBehavior.Cascade);

                            b1.OwnsMany("SmartFridgeApp.Domain.FridgeItems.FridgeItem", "_fridgeItems", b2 =>
                                {
                                    b2.Property<Guid>("Id")
                                        .ValueGeneratedOnAdd();

                                    b2.Property<string>("Category")
                                        .IsRequired();

                                    b2.Property<string>("Desc")
                                        .HasColumnName("Desc");

                                    b2.Property<DateTime>("EnteredAt")
                                        .HasColumnName("EnteredAt");

                                    b2.Property<DateTime>("ExpirationDate")
                                        .HasColumnName("ExpirationDate");

                                    b2.Property<int?>("FoodProductId");

                                    b2.Property<bool>("IsConsumed")
                                        .HasColumnName("IsConsumed");

                                    b2.Property<Guid>("UserId");

                                    b2.HasKey("Id");

                                    b2.HasIndex("FoodProductId");

                                    b2.HasIndex("UserId");

                                    b2.ToTable("FridgeItems","app");

                                    b2.HasOne("SmartFridgeApp.Domain.FoodProducts.FoodProduct", "FoodProduct")
                                        .WithMany()
                                        .HasForeignKey("FoodProductId");

                                    b2.HasOne("SmartFridgeApp.Domain.Users.User")
                                        .WithMany("_fridgeItems")
                                        .HasForeignKey("UserId")
                                        .OnDelete(DeleteBehavior.Cascade);

                                    b2.OwnsOne("SmartFridgeApp.Domain.Shared.AmountValue", "AmountValue", b3 =>
                                        {
                                            b3.Property<Guid>("FridgeItemId");

                                            b3.Property<string>("Unit")
                                                .IsRequired()
                                                .HasColumnName("Unit");

                                            b3.Property<float>("Value")
                                                .HasColumnName("Value");

                                            b3.HasKey("FridgeItemId");

                                            b3.ToTable("FridgeItems","app");

                                            b3.HasOne("SmartFridgeApp.Domain.FridgeItems.FridgeItem")
                                                .WithOne("AmountValue")
                                                .HasForeignKey("SmartFridgeApp.Domain.Shared.AmountValue", "FridgeItemId")
                                                .OnDelete(DeleteBehavior.Cascade);
                                        });
                                });
                        });
                });

            modelBuilder.Entity("SmartFridgeApp.Domain.RecipeFoodProducts.RecipeFoodProduct", b =>
                {
                    b.HasOne("SmartFridgeApp.Domain.FoodProducts.FoodProduct", "FoodProduct")
                        .WithMany("RecipeFoodProducts")
                        .HasForeignKey("FoodProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartFridgeApp.Domain.Recipes.Recipe", "Recipe")
                        .WithMany("RecipeFoodProducts")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
