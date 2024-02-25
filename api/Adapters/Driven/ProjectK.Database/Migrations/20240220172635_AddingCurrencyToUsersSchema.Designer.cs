﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProjectK.Database.Contexts;

#nullable disable

namespace ProjectK.Database.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240220172635_AddingCurrencyToUsersSchema")]
    partial class AddingCurrencyToUsersSchema
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ProjectK.Core.Entities.Category", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bytea")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at_utc");

                    b.Property<byte[]>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("created_by");

                    b.Property<string>("HexColor")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("#D1D5DB")
                        .HasColumnName("hex_color");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at_utc");

                    b.Property<byte[]>("UpdatedBy")
                        .HasColumnType("bytea")
                        .HasColumnName("updated_by");

                    b.Property<byte[]>("UserId")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_categories");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_categories_user_id");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("ProjectK.Core.Entities.CustomPlannedTransaction", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bytea")
                        .HasColumnName("id");

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("active");

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 4)
                        .HasColumnType("numeric(18,4)")
                        .HasColumnName("amount");

                    b.Property<byte[]>("BasePlannedTransactionId")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("base_planned_transaction_id");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at_utc");

                    b.Property<byte[]>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("created_by");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("description");

                    b.Property<DateOnly>("RefersTo")
                        .HasColumnType("date")
                        .HasColumnName("refers_to");

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at_utc");

                    b.Property<byte[]>("UpdatedBy")
                        .HasColumnType("bytea")
                        .HasColumnName("updated_by");

                    b.Property<byte[]>("UserId")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_custom_planned_transactions");

                    b.HasIndex("BasePlannedTransactionId")
                        .HasDatabaseName("ix_custom_planned_transactions_base_planned_transaction_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_custom_planned_transactions_user_id");

                    b.ToTable("custom_planned_transactions", (string)null);
                });

            modelBuilder.Entity("ProjectK.Core.Entities.PlannedTransaction", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bytea")
                        .HasColumnName("id");

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 4)
                        .HasColumnType("numeric(18,4)")
                        .HasColumnName("amount");

                    b.Property<int>("AmountType")
                        .HasColumnType("integer")
                        .HasColumnName("amount_type");

                    b.Property<byte[]>("CategoryId")
                        .HasColumnType("bytea")
                        .HasColumnName("category_id");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at_utc");

                    b.Property<byte[]>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("created_by");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("description");

                    b.Property<DateOnly?>("EndsAt")
                        .HasColumnType("date")
                        .HasColumnName("ends_at");

                    b.Property<int>("Recurrence")
                        .HasColumnType("integer")
                        .HasColumnName("recurrence");

                    b.Property<DateOnly>("StartsAt")
                        .HasColumnType("date")
                        .HasColumnName("starts_at");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at_utc");

                    b.Property<byte[]>("UpdatedBy")
                        .HasColumnType("bytea")
                        .HasColumnName("updated_by");

                    b.Property<byte[]>("UserId")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_planned_transactions");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("ix_planned_transactions_category_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_planned_transactions_user_id");

                    b.ToTable("planned_transactions", (string)null);
                });

            modelBuilder.Entity("ProjectK.Core.Entities.Transaction", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bytea")
                        .HasColumnName("id");

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 4)
                        .HasColumnType("numeric(18,4)")
                        .HasColumnName("amount");

                    b.Property<byte[]>("CategoryId")
                        .HasColumnType("bytea")
                        .HasColumnName("category_id");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at_utc");

                    b.Property<byte[]>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("created_by");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("description");

                    b.Property<DateOnly>("PaidAt")
                        .HasColumnType("date")
                        .HasColumnName("paid_at");

                    b.Property<byte[]>("PlannedTransactionId")
                        .HasColumnType("bytea")
                        .HasColumnName("planned_transaction_id");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at_utc");

                    b.Property<byte[]>("UpdatedBy")
                        .HasColumnType("bytea")
                        .HasColumnName("updated_by");

                    b.Property<byte[]>("UserId")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_transactions");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("ix_transactions_category_id");

                    b.HasIndex("PlannedTransactionId")
                        .HasDatabaseName("ix_transactions_planned_transaction_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_transactions_user_id");

                    b.ToTable("transactions", (string)null);
                });

            modelBuilder.Entity("ProjectK.Core.Entities.User", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bytea")
                        .HasColumnName("id");

                    b.Property<int>("Currency")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1)
                        .HasColumnName("currency");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(320)
                        .HasColumnType("character varying(320)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_users_email");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("ProjectK.Core.Entities.Category", b =>
                {
                    b.HasOne("ProjectK.Core.Entities.User", "User")
                        .WithMany("Categories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_categories_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectK.Core.Entities.CustomPlannedTransaction", b =>
                {
                    b.HasOne("ProjectK.Core.Entities.PlannedTransaction", "BasePlannedTransaction")
                        .WithMany("CustomPlannedTransactions")
                        .HasForeignKey("BasePlannedTransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_custom_planned_transactions_planned_transactions_base_plann");

                    b.HasOne("ProjectK.Core.Entities.User", "User")
                        .WithMany("CustomPlannedTransactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_custom_planned_transactions_users_user_id");

                    b.Navigation("BasePlannedTransaction");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectK.Core.Entities.PlannedTransaction", b =>
                {
                    b.HasOne("ProjectK.Core.Entities.Category", "Category")
                        .WithMany("PlannedTransactions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("fk_planned_transactions_categories_category_id");

                    b.HasOne("ProjectK.Core.Entities.User", "User")
                        .WithMany("PlannedTransactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_planned_transactions_users_user_id");

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectK.Core.Entities.Transaction", b =>
                {
                    b.HasOne("ProjectK.Core.Entities.Category", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("fk_transactions_categories_category_id");

                    b.HasOne("ProjectK.Core.Entities.PlannedTransaction", "PlannedTransaction")
                        .WithMany("Transactions")
                        .HasForeignKey("PlannedTransactionId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("fk_transactions_planned_transactions_planned_transaction_id");

                    b.HasOne("ProjectK.Core.Entities.User", "User")
                        .WithMany("Transactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_transactions_users_user_id");

                    b.Navigation("Category");

                    b.Navigation("PlannedTransaction");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectK.Core.Entities.Category", b =>
                {
                    b.Navigation("PlannedTransactions");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("ProjectK.Core.Entities.PlannedTransaction", b =>
                {
                    b.Navigation("CustomPlannedTransactions");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("ProjectK.Core.Entities.User", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("CustomPlannedTransactions");

                    b.Navigation("PlannedTransactions");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}