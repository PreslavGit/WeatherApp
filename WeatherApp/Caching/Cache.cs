using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Caching
{
    internal class Cache
    {
        [PrimaryKey]
        public string location { get; set; }
        public string forecast { get; set; }
        public DateTime cached_at { get; set; }
    }
}
