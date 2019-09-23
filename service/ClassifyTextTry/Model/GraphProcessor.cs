using ClassifyTextTry.Model.Data;
using ClassifyTextTry.Utils;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Exceptions;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassifyTextTry.Model
{
    public class GraphProcessor
    {
        private static readonly string hostname = "cosmosgraphserv.gremlin.cosmosdb.azure.com";
        private static readonly int port = 443;
        private static readonly string authKey = "<removed>";
        private static readonly string database = "graphdb";
        private static readonly string collection = "Persons";

        private GremlinClient gremlinClient;

        public static GraphProcessor GetInstance()
        {
            GraphProcessor query = new GraphProcessor();
            var gremlinServer = new GremlinServer(hostname, port, enableSsl: true,
                            username: "/dbs/" + database + "/colls/" + collection,
                            password: authKey);

            query.gremlinClient = new GremlinClient(gremlinServer, new GraphSON2Reader(), new GraphSON2Writer(), GremlinClient.GraphSON2MimeType);
            return query;
        }

        public string AddToGraph(string categoryName, string link)
        {
            var k = categoryName.Split(new string[] { "/" }, StringSplitOptions.None);

            var length = k.Length;

            string linkSha = GetSha256FromString(link);

            Dictionary<string, string> queries = new Dictionary<string, string>();
            if (!IfExists(new KeyValuePair<string, string>(nameof(QueryConstants.GetVertex), string.Format(CultureInfo.CurrentCulture, QueryConstants.GetVertex, k[1]))))
            {
                queries.Add(nameof(QueryConstants.AddVertexCategory), string.Format(CultureInfo.CurrentCulture, QueryConstants.AddVertexCategory, k[1]));
            }

            if (!IfExists(new KeyValuePair<string, string>(nameof(QueryConstants.GetVertex), string.Format(CultureInfo.CurrentCulture, QueryConstants.GetVertex, linkSha))))
            {
                queries.Add(nameof(QueryConstants.AddVertexLinkdata), string.Format(CultureInfo.CurrentCulture, QueryConstants.AddVertexLinkdata, linkSha, link));
            }

            if (length > 2)
            {
                if (!IfExists(new KeyValuePair<string, string>(nameof(QueryConstants.GetVertex), string.Format(CultureInfo.CurrentCulture, QueryConstants.GetVertex, k[2]))))
                {
                    queries.Add(nameof(QueryConstants.AddVertexSubcategory), string.Format(CultureInfo.CurrentCulture, QueryConstants.AddVertexSubcategory, k[2]));
                    queries.Add(nameof(QueryConstants.AddEdgeCategoryToSubcategory), string.Format(CultureInfo.CurrentCulture, QueryConstants.AddEdgeCategoryToSubcategory, k[1], k[2]));
                }

                queries.Add(nameof(QueryConstants.AddEdgeSubOrCategoryToLinkdata), string.Format(CultureInfo.CurrentCulture, QueryConstants.AddEdgeSubOrCategoryToLinkdata, k[2], linkSha));
            }
            else
            {
                queries.Add(nameof(QueryConstants.AddEdgeSubOrCategoryToLinkdata), string.Format(CultureInfo.CurrentCulture, QueryConstants.AddEdgeSubOrCategoryToLinkdata, k[1], linkSha));
            }

            return RunQueries(queries);
        }

        public string GetCategories()
        {
            Dictionary<string, string> queries = new Dictionary<string, string>();
            queries.Add(nameof(QueryConstants.AllCategory), QueryConstants.AllCategory);
            return RunQueries(queries);
        }

        public string GetSubCategories(string category)
        {
            Dictionary<string, string> queries = new Dictionary<string, string>();
            queries.Add(nameof(QueryConstants.AllSubCategory), string.Format(CultureInfo.CurrentCulture, QueryConstants.AllSubCategory, category));
            return RunQueries(queries);
        }

        public string GetCategoryLinks(string category)
        {
            Dictionary<string, string> queries = new Dictionary<string, string>();
            queries.Add(nameof(QueryConstants.AllCategoryLinks0), string.Format(CultureInfo.CurrentCulture, QueryConstants.AllCategoryLinks0, category));
            queries.Add(nameof(QueryConstants.AllCategoryLinks1), string.Format(CultureInfo.CurrentCulture, QueryConstants.AllCategoryLinks1, category));
            queries.Add(nameof(QueryConstants.AllCategoryLinks2), string.Format(CultureInfo.CurrentCulture, QueryConstants.AllCategoryLinks2, category));
            queries.Add(nameof(QueryConstants.AllCategoryLinks3), string.Format(CultureInfo.CurrentCulture, QueryConstants.AllCategoryLinks3, category));
            return RunQueries(queries);
        }

        /*
        public string GetSubCategoryLinks(string category)
        {
            Dictionary<string, string> queries = new Dictionary<string, string>();
            queries.Add(nameof(QueryConstants.AllSubCategoryLinks1), string.Format(CultureInfo.CurrentCulture, QueryConstants.AllSubCategoryLinks1, category));
            queries.Add(nameof(QueryConstants.AllSubCategoryLinks2), string.Format(CultureInfo.CurrentCulture, QueryConstants.AllSubCategoryLinks2, category));
            return RunQueries(queries);
        }
        */

        public bool IfExists(KeyValuePair<string, string> query)
        {
            var result = RunQuery(query);
            var obj = JsonConvert.DeserializeObject<QueryResultNode[]>(result);
            if(obj.Length >= 1)
            {
                return true;
            }

            return false;
        }

        public string RunQuery(KeyValuePair<string, string> query)
        {
            Console.WriteLine(String.Format("Running this query: {0}: {1}", query.Key, query.Value));

            // Create async task to execute the Gremlin query.
            var resultSet = SubmitRequest(this.gremlinClient, query).Result;
            if (resultSet.Count > 0)
            {
                Console.WriteLine("\tResult:");
                foreach (var result in resultSet)
                {
                    var output = JsonConvert.SerializeObject(result);
                    Console.WriteLine($"\t{output}");
                }
                Console.WriteLine();
            }

            PrintStatusAttributes(resultSet.StatusAttributes);
            Console.WriteLine();

            return JsonConvert.SerializeObject(resultSet);
        }

        public string RunQueries(Dictionary<string, string> queries)
        {
            IEnumerable<dynamic> finalOutput = null;
            foreach (var query in queries)
            {
                Console.WriteLine(String.Format("Running this query: {0}: {1}", query.Key, query.Value));

                var resultSet = SubmitRequest(this.gremlinClient, query).Result;

                if(finalOutput == null)
                {
                    finalOutput = resultSet;
                }
                else
                {
                    finalOutput = finalOutput.Concat(resultSet);
                }

                if (resultSet.Count > 0)
                {
                    Console.WriteLine("\tResult:");
                    foreach (var result in resultSet)
                    {
                        var output = JsonConvert.SerializeObject(result);
                        Console.WriteLine($"\t{output}");
                    }
                    Console.WriteLine();
                }

                PrintStatusAttributes(resultSet.StatusAttributes);
                Console.WriteLine();
            }
            return JsonConvert.SerializeObject(finalOutput);
        }

        public static string GetSha256FromString(string strData)
        {
            var message = Encoding.ASCII.GetBytes(strData);
            SHA256Managed hashString = new SHA256Managed();
            string hex = "";

            var hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        private static Task<ResultSet<dynamic>> SubmitRequest(GremlinClient gremlinClient, KeyValuePair<string, string> query)
        {
            try
            {
                return gremlinClient.SubmitAsync<dynamic>(query.Value);
            }
            catch (ResponseException e)
            {
                Console.WriteLine("\tRequest Error!");

                // Print the Gremlin status code.
                Console.WriteLine($"\tStatusCode: {e.StatusCode}");

                // On error, ResponseException.StatusAttributes will include the common StatusAttributes for successful requests, as well as
                // additional attributes for retry handling and diagnostics.
                // These include:
                //  x-ms-retry-after-ms         : The number of milliseconds to wait to retry the operation after an initial operation was throttled. This will be populated when
                //                              : attribute 'x-ms-status-code' returns 429.
                //  x-ms-activity-id            : Represents a unique identifier for the operation. Commonly used for troubleshooting purposes.
                PrintStatusAttributes(e.StatusAttributes);
                Console.WriteLine($"\t[\"x-ms-retry-after-ms\"] : { GetValueAsString(e.StatusAttributes, "x-ms-retry-after-ms")}");
                Console.WriteLine($"\t[\"x-ms-activity-id\"] : { GetValueAsString(e.StatusAttributes, "x-ms-activity-id")}");

                throw;
            }
        }

        private static void PrintStatusAttributes(IReadOnlyDictionary<string, object> attributes)
        {
            Console.WriteLine($"\tStatusAttributes:");
            Console.WriteLine($"\t[\"x-ms-status-code\"] : { GetValueAsString(attributes, "x-ms-status-code")}");
            Console.WriteLine($"\t[\"x-ms-total-request-charge\"] : { GetValueAsString(attributes, "x-ms-total-request-charge")}");
        }

        public static string GetValueAsString(IReadOnlyDictionary<string, object> dictionary, string key)
        {
            return JsonConvert.SerializeObject(GetValueOrDefault(dictionary, key));
        }

        public static object GetValueOrDefault(IReadOnlyDictionary<string, object> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return null;
        }

    }
}
