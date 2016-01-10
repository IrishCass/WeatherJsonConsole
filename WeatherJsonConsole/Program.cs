using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherJsonConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            WeatherApiClient.GetWeatherForecast("47546");
            //WeatherApiClient.GetWeatherForecastAsync("47546");
            Console.ReadLine();
        }
    }
}
