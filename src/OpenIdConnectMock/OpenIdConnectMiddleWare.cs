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
        private readonly Uri baseUri;

        public OpenIdConnectMiddleWare(Uri baseUri, OwinMiddleware next)
            : base(next)
        {
            this.baseUri = baseUri;
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
                ResponseModeSupported = new List<string> { "query", "fragment", "form_post" },
                ResponseTypeSupported = new List<string> { "code", "code id_token", "code token", "code id_token token", "id_token", "id_token token", "token", "token id_token" }
            };

            context.Response.StatusCode = 200;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(configuration));
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