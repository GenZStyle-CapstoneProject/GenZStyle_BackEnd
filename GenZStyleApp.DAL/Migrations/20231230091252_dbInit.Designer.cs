﻿// <auto-generated />
using System;
using GenZStyleApp.DAL.DBContext;
using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GenZStyleApp.DAL.Migrations
{
    [DbContext(typeof(GenZStyleDbContext))]
    [Migration("20231230091252_dbInit")]
    partial class dbInit
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"), 1L, 1);

                    b.Property<string>("AvatarUrl")
                        .IsRequired()
                        .HasMaxLength(2147483647)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("InboxId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVip")
                        .HasColumnType("bit");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("AccountId");

                    b.HasIndex("UserId");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Category", b =>
                {
                    b.Property<int>("CategoriesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoriesId"), 1L, 1);

                    b.Property<string>("CategoryDescription")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CategoriesId");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime");

                    b.Property<int?>("ParentCommentId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.HasKey("CommentId");

                    b.HasIndex("AccountId");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("PostId");

                    b.ToTable("Comment", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.FashionItem", b =>
                {
                    b.Property<int>("FashionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FashionId"), 1L, 1);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<string>("fashionDescription")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("fashionName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("FashionId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("PostId");

                    b.ToTable("FashionItem", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Inbox", b =>
                {
                    b.Property<int>("InboxId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InboxId"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("LastMessage")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("InboxId");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("Inbox", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.InboxPaticipant", b =>
                {
                    b.Property<int>("InboxPaticipantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InboxPaticipantId"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("InboxId")
                        .HasColumnType("int");

                    b.HasKey("InboxPaticipantId");

                    b.HasIndex("AccountId");

                    b.HasIndex("InboxId");

                    b.ToTable("InboxPaticipant", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Invoice", b =>
                {
                    b.Property<int>("InvoiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InvoiceId"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<string>("PaymentType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("packageId")
                        .HasColumnType("int");

                    b.HasKey("InvoiceId");

                    b.HasIndex("AccountId");

                    b.HasIndex("packageId");

                    b.ToTable("Invoice", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Like", b =>
                {
                    b.Property<int>("LikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LikeId"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.HasKey("LikeId");

                    b.HasIndex("AccountId");

                    b.HasIndex("PostId");

                    b.ToTable("Like", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MessageId"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("DeleteAt")
                        .HasColumnType("datetime");

                    b.Property<string>("File")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("InboxId")
                        .HasColumnType("int");

                    b.Property<bool>("Seen")
                        .HasColumnType("bit");

                    b.HasKey("MessageId");

                    b.HasIndex("AccountId");

                    b.HasIndex("InboxId");

                    b.ToTable("Message", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationId"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime");

                    b.HasKey("NotificationId");

                    b.HasIndex("AccountId");

                    b.ToTable("Notification", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Package", b =>
                {
                    b.Property<int>("PackageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PackageId"), 1L, 1);

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PackageName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("PackageId");

                    b.ToTable("Package", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Payment", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentId"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("InvoiceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("PaymentId");

                    b.HasIndex("InvoiceId");

                    b.ToTable("Payment", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PostId"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime");

                    b.HasKey("PostId");

                    b.HasIndex("AccountId");

                    b.ToTable("Post", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Style", b =>
                {
                    b.Property<int>("StyleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StyleId"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("StyleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime");

                    b.HasKey("StyleId");

                    b.HasIndex("AccountId");

                    b.ToTable("Style", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.StyleFashion", b =>
                {
                    b.Property<int>("StyleId")
                        .HasColumnType("int");

                    b.Property<int>("FashionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime");

                    b.HasKey("StyleId", "FashionId");

                    b.HasIndex("FashionId");

                    b.ToTable("StyleFashion", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionId"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("PaymentId")
                        .HasColumnType("int");

                    b.Property<string>("TransStyle")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime");

                    b.Property<int>("WalletId")
                        .HasColumnType("int");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.HasKey("TransactionId");

                    b.HasIndex("PaymentId");

                    b.HasIndex("WalletId");

                    b.ToTable("Transaction", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("Dob")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.UserRelation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("FollowerId")
                        .HasColumnType("int");

                    b.Property<int>("FollowingId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FollowingId");

                    b.ToTable("UserRelation", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Wallet", b =>
                {
                    b.Property<int>("WalletId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WalletId"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatDate")
                        .HasColumnType("datetime");

                    b.HasKey("WalletId");

                    b.HasIndex("AccountId");

                    b.ToTable("Wallet", (string)null);
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Account", b =>
                {
                    b.HasOne("GenZStyleApp.DAL.Models.User", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Comment", b =>
                {
                    b.HasOne("GenZStyleApp.DAL.Models.Account", "Account")
                        .WithMany("Comments")
                        .HasForeignKey("AccountId")
                        .IsRequired();

                    b.HasOne("GenZStyleApp.DAL.Models.Comment", "ParentComment")
                        .WithMany("SubComments")
                        .HasForeignKey("ParentCommentId")
                        .IsRequired();

                    b.HasOne("GenZStyleApp.DAL.Models.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("ParentComment");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.FashionItem", b =>
                {
                    b.HasOne("GenZStyleApp.DAL.Models.Category", "Category")
                        .WithMany("FashionItems")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GenZStyleApp.DAL.Models.Post", "Post")
                        .WithMany("FashionItems")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Inbox", b =>
                {
                    b.HasOne("GenZStyleApp.DAL.Models.Account", "Account")
                        .WithOne("Inbox")
                        .HasForeignKey("GenZStyleApp.DAL.Models.Inbox", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.InboxPaticipant", b =>
                {
                    b.HasOne("GenZStyleApp.DAL.Models.Account", "Account")
                        .WithMany("InboxPaticipants")
                        .HasForeignKey("AccountId")
                        .IsRequired();

                    b.HasOne("GenZStyleApp.DAL.Models.Inbox", "Inbox")
                        .WithMany("Paticipants")
                        .HasForeignKey("InboxId")
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Inbox");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Invoice", b =>
                {
                    b.HasOne("GenZStyleApp.DAL.Models.Account", "Account")
                        .WithMany("Invoices")
                        .HasForeignKey("AccountId")
                        .IsRequired();

                    b.HasOne("GenZStyleApp.DAL.Models.Package", "Package")
                        .WithMany("Invoices")
                        .HasForeignKey("packageId")
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Package");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Like", b =>
                {
                    b.HasOne("GenZStyleApp.DAL.Models.Account", "Account")
                        .WithMany("Likes")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GenZStyleApp.DAL.Models.Post", "Post")
                        .WithMany("Likes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Message", b =>
                {
                    b.HasOne("GenZStyleApp.DAL.Models.Account", "Account")
                        .WithMany("Messages")
                        .HasForeignKey("AccountId")
                        .IsRequired();

                    b.HasOne("GenZStyleApp.DAL.Models.Inbox", "Inbox")
                        .WithMany("Messages")
                        .HasForeignKey("InboxId")
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Inbox");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Notification", b =>
                {
                    b.HasOne("GenZStyleApp.DAL.Models.Account", "Account")
                        .WithMany("Notifications")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Payment", b =>
                {
                    b.HasOne("GenZStyleApp.DAL.Models.Invoice", "Invoice")
                        .WithMany("Payments")
                        .HasForeignKey("InvoiceId")
                        .IsRequired();

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Post", b =>
                {
                    b.HasOne("GenZStyleApp.DAL.Models.Account", "Account")
                        .WithMany("Posts")
                        .HasForeignKey("AccountId")
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Style", b =>
                {
                    b.HasOne("GenZStyleApp.DAL.Models.Account", "Account")
                        .WithMany("Styles")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.StyleFashion", b =>
                {
                    b.HasOne("GenZStyleApp.DAL.Models.FashionItem", "FashionItem")
                        .WithMany("StyleFashions")
                        .HasForeignKey("FashionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GenZStyleApp.DAL.Models.Style", "Style")
                        .WithMany("StyleFashions")
                        .HasForeignKey("StyleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FashionItem");

                    b.Navigation("Style");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Transaction", b =>
                {
                    b.HasOne("GenZStyleApp.DAL.Models.Payment", "Payment")
                        .WithMany("Transactions")
                        .HasForeignKey("PaymentId")
                        .IsRequired();

                    b.HasOne("GenZStyleApp.DAL.Models.Wallet", "wallet")
                        .WithMany("Transactions")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Payment");

                    b.Navigation("wallet");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.User", b =>
                {
                    b.HasOne("GenZStyleApp.DAL.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.UserRelation", b =>
                {
                    b.HasOne("GenZStyleApp.DAL.Models.Account", "Account")
                        .WithMany("UserRelations")
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Wallet", b =>
                {
                    b.HasOne("GenZStyleApp.DAL.Models.Account", "Account")
                        .WithMany("Wallets")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Account", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Inbox")
                        .IsRequired();

                    b.Navigation("InboxPaticipants");

                    b.Navigation("Invoices");

                    b.Navigation("Likes");

                    b.Navigation("Messages");

                    b.Navigation("Notifications");

                    b.Navigation("Posts");

                    b.Navigation("Styles");

                    b.Navigation("UserRelations");

                    b.Navigation("Wallets");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Category", b =>
                {
                    b.Navigation("FashionItems");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Comment", b =>
                {
                    b.Navigation("SubComments");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.FashionItem", b =>
                {
                    b.Navigation("StyleFashions");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Inbox", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("Paticipants");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Invoice", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Package", b =>
                {
                    b.Navigation("Invoices");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Payment", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("FashionItems");

                    b.Navigation("Likes");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Style", b =>
                {
                    b.Navigation("StyleFashions");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.User", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("GenZStyleApp.DAL.Models.Wallet", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
