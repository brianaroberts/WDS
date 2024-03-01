using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WDS.Authorization;
using DataService.Models.Settings;
using DataService.Core.Settings;

namespace WDS.Authentication
{
    public class InMemoryGetApiKeyQuery : IGetApiKeyQuery
    {
        private readonly IDictionary<string, ApiKey> _apiKeys;

        public InMemoryGetApiKeyQuery()
        {
            _apiKeys = new Dictionary<string, ApiKey>();
            var i = 1; 

            foreach (var key in DataServiceSettings.ApiKeys.Model.Keys)
            {
                _apiKeys.Add(key.Name, new ApiKey(i++, key.OwnerEmail, key.ApiKey, DateTime.Now, key.IPMask, key.Roles));                     
            }
        }

        public Task<ApiKey> Execute(string providedApiKey)
        {
            var apiKey = _apiKeys.Where(o => o.Value.Key == providedApiKey);
            var apiKeyValue = apiKey.Count() == 0  ? null : apiKey.First().Value;

            //_apiKeys.TryGetValue(providedApiKey, out var apiKey);
            return Task.FromResult(apiKeyValue);
        }
    }
}
