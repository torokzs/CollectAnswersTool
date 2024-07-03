using CAT.Common;
using Newtonsoft.Json;
using System.Configuration;
using System.Text;

namespace CollectAnswersTool
{
    public class ChatbotClient
    {
        public static async Task<string> PostQuestionAsync(string question)
        {
            var requestModel = new RequestModel
            {
                Question = question,
                ConversationId = null
            };

            var jsonRequest = JsonConvert.SerializeObject(requestModel);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Direct instantiation of HttpClient (use with caution)
            using (var httpClient = new HttpClient())
            {
                string? baseUrl = ConfigurationManager.AppSettings["ChatbotUrl"];
                if (string.IsNullOrEmpty(baseUrl))
                {
                    Console.WriteLine("ERROR: ChatbotUrl is not configured properly, please check configuration settings.");
                    return string.Empty;
                }

                var response = await httpClient.PostAsync($"{baseUrl}", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var modelResponse = JsonConvert.DeserializeObject<ResponseModel>(jsonResponse);
                    if (modelResponse == null || modelResponse.Result == null)
                    {
                        Console.WriteLine("ERROR: The response model or result is null.");
                        return string.Empty;
                    }

                    return modelResponse.Result;
                }
                else
                {
                    Console.WriteLine($"ERROR: Failed to post question. Status code: {response.StatusCode}");
                    return string.Empty;
                }
            }
        }
    }
}