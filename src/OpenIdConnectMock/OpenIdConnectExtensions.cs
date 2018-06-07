namespace OpenIdConnectMock
{
    using System;
    using Owin;

    public static class OpenIdConnectExtensions
    {
        public static IAppBuilder UseOpenIdConnectMock(this IAppBuilder app, Uri baseUri)
        {
            app.Use(typeof(OpenIdConnectMiddleWare), baseUri);
            return app;
        }
    }
}