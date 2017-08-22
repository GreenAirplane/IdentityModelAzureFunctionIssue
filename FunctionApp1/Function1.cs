using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "AzureFunction Test");

                string url = "https://api.github.com/users/brockallen";
                HttpResponseMessage response = await client.GetAsync(url);

                GitHubUser user = await response.Content.ReadAsAsync<GitHubUser>();

                string result = $"User Id: {user.Id}; Login: {user.Login}";
                log.Info(result);
                return req.CreateResponse(HttpStatusCode.OK, result);
            }
        }
    }

    public class GitHubUser
    {
        public string Login { get; set; }
        public string Id { get; set; }
    }
}
