using ConstruMarket.Teste.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConstruMarket.Teste.DataAccess.Maps
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(c => c.ProdutoId);
            builder.Property(c => c.Nome).HasColumnName("Name");
            builder.Property(c => c.Preco).HasColumnName("Price");
            builder.Property(c => c.Marca).HasColumnName("Brand");
            builder.Property(c => c.CodigoMarca).HasColumnName("CodBrand");
           
        }
    }
}