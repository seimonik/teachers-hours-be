﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TH.Dal;
using TH.Dal.Enums;

#nullable disable

namespace TH.Dal.Migrations
{
    [DbContext(typeof(TeachersHoursDbContext))]
    partial class TeachersHoursDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "document_types", new[] { "ordinary", "request", "calculation" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "education_level_types", new[] { "bachelor", "specialty", "magistracy", "postgraduate" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TH.Dal.Entities.Document", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<DocumentTypes>("DocumentType")
                        .HasColumnType("document_types");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ParentDocumentId")
                        .HasColumnType("uuid");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ParentDocumentId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("TH.Dal.Entities.Specialization", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<EducationLevelTypes>("EducationLevel")
                        .HasColumnType("education_level_types");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("Specializations", "lookups");

                    b.HasData(
                        new
                        {
                            Code = "02.03.02",
                            EducationLevel = EducationLevelTypes.Bachelor,
                            Name = "ФИИТ"
                        },
                        new
                        {
                            Code = "02.03.03",
                            EducationLevel = EducationLevelTypes.Bachelor,
                            Name = "МОАИС"
                        },
                        new
                        {
                            Code = "09.03.01",
                            EducationLevel = EducationLevelTypes.Bachelor,
                            Name = "ИВТ"
                        },
                        new
                        {
                            Code = "09.03.03",
                            EducationLevel = EducationLevelTypes.Bachelor,
                            Name = "ПИ"
                        },
                        new
                        {
                            Code = "27.03.03",
                            EducationLevel = EducationLevelTypes.Bachelor,
                            Name = "САиУ"
                        },
                        new
                        {
                            Code = "44.03.01",
                            EducationLevel = EducationLevelTypes.Bachelor,
                            Name = "ПО"
                        },
                        new
                        {
                            Code = "10.05.01",
                            EducationLevel = EducationLevelTypes.Specialty,
                            Name = "КБ"
                        },
                        new
                        {
                            Code = "02.04.03",
                            EducationLevel = EducationLevelTypes.Magistracy,
                            Name = "МОАИС"
                        },
                        new
                        {
                            Code = "09.04.01",
                            EducationLevel = EducationLevelTypes.Magistracy,
                            Name = "ИВТ"
                        },
                        new
                        {
                            Code = "44.04.01",
                            EducationLevel = EducationLevelTypes.Magistracy,
                            Name = "ПО"
                        },
                        new
                        {
                            Code = "1.1.5",
                            EducationLevel = EducationLevelTypes.Postgraduate,
                            Name = "МЛ"
                        },
                        new
                        {
                            Code = "1.2.2",
                            EducationLevel = EducationLevelTypes.Postgraduate,
                            Name = "ММ"
                        },
                        new
                        {
                            Code = "1.2.3",
                            EducationLevel = EducationLevelTypes.Postgraduate,
                            Name = "ТИ"
                        },
                        new
                        {
                            Code = "2.3.1",
                            EducationLevel = EducationLevelTypes.Postgraduate,
                            Name = "СА"
                        },
                        new
                        {
                            Code = "02.06.01",
                            EducationLevel = EducationLevelTypes.Postgraduate,
                            Name = "КИН"
                        },
                        new
                        {
                            Code = "09.06.01",
                            EducationLevel = EducationLevelTypes.Postgraduate,
                            Name = "ИВТ"
                        },
                        new
                        {
                            Code = "01.06.01",
                            EducationLevel = EducationLevelTypes.Postgraduate,
                            Name = "ММ"
                        });
                });

            modelBuilder.Entity("TH.Dal.Entities.Teacher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Rate")
                        .HasColumnType("integer");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("TH.Dal.Entities.TimeNorm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Norm")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("Code");

                    b.ToTable("TimeNorms", "lookups");

                    b.HasData(
                        new
                        {
                            Id = new Guid("85744b33-9494-4436-ad8d-743b92135b17"),
                            Code = "ProductionPractice",
                            Name = "Производственная практика",
                            Norm = 4.0
                        },
                        new
                        {
                            Id = new Guid("41371a87-ec4c-4038-bd03-d07c80ff88a7"),
                            Code = "EducationalPractice23",
                            Name = "Учебная практика (НИР) 2 курс 3 семестр",
                            Norm = 18.0
                        },
                        new
                        {
                            Id = new Guid("3ec94a49-266d-4cf2-bfee-fe35516cbd79"),
                            Code = "EducationalPractice35",
                            Name = "Учебная практика (НИР) 3 курс 5 семестр",
                            Norm = 18.0
                        },
                        new
                        {
                            Id = new Guid("25d1411a-e920-4d44-a036-849698387abf"),
                            Code = "EducationalPractice24",
                            Name = "Учебная практика (НИР) 2 курс 4 семестр",
                            Norm = 16.0
                        },
                        new
                        {
                            Id = new Guid("386cfcb3-643d-4b0f-9135-b5ea4f5182cd"),
                            Code = "EducationalPractice36",
                            Name = "Учебная практика (НИР) 3 курс 6 семестр",
                            Norm = 16.0
                        },
                        new
                        {
                            Id = new Guid("760ecbf0-03be-484b-b076-c37059dc478b"),
                            Code = "EducationalPractice47",
                            Name = "Учебная практика (НИР) 4 курс 7 семестр",
                            Norm = 14.0
                        },
                        new
                        {
                            Id = new Guid("837a8528-d235-4649-aa32-1e3b01f33928"),
                            Code = "Coursework2",
                            Name = "Норма часов по курсовым работам (2 курс)",
                            Norm = 5.0
                        },
                        new
                        {
                            Id = new Guid("051f35df-bc4e-452f-9d1a-4a708372fc2f"),
                            Code = "Coursework3",
                            Name = "Норма часов по курсовым работам (3 курс)",
                            Norm = 10.0
                        },
                        new
                        {
                            Id = new Guid("d456e79d-c74d-4783-b9a1-8aecdfb31750"),
                            Code = "Coursework2PO",
                            Name = "Норма часов по курсовым работам для ПО (2 курс)",
                            Norm = 3.0
                        },
                        new
                        {
                            Id = new Guid("a90b91fa-b823-4643-8186-9e2cb90d7156"),
                            Code = "Coursework3PO",
                            Name = "Норма часов по курсовым работам для ПО (3 курс)",
                            Norm = 10.0
                        },
                        new
                        {
                            Id = new Guid("89bae101-3f08-4e1e-b93f-d2bd578b9361"),
                            Code = "FinalQualifyingWorkBachelor",
                            Name = "ВКР (бакалавриат)",
                            Norm = 24.25
                        },
                        new
                        {
                            Id = new Guid("39d825fb-5e2e-4bc8-b0f6-44d95a122fab"),
                            Code = "FinalQualifyingWorkMagistracy",
                            Name = "ВКР (магистратура)",
                            Norm = 34.25
                        });
                });

            modelBuilder.Entity("TH.Dal.Entities.Document", b =>
                {
                    b.HasOne("TH.Dal.Entities.Document", "ParentDocument")
                        .WithMany("ChildDocuments")
                        .HasForeignKey("ParentDocumentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("ParentDocument");
                });

            modelBuilder.Entity("TH.Dal.Entities.Document", b =>
                {
                    b.Navigation("ChildDocuments");
                });
#pragma warning restore 612, 618
        }
    }
}
