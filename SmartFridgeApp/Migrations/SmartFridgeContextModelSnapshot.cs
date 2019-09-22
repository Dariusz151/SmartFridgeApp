﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartFridgeApp.Persistence;

namespace SmartFridgeApp.Migrations
{
    [DbContext(typeof(SmartFridgeContext))]
    partial class SmartFridgeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SmartFridgeApp.Domain.Models.FoodItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<int>("Category");

                    b.Property<DateTime>("EnteredAt");

                    b.Property<DateTime>("ExpirationDate");

                    b.Property<string>("Name");

                    b.Property<int>("Unit");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("FoodItems");
                });

            modelBuilder.Entity("SmartFridgeApp.Domain.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Login");

                    b.Property<int>("Role");

                    b.Property<Guid?>("UserGroupId");

                    b.HasKey("Id");

                    b.HasIndex("UserGroupId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SmartFridgeApp.Domain.Models.UserGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("SmartFridgeApp.Persistence.ConsumedFoodItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount");

                    b.Property<DateTime>("ConsumedAt");

                    b.Property<string>("FoodItemName");

                    b.Property<int>("Unit");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.ToTable("ConsumedFoodItems");
                });

            modelBuilder.Entity("SmartFridgeApp.Domain.Models.FoodItem", b =>
                {
                    b.HasOne("SmartFridgeApp.Domain.Models.User")
                        .WithMany("FoodItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartFridgeApp.Domain.Models.User", b =>
                {
                    b.HasOne("SmartFridgeApp.Domain.Models.UserGroup")
                        .WithMany("Users")
                        .HasForeignKey("UserGroupId");
                });
#pragma warning restore 612, 618
        }
    }
}
