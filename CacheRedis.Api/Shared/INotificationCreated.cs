namespace CacheRedis.Api.Shared;

public interface INotificationCreated 
{
    public string Sigla {get;set;}
    public string NomeMoeda {get;set;}
    public DateOnly Data {get;set;}
    public double Valor {get;set;}    
}