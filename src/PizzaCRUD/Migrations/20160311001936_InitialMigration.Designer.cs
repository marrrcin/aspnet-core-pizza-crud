using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using PizzaCRUD.Data;

namespace PizzaCRUD.Migrations
{
    [DbContext(typeof(PizzaDbContext))]
    [Migration("20160311001936_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PizzaCRUD.Models.Ingridient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 64);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();
                });

            modelBuilder.Entity("PizzaCRUD.Models.Pizza", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DoughType")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 64);

                    b.Property<decimal>("PriceLarge");

                    b.Property<decimal>("PriceMedium");

                    b.Property<decimal>("PriceSmall");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();
                });

            modelBuilder.Entity("PizzaCRUD.Models.PizzaIngridient", b =>
                {
                    b.Property<Guid>("PizzaId");

                    b.Property<Guid>("IngridientId");

                    b.HasKey("PizzaId", "IngridientId");
                });

            modelBuilder.Entity("PizzaCRUD.Models.PizzaIngridient", b =>
                {
                    b.HasOne("PizzaCRUD.Models.Ingridient")
                        .WithMany()
                        .HasForeignKey("IngridientId");

                    b.HasOne("PizzaCRUD.Models.Pizza")
                        .WithMany()
                        .HasForeignKey("PizzaId");
                });
        }
    }
}
