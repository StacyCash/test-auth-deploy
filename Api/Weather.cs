using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;

namespace Api
{
    public static class Weather
    {
        [FunctionName("Weather")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            return new OkObjectResult("""
[
  {
    "date": "2022-01-06",
    "temperatureC": 1,
    "summary": "Freezing"
  },
  {
                "date": "2022-01-07",
    "temperatureC": 14,
    "summary": "Bracing"
  },
  {
                "date": "2022-01-08",
    "temperatureC": -13,
    "summary": "Freezing"
  },
  {
                "date": "2022-01-09",
    "temperatureC": -16,
    "summary": "Balmy"
  },
  {
                "date": "2022-01-10",
    "temperatureC": -2,
    "summary": "Chilly"
  }
]
"""
);
        }

        [FunctionName("Auth")]
        public static async Task<IActionResult> Auth(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            ClientPrincipal clientPrincipal;
            if (!req.Headers.TryGetValue("x-ms-client-principal", out var header))
            {
                clientPrincipal = new();
            }
            else
            {
                var data = header[0];
                var decoded = Convert.FromBase64String(data);
                var json = Encoding.UTF8.GetString(decoded);
                try
                {
                    var principal = JsonSerializer.Deserialize<ClientPrincipal>(json);

                    return new OkObjectResult(principal);
                }
                catch (Exception ex)
                {
                    return new OkObjectResult(ex.Message);
                }

                //clientPrincipal = principal ?? new ClientPrincipal();
            }
        
            return new OkObjectResult(clientPrincipal);
        }
    }

    public class ClientPrincipal
    {
        public string? identityProvider { get; set; }
        public string? userId { get; set; }
        public string? userDetails { get; set; }
        public IEnumerable<string>? userRoles { get; set; }
        public IEnumerable<SwaClaims>? claims { get; set; }
        public string? accessToken { get; set; }
    }

    public class SwaClaims
    {
        public string? typ { get; set; }
        public string? val { get; set; }
    }
}
