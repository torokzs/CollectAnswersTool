using CAT.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using System;
using System.Runtime;

namespace ChatbotApi.Controllers
{
    [ApiController]
    [Route("api/Chat/company")]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly AOAISettings _aoaiSettings;

        public CompanyController(ILogger<CompanyController> logger, IOptions<AOAISettings> aoaiSettings)
        {
            _logger = logger;
            _aoaiSettings = aoaiSettings.Value;
        }

        [HttpPost]
        public IActionResult Post([FromBody] RequestModel request)
        {
            // Process the request and generate the response
            string response = ProcessRequest(request);

            // Return the response
            return Ok(response);
        }

        private string ProcessRequest(RequestModel request)
        {
            var kernel = GetKernel(); // Would be better to use a singleton kernel
            var answer = CollectAnswer(request.Question, kernel);

            ResponseModel response = new()
            {
                Role = "string",
                ContentPath = "string",
                Result = answer,
                MessageDate = DateTime.UtcNow,
                DailyRequestCount = 0,
                DailyRemainingRequestCount = 0
            };

            string jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            return jsonResponse;
        }

        private Kernel GetKernel()
        {
            string deploymentName = _aoaiSettings.ModelDeploymentName;
            if (string.IsNullOrEmpty(deploymentName))
            {
                Console.WriteLine("ERROR: AOAIModelDeploymentName is not configured.");
            }

            string endpoint = _aoaiSettings.Endpoint;
            if (string.IsNullOrEmpty(endpoint))
            {
                Console.WriteLine("ERROR: AOAIEndpoint is not configured.");
            }

            string apiKey = _aoaiSettings.ApiKey;
            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("ERROR: AOAIApiKey is not configured.");
            }

            var builder = Kernel.CreateBuilder();
            builder.AddAzureOpenAIChatCompletion(deploymentName, endpoint, apiKey);
            var kernel = builder.Build();

            return kernel;
        }

        // Collect answer from LLM
        private static string CollectAnswer(string question, Kernel kernel)
        {
            if (string.IsNullOrEmpty(question))
            {
                Console.WriteLine("WARNING: Question is empty.");
                return string.Empty;
            }

            try
            {
                var response = kernel.InvokePromptAsync(question).Result;
                return response.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                return string.Empty;
            }
        }
    }
}
