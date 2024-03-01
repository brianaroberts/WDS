using System;
using System.Collections.Generic;

namespace WDS.Authentication
{
    public class ApiKey
    {
        public int Id { get; }
        public string Owner { get; }
        public string Key { get; }
        public string IPAddressMask { get; }
        public DateTime Created { get; }
        public IReadOnlyCollection<string> Roles { get; }
        

        public ApiKey(int id, string owner, string key, DateTime created, string ipmask, IReadOnlyCollection<string> roles)
        {
            Id = id;
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Created = created;
            IPAddressMask = ipmask ?? throw new ArgumentNullException(nameof(ipmask));
            Roles = roles ?? new List<string>();
        }
    }
}
