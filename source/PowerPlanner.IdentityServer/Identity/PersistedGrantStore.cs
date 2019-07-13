using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Identity
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        // This should be updated to commit to actual SQL database: https://mcguirev10.com/2018/01/02/identityserver4-without-entity-framework.html

        private List<PersistedGrant> _persistedGrants = new List<PersistedGrant>();

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            return _persistedGrants.Where(i => i.SubjectId == subjectId).ToArray();
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            return _persistedGrants.FirstOrDefault(i => i.Key == key);
        }

        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            _persistedGrants.RemoveAll(i => i.SubjectId == subjectId && i.ClientId == clientId);
        }

        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            _persistedGrants.RemoveAll(i => i.SubjectId == subjectId && i.ClientId == clientId && i.Type == type);
        }

        public async Task RemoveAsync(string key)
        {
            _persistedGrants.RemoveAll(i => i.Key == key);
        }

        public async Task StoreAsync(PersistedGrant grant)
        {
            await RemoveAsync(grant.Key);
            _persistedGrants.Add(grant);
        }
    }
}
