﻿// <auto-generated />
using System;
using HxStudioFileUploadService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HxStudioFileUploadService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HxStudioFileUploadService.Models.Domain", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Domain");
                });

            modelBuilder.Entity("HxStudioFileUploadService.Models.Like", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("MockupGroupId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsLiked")
                        .HasColumnType("bit");

                    b.HasKey("UserId", "MockupGroupId");

                    b.HasIndex("MockupGroupId")
                        .IsUnique();

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("HxStudioFileUploadService.Models.Mockup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MockupGroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MockupGroupId");

                    b.ToTable("Mockups");
                });

            modelBuilder.Entity("HxStudioFileUploadService.Models.MockupGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DomainId")
                        .HasColumnType("int");

                    b.Property<int?>("ModifiedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProjectDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubDomainId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DomainId");

                    b.HasIndex("SubDomainId");

                    b.ToTable("MockupGroups");
                });

            modelBuilder.Entity("HxStudioFileUploadService.Models.Subdomain", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DomainId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DomainId");

                    b.ToTable("Subdomain");
                });

            modelBuilder.Entity("HxStudioFileUploadService.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MockupGroupId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MockupGroupId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("HxStudioFileUploadService.Models.Like", b =>
                {
                    b.HasOne("HxStudioFileUploadService.Models.MockupGroup", "MockupGroup")
                        .WithOne("Like")
                        .HasForeignKey("HxStudioFileUploadService.Models.Like", "MockupGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MockupGroup");
                });

            modelBuilder.Entity("HxStudioFileUploadService.Models.Mockup", b =>
                {
                    b.HasOne("HxStudioFileUploadService.Models.MockupGroup", "MockupGroup")
                        .WithMany("Mockups")
                        .HasForeignKey("MockupGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MockupGroup");
                });

            modelBuilder.Entity("HxStudioFileUploadService.Models.MockupGroup", b =>
                {
                    b.HasOne("HxStudioFileUploadService.Models.Domain", "Domain")
                        .WithMany()
                        .HasForeignKey("DomainId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HxStudioFileUploadService.Models.Subdomain", "SubDomain")
                        .WithMany()
                        .HasForeignKey("SubDomainId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Domain");

                    b.Navigation("SubDomain");
                });

            modelBuilder.Entity("HxStudioFileUploadService.Models.Subdomain", b =>
                {
                    b.HasOne("HxStudioFileUploadService.Models.Domain", "Domain")
                        .WithMany()
                        .HasForeignKey("DomainId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Domain");
                });

            modelBuilder.Entity("HxStudioFileUploadService.Models.Tag", b =>
                {
                    b.HasOne("HxStudioFileUploadService.Models.MockupGroup", "MockupGroup")
                        .WithMany("Tags")
                        .HasForeignKey("MockupGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MockupGroup");
                });

            modelBuilder.Entity("HxStudioFileUploadService.Models.MockupGroup", b =>
                {
                    b.Navigation("Like")
                        .IsRequired();

                    b.Navigation("Mockups");

                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
