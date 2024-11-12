﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OMGVA_PoS.Data_layer.Context;

#nullable disable

namespace OMGVA_PoS.Datalayer.Migrations
{
    [DbContext(typeof(OMGVADbContext))]
    [Migration("20241112164536_InitialModel")]
    partial class InitialModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Business", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StripeAccId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Businesses");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Customer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Discount", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<short>("Amount")
                        .HasColumnType("smallint");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("bit");

                    b.Property<DateTime>("TimeValidUntil")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.EmployeeSchedule", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<long>("EmployeeId")
                        .HasColumnType("bigint");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<bool>("IsCancelled")
                        .HasColumnType("bit");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EmployeeSchedules");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Giftcard", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Giftcards");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.GiftcardPayment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<decimal>("AmountUsed")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("GiftcardId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GiftcardId");

                    b.ToTable("GiftcardPayments");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Item", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("BusinessId")
                        .HasColumnType("bigint");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("DiscountId")
                        .HasColumnType("bigint");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("ImgPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InventoryQuantity")
                        .HasColumnType("int");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("bit");

                    b.Property<string>("ItemGroup")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("BusinessId");

                    b.HasIndex("DiscountId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.ItemVariation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("InventoryQuantity")
                        .HasColumnType("int");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("bit");

                    b.Property<long>("ItemId")
                        .HasColumnType("bigint");

                    b.Property<string>("ItemVariationGroup")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PriceChange")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("ItemVariations");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Order", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("DiscountId")
                        .HasColumnType("bigint");

                    b.Property<string>("PaymentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RefundReason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("Tip")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("DiscountId");

                    b.HasIndex("PaymentId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.OrderItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("DiscountId")
                        .HasColumnType("bigint");

                    b.Property<long>("ItemId")
                        .HasColumnType("bigint");

                    b.Property<long>("OrderId")
                        .HasColumnType("bigint");

                    b.Property<short>("Quantity")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("DiscountId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.OrderItemVariation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("ItemVariationId")
                        .HasColumnType("bigint");

                    b.Property<long>("OrderItemId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ItemVariationId");

                    b.HasIndex("OrderItemId");

                    b.ToTable("OrderItemVariations");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Payment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<long?>("GiftcardPaymentId")
                        .HasColumnType("bigint");

                    b.Property<int>("Method")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("GiftcardPaymentId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Reservation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<long>("EmployeeId")
                        .HasColumnType("bigint");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimeReserved")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.StripeReader", b =>
                {
                    b.Property<string>("ReaderId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("BusinessId")
                        .HasColumnType("bigint");

                    b.HasKey("ReaderId");

                    b.HasIndex("BusinessId");

                    b.ToTable("StripeReaders");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Tax", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsArchived")
                        .HasColumnType("bit");

                    b.Property<short>("Percent")
                        .HasColumnType("smallint");

                    b.Property<string>("TaxType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Taxes");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.TaxItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("ItemId")
                        .HasColumnType("bigint");

                    b.Property<long>("TaxId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("TaxId");

                    b.ToTable("TaxItems");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("BusinessId")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasLeft")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BusinessId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.EmployeeSchedule", b =>
                {
                    b.HasOne("OMGVA_PoS.Data_layer.Models.User", "User")
                        .WithMany("EmployeeSchedules")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.GiftcardPayment", b =>
                {
                    b.HasOne("OMGVA_PoS.Data_layer.Models.Giftcard", "Giftcard")
                        .WithMany("GiftcardPayments")
                        .HasForeignKey("GiftcardId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Giftcard");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Item", b =>
                {
                    b.HasOne("OMGVA_PoS.Data_layer.Models.Business", "Business")
                        .WithMany("Items")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OMGVA_PoS.Data_layer.Models.Discount", "Discount")
                        .WithMany("Items")
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Business");

                    b.Navigation("Discount");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.ItemVariation", b =>
                {
                    b.HasOne("OMGVA_PoS.Data_layer.Models.Item", "Item")
                        .WithMany("ItemVariations")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Order", b =>
                {
                    b.HasOne("OMGVA_PoS.Data_layer.Models.Discount", "Discount")
                        .WithMany("Orders")
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("OMGVA_PoS.Data_layer.Models.Payment", "Payment")
                        .WithOne("Order")
                        .HasForeignKey("OMGVA_PoS.Data_layer.Models.Order", "PaymentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OMGVA_PoS.Data_layer.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Discount");

                    b.Navigation("Payment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.OrderItem", b =>
                {
                    b.HasOne("OMGVA_PoS.Data_layer.Models.Discount", "Discount")
                        .WithMany("OrderItems")
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("OMGVA_PoS.Data_layer.Models.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Discount");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.OrderItemVariation", b =>
                {
                    b.HasOne("OMGVA_PoS.Data_layer.Models.ItemVariation", "ItemVariation")
                        .WithMany("OrderItemVariations")
                        .HasForeignKey("ItemVariationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OMGVA_PoS.Data_layer.Models.OrderItem", "OrderItem")
                        .WithMany("OrderItemVariations")
                        .HasForeignKey("OrderItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ItemVariation");

                    b.Navigation("OrderItem");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Payment", b =>
                {
                    b.HasOne("OMGVA_PoS.Data_layer.Models.Customer", "Customer")
                        .WithMany("Payments")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OMGVA_PoS.Data_layer.Models.GiftcardPayment", "GiftcardPayment")
                        .WithMany()
                        .HasForeignKey("GiftcardPaymentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Customer");

                    b.Navigation("GiftcardPayment");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Reservation", b =>
                {
                    b.HasOne("OMGVA_PoS.Data_layer.Models.Customer", "Customer")
                        .WithMany("Reservations")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OMGVA_PoS.Data_layer.Models.User", "User")
                        .WithMany("Reservations")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.StripeReader", b =>
                {
                    b.HasOne("OMGVA_PoS.Data_layer.Models.Business", "Business")
                        .WithMany("StripeReaders")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.TaxItem", b =>
                {
                    b.HasOne("OMGVA_PoS.Data_layer.Models.Item", "Item")
                        .WithMany("TaxItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OMGVA_PoS.Data_layer.Models.Tax", "Tax")
                        .WithMany("TaxItems")
                        .HasForeignKey("TaxId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Tax");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.User", b =>
                {
                    b.HasOne("OMGVA_PoS.Data_layer.Models.Business", "Business")
                        .WithMany("Users")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Business", b =>
                {
                    b.Navigation("Items");

                    b.Navigation("StripeReaders");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Customer", b =>
                {
                    b.Navigation("Payments");

                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Discount", b =>
                {
                    b.Navigation("Items");

                    b.Navigation("OrderItems");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Giftcard", b =>
                {
                    b.Navigation("GiftcardPayments");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Item", b =>
                {
                    b.Navigation("ItemVariations");

                    b.Navigation("TaxItems");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.ItemVariation", b =>
                {
                    b.Navigation("OrderItemVariations");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.OrderItem", b =>
                {
                    b.Navigation("OrderItemVariations");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Payment", b =>
                {
                    b.Navigation("Order")
                        .IsRequired();
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.Tax", b =>
                {
                    b.Navigation("TaxItems");
                });

            modelBuilder.Entity("OMGVA_PoS.Data_layer.Models.User", b =>
                {
                    b.Navigation("EmployeeSchedules");

                    b.Navigation("Orders");

                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
