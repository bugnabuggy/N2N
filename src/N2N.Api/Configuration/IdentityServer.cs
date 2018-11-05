using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace N2N.Api.Configuration
{
    public class IdentityServer
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                new IdentityResources.Phone(),
                new IdentityResources.Address()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new[]
            {
                // simple API with a single scope (in this case the scope name is the same as the api name)
                new ApiResource(SiteConstants.ApiName),

            };
        }


        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
                {
                    new Client
                    {
                        ClientId = "frontend",
                        ClientName = "N2N site",
                        
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                        AccessTokenType = AccessTokenType.Jwt,
                        AccessTokenLifetime = SiteConstants.AccessTokenLifeTime,
                        AllowAccessTokensViaBrowser = true,
                        ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },

                        RequireConsent = false,
                        AllowedScopes =
                        {
                            SiteConstants.ApiName
                        }
                    }
                };
        }
    }
}
