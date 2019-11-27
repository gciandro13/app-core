# Data.Cache Extension

 This extension exposes the ICache interface, with get / set of cache keys, using the Microsoft.Extensions.Caching.Abstractions assembly.
 An ICacheRepository with CRUD operations is also provider.

## Extension configuration options

Options

    Type: {Memory|Distributed} 
    EntryExpirationInMinutes: override the default duration (in minutes) of the provided cache profiles. 

### Additional documentation
  In-memory caching in ASP.NET Core: https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory
  Working with a distributed cache in ASP.NET Core: https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed

## Sample of usage

```csharp
public class SomeController: ControllerBase {
    
    private ICache _cache;

    public SomeController(ICache cache) {
        _cache = cache;
    }

    [HttpGet]
    public IActionResult Get() {
        string key = "api:somedata";
        var result = _cache.Get<SomeData>(key);
        if (result == null) {
            result = SomeData.GetData();
            _cache.Set(key, result, new CacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)});
            //_cache.Set(key, result, CacheEntryOptions.Expiration.Fast); //use a default profile
        }
        return Ok(result);
    }
}
```
###  Using ICacheRepository in Api.EntityCache

```csharp
public class SomeEntityCacheController : EntityCachedController<SomeEntity> {
    public SomeEntityCacheController(IRepository<SomeEntity> repository, ICacheRepository<SomeEntity> cachedRepository) : base(repository, cachedRepository) { }
}
```

## Sample configuration

```json
      "Ws.Core.Extensions.Data.Cache": {
        "priority": 3,
        "options": {
          "type": "Distributed",
          "entryExpirationInMinutes": {
            "fast": 10,
            "medium": 60,
            "slow": 240,
            "never": 1440
          }
        }
      }
```
