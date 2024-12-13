﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OmgvaPOS.Database.Context;

#nullable disable

namespace OmgvaPOS.Migrations
{
    [DbContext(typeof(OmgvaDbContext))]
    partial class OmgvaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OmgvaPOS.BusinessManagement.Models.Business", b =>
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

            modelBuilder.Entity("OmgvaPOS.CustomerManagement.Models.Customer", b =>
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

            modelBuilder.Entity("OmgvaPOS.DiscountManagement.Models.Discount", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<short>("Amount")
                        .HasColumnType("smallint");

                    b.Property<long>("BusinessId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("bit");

                    b.Property<DateTime>("TimeValidUntil")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("OmgvaPOS.GiftcardManagement.Models.Giftcard", b =>
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

            modelBuilder.Entity("OmgvaPOS.GiftcardPaymentManagement.Models.GiftcardPaymentEntity", b =>
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

            modelBuilder.Entity("OmgvaPOS.ItemManagement.Models.Item", b =>
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

                    b.Property<TimeSpan?>("Duration")
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

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BusinessId");

                    b.HasIndex("DiscountId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("OmgvaPOS.ItemVariationManagement.Models.ItemVariation", b =>
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

            modelBuilder.Entity("OmgvaPOS.OrderItemManagement.Models.OrderItem", b =>
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

            modelBuilder.Entity("OmgvaPOS.OrderItemVariationManagement.Models.OrderItemVariation", b =>
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

            modelBuilder.Entity("OmgvaPOS.OrderManagement.Models.Order", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("DiscountId")
                        .HasColumnType("bigint");

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

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("OmgvaPOS.PaymentManagement.Models.Payment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<long?>("GiftCardPaymentId")
                        .HasColumnType("bigint");

                    b.Property<long>("GiftcardPaymentEntityId")
                        .HasColumnType("bigint");

                    b.Property<int>("Method")
                        .HasColumnType("int");

                    b.Property<long>("OrderId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("GiftcardPaymentEntityId");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("OmgvaPOS.PaymentManagement.Models.StripeReader", b =>
                {
                    b.Property<string>("ReaderId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("BusinessId")
                        .HasColumnType("bigint");

                    b.HasKey("ReaderId");

                    b.HasIndex("BusinessId");

                    b.ToTable("StripeReaders");
                });

            modelBuilder.Entity("OmgvaPOS.ReservationManagement.Models.Reservation", b =>
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

            modelBuilder.Entity("OmgvaPOS.ScheduleManagement.Models.EmployeeSchedule", b =>
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

            modelBuilder.Entity("OmgvaPOS.TaxManagement.Models.Tax", b =>
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

            modelBuilder.Entity("OmgvaPOS.TaxManagement.Models.TaxItem", b =>
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

            modelBuilder.Entity("OmgvaPOS.UserManagement.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("BusinessId")
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

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BusinessId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OmgvaPOS.GiftcardPaymentManagement.Models.GiftcardPaymentEntity", b =>
                {
                    b.HasOne("OmgvaPOS.GiftcardManagement.Models.Giftcard", "Giftcard")
                        .WithMany("GiftcardPayments")
                        .HasForeignKey("GiftcardId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Giftcard");
                });

            modelBuilder.Entity("OmgvaPOS.ItemManagement.Models.Item", b =>
                {
                    b.HasOne("OmgvaPOS.BusinessManagement.Models.Business", "Business")
                        .WithMany("Items")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OmgvaPOS.DiscountManagement.Models.Discount", "Discount")
                        .WithMany("Items")
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Business");

                    b.Navigation("Discount");
                });

            modelBuilder.Entity("OmgvaPOS.ItemVariationManagement.Models.ItemVariation", b =>
                {
                    b.HasOne("OmgvaPOS.ItemManagement.Models.Item", "Item")
                        .WithMany("ItemVariations")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("OmgvaPOS.OrderItemManagement.Models.OrderItem", b =>
                {
                    b.HasOne("OmgvaPOS.DiscountManagement.Models.Discount", "Discount")
                        .WithMany("OrderItems")
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("OmgvaPOS.OrderManagement.Models.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Discount");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("OmgvaPOS.OrderItemVariationManagement.Models.OrderItemVariation", b =>
                {
                    b.HasOne("OmgvaPOS.ItemVariationManagement.Models.ItemVariation", "ItemVariation")
                        .WithMany("OrderItemVariations")
                        .HasForeignKey("ItemVariationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OmgvaPOS.OrderItemManagement.Models.OrderItem", "OrderItem")
                        .WithMany("OrderItemVariations")
                        .HasForeignKey("OrderItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ItemVariation");

                    b.Navigation("OrderItem");
                });

            modelBuilder.Entity("OmgvaPOS.OrderManagement.Models.Order", b =>
                {
                    b.HasOne("OmgvaPOS.DiscountManagement.Models.Discount", "Discount")
                        .WithMany("Orders")
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("OmgvaPOS.UserManagement.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Discount");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OmgvaPOS.PaymentManagement.Models.Payment", b =>
                {
                    b.HasOne("OmgvaPOS.CustomerManagement.Models.Customer", "Customer")
                        .WithMany("Payments")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OmgvaPOS.GiftcardPaymentManagement.Models.GiftcardPaymentEntity", "GiftcardPaymentEntity")
                        .WithMany()
                        .HasForeignKey("GiftcardPaymentEntityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OmgvaPOS.OrderManagement.Models.Order", "Order")
                        .WithOne("Payment")
                        .HasForeignKey("OmgvaPOS.PaymentManagement.Models.Payment", "OrderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("GiftcardPaymentEntity");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("OmgvaPOS.PaymentManagement.Models.StripeReader", b =>
                {
                    b.HasOne("OmgvaPOS.BusinessManagement.Models.Business", "Business")
                        .WithMany("StripeReaders")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("OmgvaPOS.ReservationManagement.Models.Reservation", b =>
                {
                    b.HasOne("OmgvaPOS.CustomerManagement.Models.Customer", "Customer")
                        .WithMany("Reservations")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OmgvaPOS.UserManagement.Models.User", "User")
                        .WithMany("Reservations")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OmgvaPOS.ScheduleManagement.Models.EmployeeSchedule", b =>
                {
                    b.HasOne("OmgvaPOS.UserManagement.Models.User", "User")
                        .WithMany("EmployeeSchedules")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("OmgvaPOS.TaxManagement.Models.TaxItem", b =>
                {
                    b.HasOne("OmgvaPOS.ItemManagement.Models.Item", "Item")
                        .WithMany("TaxItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OmgvaPOS.TaxManagement.Models.Tax", "Tax")
                        .WithMany("TaxItems")
                        .HasForeignKey("TaxId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Tax");
                });

            modelBuilder.Entity("OmgvaPOS.UserManagement.Models.User", b =>
                {
                    b.HasOne("OmgvaPOS.BusinessManagement.Models.Business", "Business")
                        .WithMany("Users")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Business");
                });

            modelBuilder.Entity("OmgvaPOS.BusinessManagement.Models.Business", b =>
                {
                    b.Navigation("Items");

                    b.Navigation("StripeReaders");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("OmgvaPOS.CustomerManagement.Models.Customer", b =>
                {
                    b.Navigation("Payments");

                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("OmgvaPOS.DiscountManagement.Models.Discount", b =>
                {
                    b.Navigation("Items");

                    b.Navigation("OrderItems");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("OmgvaPOS.GiftcardManagement.Models.Giftcard", b =>
                {
                    b.Navigation("GiftcardPayments");
                });

            modelBuilder.Entity("OmgvaPOS.ItemManagement.Models.Item", b =>
                {
                    b.Navigation("ItemVariations");

                    b.Navigation("TaxItems");
                });

            modelBuilder.Entity("OmgvaPOS.ItemVariationManagement.Models.ItemVariation", b =>
                {
                    b.Navigation("OrderItemVariations");
                });

            modelBuilder.Entity("OmgvaPOS.OrderItemManagement.Models.OrderItem", b =>
                {
                    b.Navigation("OrderItemVariations");
                });

            modelBuilder.Entity("OmgvaPOS.OrderManagement.Models.Order", b =>
                {
                    b.Navigation("OrderItems");

                    b.Navigation("Payment")
                        .IsRequired();
                });

            modelBuilder.Entity("OmgvaPOS.TaxManagement.Models.Tax", b =>
                {
                    b.Navigation("TaxItems");
                });

            modelBuilder.Entity("OmgvaPOS.UserManagement.Models.User", b =>
                {
                    b.Navigation("EmployeeSchedules");

                    b.Navigation("Orders");

                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
