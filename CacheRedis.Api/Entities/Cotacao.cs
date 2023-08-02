using System.Text.Json.Serialization;

namespace CacheRedis.Api.Entities;

public class Cotacao 
{
    [JsonPropertyName("id")]
    public int CotacaoId {get;set;}
    [JsonPropertyName("sigla")]
    public string Sigla {get;set;} = string.Empty;
    [JsonPropertyName("nome-moeda")]
    public string NomeMoeda {get;set;} = string.Empty;
    [JsonPropertyName("data")]
    public DateOnly Data {get;set;}
    [JsonPropertyName("valor")]
    public double Valor {get;set;}
}