using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace WeatherJsonConsole
{
    public static class WeatherApiClient
    {
        private const string baseUrl = "http://api.openweathermap.org/data/2.5/weather?zip={0},us&APPID=29fc6bd4dab811257787c5691cb04658";

        public static void GetWeatherForecast(string zip)
        {
            // Blog with example:  http://blog.anthonybaker.me/2013/05/how-to-consume-json-rest-api-in-net.html
            // http://openweathermap.org/appid

            //var url1 = "http://api.openweathermap.org/data/2.1/find/city?lat=51.50853&lon=-0.12574&cnt=10";
            var urlByCityCode = "http://api.openweathermap.org/data/2.5/forecast/city?id=524901&APPID=29fc6bd4dab811257787c5691cb04658";
            //var urlByZip = "http://api.openweathermap.org/data/2.5/weather?zip=47546,us&APPID=29fc6bd4dab811257787c5691cb04658";
            var urlByZip = string.Format(baseUrl, zip);

            //var syncClient = new WebClient();
            //var content = syncClient.DownloadString(url);

            // http://stackoverflow.com/questions/5999215/exception-handling-the-right-way-for-webclient-downloadstring

            // How to create C# class code from JSON string
            // http://json2csharp.com/#

            string html;
            using (WebClient client = new WebClient())
            {
                try
                {
                    html = client.DownloadString(urlByZip);
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(WeatherData));
                    using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(html)))
                    {
                        // deserialize the JSON object using the WeatherData type.
                        var weatherData = (WeatherData)serializer.ReadObject(ms);
                    }
                    Console.WriteLine(html);
                }
                catch (WebException e)
                {
                    //How do I capture this from the UI to show the error in a message box?
                    Console.WriteLine(e.Message);
                    //throw e;
                }
            }

        }


        /// <summary>
        /// Retrieves the Weather Forecast data asynchronously.
        /// </summary>
        /// <param name="latitude">latitude of the geolocation</param>
        /// <param name="longitude">longitud of the geolocation</param>
        /// <param name="stationQuantity">number of weather stations to be included</param>
        public static void GetWeatherForecastAsync(string zip)
        {
            // Customize URL according to geo location parameters
            var urlByZip = string.Format(baseUrl, zip);

            // Async Consumption
            var asyncClient = new WebClient();
            asyncClient.DownloadStringCompleted += asyncClient_DownloadStringCompleted;
            asyncClient.DownloadStringAsync(new Uri(urlByZip));

            // Do something else...
        }

        /// <summary>
        /// Parses the weather data once the response download is completed.
        /// </summary>
        /// <param name="sender">object that originated the event</param>
        /// <param name="e">event arguments</param>
        static void asyncClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            // Create the Json serializer and parse the response
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(WeatherData));
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(e.Result)))
            {
                // deserialize the JSON object using the WeatherData type.
                var weatherData = (WeatherData)serializer.ReadObject(ms);
            }
            Console.WriteLine(e.Result);
        }

    }
}
