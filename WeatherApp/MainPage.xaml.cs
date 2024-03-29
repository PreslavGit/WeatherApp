using CommunityToolkit.Maui.Alerts;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using WeatherApp.Caching;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        private string GEOLOC = "Current location";
        private string SEARCH_CITY = "Search by city";

        public string SearchBtnText { get; set; }
        public string CityEntry { get; set; }
        public string Temperature { get; set; } = "---";
        public string Humidity { get; set; } = "---";
        public string SunriseTime { get; set; } = "---";
        public string SunsetTime { get; set; } = "---";
        public string WeatherIcon { get; set; } = "http://openweathermap.org/img/wn/01n.png";

        public string TodayPlusTwo { get; set; } = DateTime.Now.AddDays(2).DayOfWeek.ToString();
        public string TodayPlusThree { get; set; } = DateTime.Now.AddDays(3).DayOfWeek.ToString();
        public string TodayPlusFour { get; set; } = DateTime.Now.AddDays(4).DayOfWeek.ToString();

        public List<Forecast> Forecasts { get; set; } = new List<Forecast>();
        public List<Forecast> FilteredForecasts { get; set; } = new List<Forecast>();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            CacheService.InitDB();

            SearchBtnText = GEOLOC;

            UpdateUI();
        }

        private async void SearchButton_Clicked(object sender, EventArgs e)
        {
            CurrWeatherDTO weather;
            if (SearchBtnText == GEOLOC)
            {
                weather = await WeatherService.GetCurrWeatherCurrLocation();
                Forecasts = ForecastsFromDTO(await WeatherService.GetForecastCurrLocation(), weather.Timezone);
            }
            else
            {
                weather = await WeatherService.GetCurrWeatherByCity(CityEntry);

                var cachedForecast = CacheService.getCached(CityEntry);
                if (cachedForecast == null)
                {
                    Forecasts = ForecastsFromDTO(await WeatherService.GetForecastByCity(CityEntry), weather.Timezone);
                    CacheService.setCache(CityEntry, Forecasts);
                }
                else
                {
                    Forecasts = cachedForecast;
                    Toast.Make($"""Cache hit for {CityEntry}""").Show();
                }
            }
            FilteredForecasts = FilterForecasts(Forecasts, 0);

            Temperature = weather.Main.Temp.ToString() + " C";
            Humidity = weather.Main.Humidity.ToString() + " %";
            SunriseTime = UnixToDateTime(weather.Sys.Sunrise, weather.Timezone).ToString("HH:mm") + " ч.";
            SunsetTime = UnixToDateTime(weather.Sys.Sunset, weather.Timezone).ToString("HH:mm") + " ч.";
            WeatherIcon = $"""http://openweathermap.org/img/wn/{weather.Weather[0].Icon}.png""";

            UpdateUI();
        }
        public void ForecastDaySelect(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                switch (btn.CommandParameter.ToString())
                {
                    case "Today":
                        FilteredForecasts = FilterForecasts(Forecasts, 0);
                        break;
                    case "Tomorrow":
                        FilteredForecasts = FilterForecasts(Forecasts, 1);
                        break;
                    case nameof(TodayPlusTwo):
                        FilteredForecasts = FilterForecasts(Forecasts, 2);
                        break;
                    case nameof(TodayPlusThree):
                        FilteredForecasts = FilterForecasts(Forecasts, 3);
                        break;
                    case nameof(TodayPlusFour):
                        FilteredForecasts = FilterForecasts(Forecasts, 4);
                        break;
                }
            }
            OnPropertyChanged(nameof(FilteredForecasts));
        }

        private DateTime UnixToDateTime(long unixTimeStamp, long timezoneOffset)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp + timezoneOffset);
            return dateTime;
        }
        public void CityEntry_Changed(object sender, EventArgs e)
        {
            if (sender is Entry entry)
            {
                bool isEmpty = entry.Text.Length == 0;
                if (isEmpty)
                {
                    SearchBtnText = GEOLOC;
                }
                else
                {
                    SearchBtnText = SEARCH_CITY;
                }
                OnPropertyChanged(nameof(SearchBtnText));
            }
        }

        private List<Forecast> ForecastsFromDTO(ForecastDTO dto, long timezoneOffset)
        {
            var res = new List<Forecast>();

            foreach (var l in dto.List)
            {
                var time = UnixToDateTime(l.Dt, timezoneOffset);
                var temp = l.Main.Temp;
                var f = new Forecast(time, temp);
                res.Add(f);
            }

            return res;
        }
        private List<Forecast> FilterForecasts(List<Forecast> forecasts, int addDays)
        {
            var res = new List<Forecast>();

            foreach (var f in forecasts)
            {
                var isDay = DateTime.Now.AddDays(addDays).DayOfWeek;
                if (f.Time.DayOfWeek == isDay)
                {
                    res.Add(f);
                }
            }

            return res;
        }
        private void UpdateUI()
        {
            OnPropertyChanged(nameof(Temperature));
            OnPropertyChanged(nameof(Humidity));
            OnPropertyChanged(nameof(SunriseTime));
            OnPropertyChanged(nameof(SunsetTime));
            OnPropertyChanged(nameof(WeatherIcon));
            OnPropertyChanged(nameof(TodayPlusTwo));
            OnPropertyChanged(nameof(TodayPlusThree));
            OnPropertyChanged(nameof(TodayPlusFour));
            OnPropertyChanged(nameof(SearchBtnText));
            OnPropertyChanged(nameof(FilteredForecasts));
        }
    }

    public class Forecast
    {
        private DateTime _time;
        private double _temp;

        public DateTime Time { get { return _time; } set { _time = value; } }
        public string DisplayTime { get { return _time.ToString("HH:mm") + " ч."; } set { return; } }

        public double Temp { get { return _temp; } set { _temp = value; } }
        public string DisplayTemp { get { return _temp.ToString() + " C"; } set { return; } }
        public Forecast(DateTime time, double temp)
        {
            Time = time;
            Temp = temp;
        }
    }
}
