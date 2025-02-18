using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProducaoMicroservice.Core.Entities;

namespace ProducaoMicroservice.Adapters.Database.PostgreSQL
{
    public class ProducaoContext : DbContext
    {
        public DbSet<Producao> Producoes { get; set; }

        public ProducaoContext(DbContextOptions<ProducaoContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Criar um conversor de valores para DateTime -> UTC
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),  // Salvar como UTC
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // Ler como UTC
            );

            // Aplicar o conversor a todas as propriedades DateTime
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTime));

                foreach (var property in properties)
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(property.Name)
                        .HasConversion(dateTimeConverter);
                }
            }
        }
    }
}
