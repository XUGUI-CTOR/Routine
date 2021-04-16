﻿// <auto-generated />
using EFCoreCode_First;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCoreCode_First.Migrations.Blogging
{
    [DbContext(typeof(BloggingContext))]
    [Migration("20210414075652_updateallTables")]
    partial class updateallTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("EFCoreCode_First.Blog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Rating")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("varchar(150)");

                    b.HasKey("Id");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("EFCoreCode_First.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BlogId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("blog_id");

                    b.Property<string>("Content")
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Title")
                        .HasColumnType("varchar(80)");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("EFCoreCode_First.Post", b =>
                {
                    b.HasOne("EFCoreCode_First.Blog", "Blog")
                        .WithMany("Posts")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blog");
                });

            modelBuilder.Entity("EFCoreCode_First.Blog", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}