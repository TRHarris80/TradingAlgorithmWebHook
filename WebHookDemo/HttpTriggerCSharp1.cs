using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public static class HttpTriggerCSharp1
    {
        [FunctionName("HttpTriggerCSharp1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            
            string price = req.Query["price"];
            string symbol = req.Query["symbol"];
            string quantity = req.Query["quantity"];
            string passphrase = req.Query["passphrase"];

           /* 
                https://astraalgo-webhookdemo.azurewebsites.net/api/HttpTriggerCSharp1?code=8PYLvly%2Fo3uOoZrBw%2FmddDJ1Sd1tC46LElbkrZhZIdzlTLQea12fBQ%3D%3D
                {       
                    "name": "Trenton",
	                "price": {{close}},
	                "symbol": {{ticker}},
	                "quantity": "5",
	                "passphrase": "Stu"
                }
            */



            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            price = price ?? data?.price;
            symbol = symbol ?? data?.symbol;
            quantity = quantity ?? data?.quantity;
            passphrase = passphrase ?? data?.passphrase;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. The price is {price}, the symbol is {symbol}, and the quantity is {quantity}";

            return new OkObjectResult(responseMessage);
        }
    }
}
