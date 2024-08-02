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
                var principal = JsonSerializer.Deserialize<ClientPrincipal>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new OkObjectResult(principal);

                //clientPrincipal = principal ?? new ClientPrincipal();
            }
        
            return new OkObjectResult(clientPrincipal);
        }
    }

    public class ClientPrincipal
    {
        public string? IdentityProvider { get; set; }
        public string? UserId { get; set; }
        public string? UserDetails { get; set; }
        public IEnumerable<string>? UserRoles { get; set; }
        public IEnumerable<SwaClaims>? Claims { get; set; }
        public string? AccessToken { get; set; }
    }

    public class SwaClaims
    {
        public string? Typ { get; set; }
        public string? Val { get; set; }
    }
}
