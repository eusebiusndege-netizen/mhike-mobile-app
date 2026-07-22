using MHike.Models;
using SQLite;

namespace MHike.Services;

public class HikeRepository
{
    private SQLiteAsyncConnection? _database;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public HikeRepository()
    {
    }

    private async Task Initialize()
    {
        await _semaphore.WaitAsync();
        try
        {
            if (_database != null) return;

            _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await _database.CreateTableAsync<Hike>();
            await _database.CreateTableAsync<Observation>();
            await _database.CreateTableAsync<User>();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    // Hike operations
    public async Task<List<Hike>> GetHikesAsync()
    {
        await Initialize();
        return await _database!.Table<Hike>().ToListAsync();
    }

    public async Task<Hike?> GetHikeAsync(int id)
    {
        await Initialize();
        return await _database!.Table<Hike>().Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveHikeAsync(Hike hike)
    {
        await Initialize();
        if (hike.Id != 0)
            return await _database!.UpdateAsync(hike);
        else
            return await _database!.InsertAsync(hike);
    }

    public async Task<int> DeleteHikeAsync(Hike hike)
    {
        await Initialize();
        await _database!.Table<Observation>().DeleteAsync(x => x.HikeId == hike.Id);
        return await _database!.DeleteAsync(hike);
    }

    // Observation operations
    public async Task<List<Observation>> GetObservationsAsync(int hikeId)
    {
        await Initialize();
        return await _database!.Table<Observation>()
            .Where(x => x.HikeId == hikeId)
            .ToListAsync();
    }

    public async Task<int> SaveObservationAsync(Observation observation)
    {
        await Initialize();
        if (observation.Id != 0)
            return await _database!.UpdateAsync(observation);
        else
            return await _database!.InsertAsync(observation);
    }

    public async Task<int> DeleteObservationAsync(Observation observation)
    {
        await Initialize();
        return await _database!.DeleteAsync(observation);
    }

    // User operations
    public async Task<User?> GetUserAsync(string username, string password)
    {
        await Initialize();
        return await _database!.Table<User>()
            .Where(x => x.Username == username && x.Password == password)
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateUserAsync(User user)
    {
        await Initialize();
        return await _database!.InsertAsync(user);
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        await Initialize();
        var user = await _database!.Table<User>()
            .Where(x => x.Username == username)
            .FirstOrDefaultAsync();
        return user != null;
    }
}