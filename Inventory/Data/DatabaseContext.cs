using Inventory.Properties;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static SQLite.SQLite3;

namespace Inventory.Data
{
    public class DatabaseContext: IAsyncDisposable
    {
        private static string _dbPath => Path.Combine(FileSystem.AppDataDirectory, Resource.DatabaseName);
        private SQLiteAsyncConnection _conn;
        private SQLiteAsyncConnection _database => _conn ??= new SQLiteAsyncConnection(_dbPath, SQLiteOpenFlags.Create| SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache);
        public DatabaseContext()
        {
        }
        private async Task CreateTableIfNotExistsc<T>() where T : class, new()
        {
          var result = await _database.CreateTableAsync<T>();
        }

        private async Task<AsyncTableQuery<T>> GetTableAsync<T>() where T : class, new()
        {
            await CreateTableIfNotExistsc<T>();
            return _database.Table<T>();
        }

        private async Task<TResult> Execute<T, TResult>(Func<Task<TResult>> action) where T : class, new()
        {
            await CreateTableIfNotExistsc<T>();
            return await action();
        }

        public string GetDbLocation()
        {
            return _dbPath;
        }

        public async Task<List<T>> GetAllAsync<T>() where T : class, new()
        {
            var table = await GetTableAsync<T>();
            return await table.ToListAsync();
        }

        public async Task<T> GetByKeyAsync<T>(object primaryKey) where T : class, new()
        {
            await CreateTableIfNotExistsc<T>();
            //return await Execute<T,T>(async () => await _database.GetAsync<T>(primaryKey));
            return await _database.GetAsync<T>(primaryKey);
        }

        public async Task<List<T>> GetFilteredAsync<T>(Expression<Func<T, bool>> predExpr) where T : class, new()
        {
            var table = await GetTableAsync<T>();
            return await table.Where(predExpr).ToListAsync();
        }

        public async Task<bool> AddItemAsync<T>(T item) where T : class, new()
        {
            await CreateTableIfNotExistsc<T>();
            var result = await _database.InsertAsync(item);
            return result > 0;
        }

        public async Task<bool> UpdateItemAsync<T>(T item) where T : class, new()
        {
            await CreateTableIfNotExistsc<T>();
            var result = await _database.UpdateAsync(item);

            //return await Execute<T,bool>(async () => await _database.UpdateAsync(item) > 0);
            return result > 0;
        }

        public async Task<bool> DeleteItemAsync<T>(T item) where T : class, new()
        {
            await CreateTableIfNotExistsc<T>();
            var result = await _database.DeleteAsync(item);
            return result > 0;
        }

        public async Task<bool> DeleteItemByKeyAsync<T>(object primaryKey) where T : class, new()
        {
            await CreateTableIfNotExistsc<T>();
            var result = await _database.DeleteAsync(primaryKey);
            return result > 0;
        }

        public async ValueTask DisposeAsync() => _conn?.CloseAsync();
    }
}
