// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JiraExtensions.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2015 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System.Net;
    using Atlassian.Jira;
    using Catel;
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    using RestSharp;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public static partial class JiraExtensions
    {
        private static JsonSerializerSettings GetJsonSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy-MM-dd"
            };
        }

        public static async Task<JToken> ExecuteRequestRawAsync(this Atlassian.Jira.Remote.IJiraRestClient jiraRestClient, Method method, string resource, string jsonRequestBody)
        {
            Argument.IsNotNull(() => jiraRestClient);
            Argument.IsNotNullOrWhitespace(() => jsonRequestBody);

            var restRequest = new RestRequest
            {
                Resource = resource,
                Method = method,
                RequestFormat = DataFormat.Json,
            };

            restRequest.AddParameter(new Parameter("application/json", jsonRequestBody, ParameterType.RequestBody));

            try
            {
                var response = await jiraRestClient.ExecuteRequestAsync(restRequest);
                return response.StatusCode != HttpStatusCode.NoContent ? JToken.Parse(response.Content) : new JObject();
            }
            catch (System.Exception ex)
            {
                Debug.Write(ex);
                throw;
            }
          
        }
    }
}
