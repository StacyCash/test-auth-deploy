using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
            return new OkObjectResult(req.Headers["x-ms-client-principal"][0]);
        }
    }
}
