using DataService.Core;
using WDS.Core.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System;
using WDS.Settings;
using System.Text.Json.Serialization;
using DataService.Core.Settings;
using Microsoft.Data.SqlClient;
using System.Data;
using WDS.Authentication;
using WDS.Authorization;
using System.IO;
using DataService.Core.Data;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using static DataService.Core.Settings.DataServiceSettings;
using static WDSClient.Models.WDSEnums;
using DataService.Core.Logging;

namespace DataService.Models.Settings
{
    public class ApiKeys
    {
        private ApiKeysModel _dataContext = new ApiKeysModel();
        
        public ApiKeysModel Model { get { return _dataContext; } }
        #region Constructor
        public ApiKeys() 
        { 

        }
        public ApiKeys(ApiKeysModel dataContext)
        {
            _dataContext = dataContext;
        }
        #endregion
        public void Load(string path, string connetionString)
        {
            _dataContext = JsonConvert.DeserializeObject<ApiKeysModel>(File.ReadAllText($"{path}\\apiKeys.json"));
            Load(connetionString);
        }
        public void Load(string connectionString)
        {
            try 
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand command = new SqlCommand($"SELECT * from [wds].[api_keys] WHERE is_active = 1 AND expire_date > getdate() AND server_host_id IN (SELECT host_id FROM [wds].[hosts] WHERE host_name = '{DataServiceSettings.Connections.HostName}' OR host_name = '*')", sqlConnection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable objTable = new DataTable();
                        adapter.Fill(objTable);

                        foreach (DataRow row in objTable.Rows)
                        {
                            if (!_dataContext.Keys.Any(o => o.ApiEncryptedKey == row["encrypted_apiKey"].ToString()))
                            {
                                _dataContext.Keys.Add(new ApiKeyModel()
                                {
                                    Name = row["name"].ToString(),
                                    ApiEncryptedKey = row["encrypted_apiKey"].ToString(),
                                    Roles = Roles.GetRolesFromAPIKey((int)row["apiKey_id"]),
                                    IPMask = row["host_mask"].ToString(),
                                    OwnerEmail = row["owner_email"].ToString(),
                                    ExpireDate = (DateTime)row["expire_date"],
                                    DateCreated = row["create_dt"].ToString()
                                });
                            }
                        }

                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                WDSLogs.AddLog($"Unable to load API keys from the database. {ex.Message}", LogLevels.Error); 
            }
            
        }
        public string AddAPIKey(string name, string ownerEmail, string ipMask, string description, List<string> roles = null)
        {
            var guid = Guid.NewGuid();
            var newKey = new ApiKeyModel(name, guid.ToString(), ownerEmail, ipMask, roles);
            _dataContext.Keys.Add(newKey);
            AddAPIKeyToDB(newKey, description);
            
            return guid.ToString();
        }
        public void RemoveAPIKey(string name)
        {
            var key = _dataContext.Keys.Where(o => (o.Name == name || o.ApiKey == name)).FirstOrDefault();
            if (key != null)
            {
                _dataContext.Keys.Remove(key);
                RemoveAPIKeyToDB(key);
            }
        }
        public bool DoesKeyExist(string apiKey)
        {
            return _dataContext.Keys.Any(o => o.ApiKey == apiKey);
        }
        public ApiKeyModel RetrieveKey(string apiKey)
        {
            ApiKeyModel returnValue = _dataContext.Keys.Where(o => o.ApiKey == apiKey).FirstOrDefault(); ;

            return returnValue;
        }
        public ApiKeyModel RetrieveKeyByName(string apiKeyName)
        {
            ApiKeyModel returnValue = _dataContext.Keys.Where(o => o.Name == apiKeyName).FirstOrDefault(); ;

            return returnValue;
        }
        public string GetIPMask(string apiKey)
        {
            var key = _dataContext.Keys.FirstOrDefault(o => o.ApiKey == apiKey);
            return key == null ? "KeyNotFound" : key.IPMask;
        }
        private void AddAPIKeyToDB(ApiKeyModel newKey, string description)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("serverHost", DataServiceSettings.Connections.HostName);
            parameters.Add("name", newKey.Name);
            parameters.Add("encryptedAPIKey", newKey.ApiEncryptedKey);
            parameters.Add("hostMask", newKey.IPMask);
            parameters.Add("description", description);
            parameters.Add("ownerEmail", newKey.OwnerEmail);

            string sqlStatement = $"EXEC [wds].[add_APIKey]";
            WDSWriteOperations.UpdateDB(sqlStatement, parameters);

            if (newKey.Roles != null && newKey.Roles.Count > 0)
            {
                Roles.WriteRolesToDB(newKey.Name, newKey.Roles);
            }
        }
        private void RemoveAPIKeyToDB(ApiKeyModel key)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("apiKeyId", "0");
            parameters.Add("apiKeyName", key.Name);

            string sqlStatement = $"EXEC [wds].[del_APIKey]";
            WDSWriteOperations.UpdateDB(sqlStatement, parameters);
        }
    }
    
    public class ApiKeysModel
    {
        [JsonProperty("ApiKeys")]
        [JsonPropertyName("ApiKeys")]
        public List<ApiKeyModel> Keys { get; set; } = new List<ApiKeyModel>();
    }

    public class ApiKeyModel
    {
        private string _wdsSalt = "C$NTC0M|J6$S!Rocks";
        public SourcedFromTypes SourcedFrom;

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Roles")]
        public List<string> Roles { get; set; }

        [JsonProperty("ApiEKey")]
        public string ApiEncryptedKey { get; set; }

        private string _apiKey = string.Empty;
        [JsonProperty("ApiKey")]
        public string ApiKey
        {
            get
            {
                if (_apiKey.IsNullOrEmpty())
                    _apiKey = ApiEncryptedKey.Length == 64 ? AesOperation.DecryptString(_wdsSalt, ApiEncryptedKey) : "";
                return _apiKey; 
            }            
        }

        [JsonProperty("OwnerEmail")]
        public string OwnerEmail { get; set; }
        [JsonProperty("DateCreated")]
        public string DateCreated { get; set; }

        private string _ipMask; 
        [JsonProperty("IP_Mask")]
        public string IPMask 
        { 
            get 
            {
                // We want to enforce explicit permission here. You have to provide a mask. 
                if (_ipMask.IsNullOrEmpty())
                    return "false";
                else
                    return _ipMask; 
            }
            set { _ipMask = value;  } 
        }

        [JsonProperty("ExpireDate")]
        public DateTime ExpireDate { get; set; }

        public ApiKeyModel()
        {

        }
        public ApiKeyModel(string name, string apiKey, string ownerEmail, string ipMask, List<string> roles)
        {
            Name = name; 
            ApiEncryptedKey = AesOperation.EncryptString(_wdsSalt, apiKey); 
            Roles= roles;
            OwnerEmail = ownerEmail;
            _ipMask = ipMask;
            DateCreated = DateTime.Now.ToString();
            ExpireDate = DateTime.Now.AddDays(365);
        }
    }
}
