using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using CacheRedis.Api.DbContexts;
using CacheRedis.Api.Entities;
using CacheRedis.Api.Results;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace CacheRedis.Api.Controllers;

[ApiController]
[Route("/api")]
public class CotationController : ControllerBase
{
    private readonly CotacaoContext _context;
    private readonly IDatabase _redis;
    
    public CotationController(CotacaoContext context, IConnectionMultiplexer muxer)
    {
        
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _redis = muxer.GetDatabase();
    }

    [HttpGet]
    public async Task<ActionResult<CotacaoResult>> GetCotacao (DateTime data)
    {
        var dataPesquisa = DateOnly.FromDateTime(data);
        string json;
        var keyName = $"cotacao_{dataPesquisa}";
        json = await _redis.StringGetAsync(keyName);

        if(string.IsNullOrEmpty(json))
        {
            json =  JsonSerializer.Serialize( _context.CotacoesAtual.Where(c => c.Data == dataPesquisa).ToList());
            var setTask = _redis.StringSetAsync(keyName, json);
            var expireTask = _redis.KeyExpireAsync(keyName, TimeSpan.FromSeconds(3600));
            await Task.WhenAll(setTask, expireTask);
        }

        var cotacao = JsonSerializer.Deserialize<IEnumerable<Cotacao>>(json);

        var result = new CotacaoResult(cotacao);
        return Ok(
            result
        );

        
    }
}