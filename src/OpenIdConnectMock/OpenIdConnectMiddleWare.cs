namespace OpenIdConnectMock
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Owin;
    using Newtonsoft.Json;
    using OpenIdConnectMock.Models;

    internal sealed class OpenIdConnectMiddleWare : OwinMiddleware
    {
        private const string IssuerFormat = "{0}/22e92fa8-837a-4f2d-8324-b548a5ac0d38/v2.0/";

        private const string AuthorizationEndpointFormat = "{0}/te/{1}/{2}/oauth2/v2.0/authorize";

        private const string TokenEndpointFormat = "{0}/te/{1}/{2}/oauth2/v2.0/token";

        private const string EndSessionEndpointFormat = "{0}/te/{1}/{2}/oauth2/v2.0/logout";

        private const string JwksUriFormat = "{0}/te/{1}/{2}/discovery/v2.0/keys";

        private readonly Uri baseUri;

        private readonly string tenant;

        private readonly string defaultPolicy;

        public OpenIdConnectMiddleWare(OwinMiddleware next, Uri baseUri, string tenant, string defaultPolicy)
            : base(next)
        {
            this.baseUri = baseUri;
            this.tenant = tenant.ToLower();
            this.defaultPolicy = defaultPolicy.ToLower();
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (context.Request.Path.Value.EndsWith("openid-configuration"))
            {
                await HandleOpenIdConfigurationRequest(context);
            }
            else if (context.Request.Path.Value.EndsWith("authorize"))
            {
                await HandleAuthorizeRequest(context);
            }
            else
            {
                await Next.Invoke(context);
            }
        }

        private async Task HandleOpenIdConfigurationRequest(IOwinContext context)
        {
            var configuration = new OpenIdConfiguration
            {
                Issuer = string.Format(IssuerFormat, baseUri),
                AuthorizationEndpoint = string.Format(AuthorizationEndpointFormat, baseUri, tenant, defaultPolicy),
                TokenEndpoint = string.Format(TokenEndpointFormat, baseUri, tenant, defaultPolicy),
                EndSessionEndpoint = string.Format(EndSessionEndpointFormat, baseUri, tenant, defaultPolicy),
                JwksUri = string.Format(JwksUriFormat, baseUri, tenant, defaultPolicy),
                ResponseModeSupported = new List<string> { "query", "fragment", "form_post" },
                ResponseTypeSupported = new List<string> { "code", "code id_token", "code token", "code id_token token", "id_token", "id_token token", "token", "token id_token" },
                ScopesSupported = new List<string> { "openid" },
                SubjectTypeSupported = new List<string> { "pairwise" },
                IdTokenSigningAlgValuesSupported = new List<string> { "RS256" },
                TokenEndpointAuthMethodsSuported = new List<string> { "client_secret_post" },
                ClaimsSupported = new List<string> { "name", "given_name", "family_name", "email", "sub", "idp", "alternativeSecurityId", "authenticationSource", "role" }
            };

            context.Response.StatusCode = 200;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(configuration, Formatting.Indented));
        }

        private async Task HandleAuthorizeRequest(IOwinContext context)
        {
            var queries = HttpUtility.ParseQueryString(context.Request.Uri.Query);

            var responseType = queries["response_type"];
            var redirectUri = queries["redirect_uri"];
            var scope = queries["openid"];
            var state = queries["state"];

            // Return response.

            context.Response.StatusCode = 200;
            await context.Response.WriteAsync(string.Empty);
        }
    }
}