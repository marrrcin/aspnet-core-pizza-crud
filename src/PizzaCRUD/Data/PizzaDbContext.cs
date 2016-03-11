using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using PizzaCRUD.Models;

namespace PizzaCRUD.Data
{
    public class PizzaDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=pizza_wai;User Id=sa;Password = qwer1234!;");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Pizza> Pizzas { set; get; }

        public DbSet<Ingridient> Ingridients { set; get; }

        public DbSet<PizzaIngridient> PizzaIngridients { set; get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ingridient>().HasIndex(i => i.Name).IsUnique();
            modelBuilder.Entity<Pizza>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<PizzaIngridient>()
                .HasOne(pi => pi.Pizza)
                .WithMany(p => p.Ingridients)
                .HasForeignKey(pi => pi.PizzaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PizzaIngridient>()
                .HasOne(pi => pi.Ingridient)
                .WithMany(i => i.Pizzas)
                .HasForeignKey(pi => pi.IngridientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PizzaIngridient>()
                .HasKey(pi => new
                {
                    pi.PizzaId,
                    pi.IngridientId
                });

            base.OnModelCreating(modelBuilder);
        }


    }
}