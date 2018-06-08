namespace OpenIdConnectMock
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
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
            else if (context.Request.Path.Value.EndsWith("keys"))
            {
                await HandleKeysRequest(context);
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

        private async Task HandleKeysRequest(IOwinContext context)
        {
            var response = "{\r\n  \"keys\": [\r\n    {\"kid\":\"xFuaJp6VZyqg_FBkzb0QIMdw5u09s3wFvnQ7p2Ngw8Y\",\"use\":\"sig\",\"key_ops\":[\"sign\"],\"kty\":\"RSA\",\"e\":\"AQAB\",\"n\":\"rxaBX-gqdL7ja0KLiAN7PIb1tJDASAvgFm2cziKeOdwkgGpvnL7yJrqn2zNfLgQPhsp_Ua4l2dTOpR1OdMPXgnY8u-n7ZP3BJINQqXA6qIU-_4YuVEmmpbE-Yu1eOQJRkUEqlhsb-t6K59EJKSciSwRx4nKWhC7WwPD9-sPcEgYyRqpNF2qtxr7ClVuGncq33j8ifAcRYKLN18Qd3ofvgDHKj9YBr81TeSJpJoyJ5KLHOknUTxHFSUdsN54wZADlppwGQLBjnRkqsErzVaHe1pea8qUONrbrc5vieL_7cewpLRViEvtsqSCcOBvTaZCcIAnJRySSsglFLtF9NAayNw\"}\r\n  ]\r\n}";
            await context.Response.WriteAsync(response);
        }

        private async Task HandleAuthorizeRequest(IOwinContext context)
        {
            var queries = HttpUtility.ParseQueryString(context.Request.Uri.Query);

            var redirectUri = queries["redirect_uri"];
            var responseMode = queries["response_mode"];
            var responseType = queries["response_type"];
            var scope = queries["openid"];
            var state = queries["state"];

            const string code = "AwABAAAAvPM1KaPlrEqdFSBzjqfTGBCmLdgfSTLEMPGYuNHSUYBrqqf_ZT_p5uEAEJJ_nZ3UmphWygRNy2C3jJ239gV_DBnZ2syeg95Ki-374WHUP-i3yIhv5i-7KU2CEoPXwURQp6IVYMw-DjAOzn7C3JCu5wpngXmbZKtJdWmiBzHpcO2aICJPu1KvJrDLDP20chJBXzVYJtkfjviLNNW7l7Y3ydcHDsBRKZc3GuMQanmcghXPyoDg41g8XbwPudVh7uCmUponBQpIhbuffFP_tbV8SNzsPoFz9CLpBCZagJVXeqWoYMPe2dSsPiLO9Alf_YIe5zpi-zY4C3aLw5g9at35eZTfNd0gBRpR5ojkMIcZZ6IgAA";

            if (responseMode == "form_post")
            {
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync("Authentication succesfull..");

                using (var httpClient = new HttpClient())
                {
                    var redirectLocation = string.Format("id_token={0}&session_state={1}&state={2}", code, Guid.NewGuid().ToString().ToUpper(), Guid.NewGuid().ToString().ToUpper());
                    await httpClient.PostAsync(redirectUri, new StringContent(redirectLocation));
                }
            }
            else
            {
                // defaulting to query.
                var redirectLocation = string.Format("{0}?code={1}&session_state={2}&state={3}", redirectUri, code, Guid.NewGuid().ToString().ToUpper(), Guid.NewGuid().ToString().ToUpper());
                context.Response.Redirect(redirectLocation);
            }
        }
    }
}