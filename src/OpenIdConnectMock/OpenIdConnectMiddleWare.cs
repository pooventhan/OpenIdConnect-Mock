namespace OpenIdConnectMock
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Owin;

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
            throw new NotImplementedException();
        }

        private async Task HandleAuthorizeRequest(IOwinContext context)
        {
            var queries = HttpUtility.ParseQueryString(context.Request.Uri.Query);

            var responseType = queries["response_type"];
            var redirectUri = queries["redirect_uri"];
            var scope = queries["openid"];
            var state = queries["state"];

            // Return response.

            throw new NotImplementedException();
        }
    }
}