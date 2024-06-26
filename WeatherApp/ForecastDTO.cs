﻿namespace WeatherApp
{
    using System;
    using System.Collections.Generic;

    public partial class ForecastDTO
    {
        public long Cod { get; set; }
        public long Message { get; set; }
        public long Cnt { get; set; }
        public List<List> List { get; set; }
        public City City { get; set; }
    }

    public partial class CurrWeatherDTO
    {
        public Coord Coord { get; set; }
        public List<Weather> Weather { get; set; }
        public string Base { get; set; }
        public Main Main { get; set; }
        public long Visibility { get; set; }
        public Wind Wind { get; set; }
        public Rain Rain { get; set; }
        public Clouds Clouds { get; set; }
        public long Dt { get; set; }
        public Sys Sys { get; set; }
        public long Timezone { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public long Cod { get; set; }
    }

    public partial class Clouds
    {
        public long All { get; set; }
    }

    public partial class Coord
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
    }

    public partial class Main
    {
        private double _temp;
        public double Temp { get { return _temp; } set { _temp = Math.Round(value - 273.15, 1); } }
        public double FeelsLike { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public long Pressure { get; set; }
        public long Humidity { get; set; }
        public long SeaLevel { get; set; }
        public long GrndLevel { get; set; }
    }

    public partial class Rain
    {
        public double The1H { get; set; }
    }

    public partial class Sys
    {
        public long Type { get; set; }
        public long Id { get; set; }
        public string Country { get; set; }
        public long Sunrise { get; set; }
        public long Sunset { get; set; }
    }

    public partial class Weather
    {
        public long Id { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    public partial class Wind
    {
        public double Speed { get; set; }
        public long Deg { get; set; }
        public double Gust { get; set; }
    }

    public partial class City
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Coord Coord { get; set; }
        public string Country { get; set; }
        public long Population { get; set; }
        public long Timezone { get; set; }
        public long Sunrise { get; set; }
        public long Sunset { get; set; }
    }

    public partial class List
    {
        public long Dt { get; set; }
        public Main Main { get; set; }
        public List<Weather> Weather { get; set; }
        public Clouds Clouds { get; set; }
        public Wind Wind { get; set; }
        public long Visibility { get; set; }
        public double Pop { get; set; }
        public Rain Rain { get; set; }
        public Sys Sys { get; set; }
        public DateTimeOffset DtTxt { get; set; }
    }


    public partial class Rain
    {
        public double The3H { get; set; }
    }

    public partial class Sys
    {
        public string Pod { get; set; }
    }
}
