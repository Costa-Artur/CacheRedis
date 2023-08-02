using CacheRedis.Api.DbContexts;
using CacheRedis.Api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CacheRedis.Api.Controllers;

[ApiController]
[Route("/api")]
public class CotationController : ControllerBase
{
    private readonly CotacaoContext _context;

    public CotationController(CotacaoContext context)
    {
         _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet]
    public ActionResult<IEnumerable<Cotacao>> GetCotacao (DateTime data)
    {
        var dataPesquisa = DateOnly.FromDateTime(data);
        Console.WriteLine(_context.CotacoesAtual.First().CotacaoId);
        return Ok(
            _context.CotacoesAtual.Where(c => c.Data == dataPesquisa).ToList()
        );
    }
}