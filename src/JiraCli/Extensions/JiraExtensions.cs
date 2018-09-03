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

        public static JToken ExecuteRequestRaw(this IJiraRestClient jiraRestClient, Method method, string resource, string jsonRequestBody)
        {
            Argument.IsNotNull(() => jiraRestClient);
            Argument.IsNotNullOrWhitespace(() => jsonRequestBody);

            var restRequest = new RestRequest
            {
                Resource = resource,
                Method = method,
                RequestFormat = DataFormat.Json,
            };

            restRequest.AddParameter(new Parameter
            {
                Name = "application/json",
                Type = ParameterType.RequestBody,
                Value = jsonRequestBody
            });

            try
            {
                var response = jiraRestClient.ExecuteRequest(restRequest);
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