using CacheRedis.Api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CacheRedis.Api.Controllers;

[ApiController]
[Route("/api")]
public class CotationController : ControllerBase
{
    [HttpGet]
    public IEnumerable<Cotacao> GetCotacao ()
    {
        return new List<Cotacao> {};
    }
}