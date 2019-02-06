using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EnvironmentVariableDemo.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }

        [HttpGet("[action]")]
        public IEnumerable<EnvironmentSystemVariable> EnvironmentSystemVariables()
        {
            // 0 = Development
            // 1 = Quality Assurance
            // 2 = Internal Use
            // 3 = Granicus Hosted Staging Site
            // 4 = Granicus Hosted Live Site
            // 5 = Client Hosted Staging Site
            // 6 = Client Hosted Live Site

            // Registry Path for Network Service = Computer\HKEY_USERS\S-1-5-20

            string envName = "govAccessEnvironment";
            string envValue = "0";

            // Determine whether the environment variable exists.
            if (Environment.GetEnvironmentVariable(envName, EnvironmentVariableTarget.User) == null)
            {
                // If it doesn't exist, create it.
                // However we need to decide the default environment if this variable is empty

                // FIRST WAY: USE CURRENT LOGIN -- NETWORK SERVICE, VDC1\CMS6User
                // Not so easy to setup, registry file script must know which user is being used by application pool

                Environment.SetEnvironmentVariable(envName, envValue, EnvironmentVariableTarget.User);

                // SECOND WAY: USE MACHINE ENVIRONMENT SYSTEM VARIABLE
                // Can be set only by Administrator by code but
                // We can use Registry script in initial install using Administrator account to put in the environment system variable
                // We can also add this via Windows GUI on the My Computer properties page                
                if (envValue == "impossible string")
                { 
                    Environment.SetEnvironmentVariable(envName, envValue, EnvironmentVariableTarget.Machine);
                }
            }

            return new List<EnvironmentSystemVariable>()
            {
                new EnvironmentSystemVariable() { Name = "govAccessEnvironment", Value = Environment.GetEnvironmentVariable(envName, EnvironmentVariableTarget.User) },
            };
        }

        public class EnvironmentSystemVariable
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}
