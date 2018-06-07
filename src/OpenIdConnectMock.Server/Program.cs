namespace OpenIdConnectMock.Server
{
    using System;
    using Microsoft.Owin.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("OpenId Connect server has started..");
                Console.WriteLine("Press [Enter] to shut down.");
                Console.ReadLine();
            }
        }
    }
}