using CacheRedis.Api.Entities;

namespace CacheRedis.Api.Results;

public class CotacaoResult 
{
    public IEnumerable<Cotacao> Cotacoes {get;}

    public CotacaoResult(IEnumerable<Cotacao> cotacoes)
    {
        Cotacoes = cotacoes;
    }
}