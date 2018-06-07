namespace OpenIdConnectMock
{
    using System;
    using Owin;

    public static class OpenIdConnectExtensions
    {
        public static IAppBuilder UseOpenIdConnectMock(this IAppBuilder app, Uri baseUri, string tenant, string defaultPolicy)
        {
            app.Use(typeof(OpenIdConnectMiddleWare), baseUri, tenant, defaultPolicy);
            return app;
        }
    }
}