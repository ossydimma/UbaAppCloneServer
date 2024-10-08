using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text.Json.Serialization;
using UbaClone.WebApi.Data;

namespace UbaClone.WebApi.Repositories;

public class UsersRepository : IUsersRepository
{
    private DataContext _db;
    private readonly IDistributedCache _distributedCache;
    private readonly DistributedCacheEntryOptions _cacheEntryOptions;
        

    public UsersRepository(IDistributedCache distributedCache, DataContext db)
    {
        _db = db;
        _distributedCache = distributedCache;
        _cacheEntryOptions = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)) // Expire after 10 Mins
            .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Reset Expiration on accces
        
    }

    public async Task<Models.UbaClone[]> RetrieveAllAsync()
    {
        return await _db.ubaClones.ToArrayAsync();

    }

    public async Task<Models.UbaClone?> RetrieveAsync(int id)
    {
        string key = $"user:{id}";
        string? fromCache = await _distributedCache.GetStringAsync(key);

        if (!string.IsNullOrEmpty(fromCache))
            return JsonConvert.DeserializeObject<Models.UbaClone>(fromCache);

       Models.UbaClone? fromDb = await _db.ubaClones.FirstOrDefaultAsync(u => u.Id == id);
       
        if(fromDb == null) return fromDb;

        await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(fromDb), _cacheEntryOptions);

        return fromDb;
    }

    public async Task<Models.UbaClone?> CreateUserAsync(Models.UbaClone user)
    {
        string key = $"user:{user.Id}";

        await _db.ubaClones.AddAsync(user);
        int affect = await _db.SaveChangesAsync();

        if (affect == 1)
        {
           await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(user), _cacheEntryOptions);
           return user;
        }

        return null;
    }

    public async Task<Models.UbaClone?> UpdateUserAsync(Models.UbaClone user)
    {
        string key = $"user:{user.Id}";

        _db.Update(user);
        int affect = await _db.SaveChangesAsync();

        if (affect == 1)
        {
            await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(user), _cacheEntryOptions);
            return user;
        }
        return null;
    }

    public async Task<bool?> DeleteUserAsync(int id)
    {
        string key = $"user:{id}";

        Models.UbaClone? user = await _db.ubaClones.FindAsync(id);
        if (user is null) return null;

        _db.ubaClones.Remove(user);
        int affect = await _db.SaveChangesAsync();

        if (affect == 1)
        {
            _distributedCache.Remove(key);
            return true;
        }
        return null;
    }
    
}