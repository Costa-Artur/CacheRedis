using CacheRedis.Api.Entities;
using Univali.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CacheRedis.Api.DbContexts;

public class CotacaoContext : DbContext
{
    public DbSet<Cotacao> CotacoesAtual {get;set;} = null!;

    public CotacaoContext(DbContextOptions<CotacaoContext>options) : base(options){ }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var cotacao = modelBuilder.Entity<Cotacao>().ToTable("cotacao", t => t.ExcludeFromMigrations());

        cotacao.Property (c => c.Sigla)
            .HasColumnName("sigla");

        cotacao.Property (c => c.CotacaoId)
            .HasColumnName("id");

        cotacao.Property (c => c.NomeMoeda)
            .HasColumnName("nome_moeda");

        cotacao.Property (c => c.Data)
            .HasColumnName("data");

        cotacao.Property (c => c.Valor)
            .HasColumnName("valor");

        base.OnModelCreating(modelBuilder);
    }
}