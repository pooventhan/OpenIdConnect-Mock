namespace OpenIdConnectMock.Server
{
    using System;
    using Owin;

    public sealed class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.UseOpenIdConnectMock(new Uri("http://localhost:9000/"), "tenantname.onmicrosoft.com", "B2C_1A_SigninAdOnly");
        }
    }
}