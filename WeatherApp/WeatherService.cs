using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    internal static class WeatherService
    {
        private static string API_KEY = "";
        private static HttpClient http = new()
        {
            BaseAddress = new Uri("http://api.openweathermap.org"),
        };

        private static async Task<ForecastDTO> GetForecast(double lat, double lon)
        {
            var route = $"""data/2.5/forecast?lat={lat}&lon={lon}&appid={WeatherService.API_KEY}""";
            var json = await (await http.GetAsync(route))
                .EnsureSuccessStatusCode()
                .Content.ReadAsStringAsync();

            Toast.Make($"""Forecast Request Sent""").Show();

            return JsonConvert.DeserializeObject<ForecastDTO>(json);
        }
        private static async Task<CurrWeatherDTO> GetCurrWeather(double lat, double lon)
        {
            var route = $"""data/2.5/weather?lat={lat}&lon={lon}&appid={WeatherService.API_KEY}""";
            var json = await (await http.GetAsync(route))
                .EnsureSuccessStatusCode()
                .Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<CurrWeatherDTO>(json);
        }
        private static async Task<Coord> GetCityCoords(string city)
        {
            var route = $"""geo/1.0/direct?q={city}&appid={WeatherService.API_KEY}""";
            var json = await (await http.GetAsync(route))
                .EnsureSuccessStatusCode()
                .Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Coord[]>(json)[0];
        }
        private static Location GetCurrentLocation()
        {
            return new Location(42.698334, 23.319941);
        }

        public static async Task<ForecastDTO> GetForecastByCity(string city)
        {
            var cord = await GetCityCoords(city);

            return await GetForecast(cord.Lat, cord.Lon);
        }
        public static async Task<ForecastDTO> GetForecastCurrLocation()
        {
            var cord = GetCurrentLocation();

            return await GetForecast(cord.Latitude, cord.Longitude);
        }
        public static async Task<CurrWeatherDTO> GetCurrWeatherByCity(string city)
        {
            var cord = await GetCityCoords(city);

            return await GetCurrWeather(cord.Lat, cord.Lon);
        }
        public static async Task<CurrWeatherDTO> GetCurrWeatherCurrLocation()
        {
            var cord = GetCurrentLocation();

            return await GetCurrWeather(cord.Latitude, cord.Longitude);
        }
    }
}
