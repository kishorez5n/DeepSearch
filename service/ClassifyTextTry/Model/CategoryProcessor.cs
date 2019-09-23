using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Language.V1;
using Google.Apis.Services;
using Google.Apis.Discovery.v1;
using Google.Apis.Discovery.v1.Data;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Grpc.Auth;
using ClassifyTextTry.Model.Data;

namespace ClassifyTextTry.Model
{
    public class CategoryProcessor
    {
        private LanguageServiceClient serviceClient;

        private GraphProcessor graphProcessor;

        public static CategoryProcessor GetInstance()
        {
            CategoryProcessor categoryProcessor = new CategoryProcessor();

            var credential = GoogleCredential.FromFile("Config\\classifyNlp.json")
                .CreateScoped(LanguageServiceClient.DefaultScopes);
            var channel = new Grpc.Core.Channel(
                                    LanguageServiceClient.DefaultEndpoint.ToString(),
                                    credential.ToChannelCredentials());

            categoryProcessor.serviceClient = LanguageServiceClient.Create(channel);

            categoryProcessor.graphProcessor = GraphProcessor.GetInstance();

            return categoryProcessor;
        }

        public string GetF(string toClassify)
        {
            /*
             var credential = GoogleCredential.FromFile("C:\\work_perso\\gcp\\classifyNlp.json")
             */

            var credential = GoogleCredential.FromFile("Config\\classifyNlp.json")
                            .CreateScoped(LanguageServiceClient.DefaultScopes);
            var channel = new Grpc.Core.Channel(
                                    LanguageServiceClient.DefaultEndpoint.ToString(),
                                    credential.ToChannelCredentials());

            var client = LanguageServiceClient.Create(channel);
            var response = client.ClassifyText(new Document()
            {
                Content = toClassify,
                Type = Document.Types.Type.PlainText
            });
            var cat = response.Categories;
            Console.WriteLine($"Categories: {cat}");

            return cat.ToString();
        }

        public string Get(string toClassify)
        {
            var response = this.serviceClient.ClassifyText(new Document()
            {
                Content = toClassify,
                Type = Document.Types.Type.PlainText
            });
            var cat = response.Categories;
            Console.WriteLine($"Categories: {cat}");

            var k = cat[0].Name.Split(new string[] { "/" }, StringSplitOptions.None);

            return cat.ToString();
        }

        public string ClassifyData(LinkData toClassify)
        {
            var response = this.serviceClient.ClassifyText(new Document()
            {
                Content = toClassify.Text,
                Type = Document.Types.Type.PlainText
            });
            var categories = response.Categories;
            Console.WriteLine($"Categories: {categories}");

            foreach(var category in categories)
            {
                this.graphProcessor.AddToGraph(category.Name, toClassify.Link);
            }

            return categories.ToString();
        }

        private async Task Run()
        {
            // Create the service.
            var service = new DiscoveryService(new BaseClientService.Initializer
            {
                ApplicationName = "Discovery Service app",
                ApiKey = "AIzaSyCkUpbkmeAWXiSeTZrj97mpLp92mpcBDCs",
            });

            // Run the request.
            Console.WriteLine("Executing a list request...");
            var result = await service.Apis.List().ExecuteAsync();

            // Display the results.
            if (result.Items != null)
            {
                foreach (DirectoryList.ItemsData api in result.Items)
                {
                    Console.WriteLine(api.Id + " - " + api.Title);
                }
            }
        }


        private async Task<string> SendRequest(string textContent)
        {

            var docContent = new Document()
            {
                Content = textContent,
                Type = Document.Types.Type.PlainText
            };

            string docJson = JsonConvert.SerializeObject(docContent);

            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://language.googleapis.com/v1beta2/documents:classifyText");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
                // request.Headers.Add("key", "AIzaSyCkUpbkmeAWXiSeTZrj97mpLp92mpcBDCs");
                request.Content = new StringContent(docJson, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                var respContent = await response.Content.ReadAsStringAsync();

                return respContent;
            }
        }
    }
}
