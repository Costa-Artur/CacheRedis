using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using CacheRedis.Api.DbContexts;
using CacheRedis.Api.Entities;
using CacheRedis.Api.Results;
using CacheRedis.Api.Shared;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace CacheRedis.Api.Controllers;

[ApiController]
[Route("/api")]
public class CotationController : ControllerBase
{
    private readonly CotacaoContext _context;
    private readonly IDatabase _redis;
    public readonly IPublishEndpoint publishEndpoint;
    
    public CotationController(CotacaoContext context, IConnectionMultiplexer muxer, IPublishEndpoint publishEndpoint)
    {
        
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _redis = muxer.GetDatabase();
        this.publishEndpoint = publishEndpoint;
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

        foreach(var c in cotacao)
        {
            if(c.Sigla == "USD" && c.Valor < 3.0)
            {
                await publishEndpoint.Publish<INotificationCreated>(
                    new {
                        c.Sigla,
                        c.NomeMoeda,
                        c.Data,
                        c.Valor
                    }
                );
            }
        }

        var result = new CotacaoResult(cotacao);
        return Ok(
            result
        );

        
    }
}