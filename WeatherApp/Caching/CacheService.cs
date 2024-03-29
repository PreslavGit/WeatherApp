using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using SQLite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Caching
{
    public static class CacheService
    {
        private static SQLiteConnection _connection = null;
        public static void InitDB()
        {
            var path = Path.Combine(FileSystem.AppDataDirectory, "cache.db3");
            _connection = new SQLiteConnection(path, SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex | SQLiteOpenFlags.ReadWrite);
            try
            {
                _connection.CreateTable<Cache>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                throw;
            }
        }

        public static List<Forecast> getCached(string location)
        {
            if (!isCached(location))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<List<Forecast>>(getByLocation(location).forecast);
        }

        public static void setCache(string location, List<Forecast> forecasts)
        {
            var cache = new Cache();
            cache.location = location;
            cache.forecast = JsonConvert.SerializeObject(forecasts);
            cache.cached_at = DateTime.Now;

            _connection.Insert(cache);
        }

        private static bool isCached(string location)
        {
            var row = getByLocation(location);

            var isFound = row != null;
            if (!isFound)
            {
                return false;
            }

            if (isCacheExpired(row.cached_at))
            {
                _connection.Delete(row);
                return false;
            }

            return true;
        }

        private static Cache? getByLocation(string location)
        {
            try
            {
                return _connection.Table<Cache>()
                    .Where(r => r.location == location)
                    .First();
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        private static bool isCacheExpired(DateTime cachedAt)
        {
            var minsDiff = (DateTime.Now - cachedAt).TotalMinutes;
            return minsDiff > 30;
        }
    }
}
