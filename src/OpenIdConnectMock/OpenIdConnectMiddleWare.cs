namespace OpenIdConnectMock
{
    using System;
    using System.Threading.Tasks;
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
            if (context.Request.Path.Value.Contains("openid-configuration"))
            {
                await HandleOpenIdConfigurationRequest(context);
            }

            if (context.Request.Path.Value.EndsWith("authorize"))
            {
                await HandleAuthorizeRequest(context);
            }

            await Next.Invoke(context);
        }

        private async Task HandleOpenIdConfigurationRequest(IOwinContext context)
        {
            throw new NotImplementedException();
        }

        private async Task HandleAuthorizeRequest(IOwinContext context)
        {
            throw new NotImplementedException();
        }
    }
}