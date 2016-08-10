using Owin;
using System.Web.Http;

namespace Sean.WorldServer
{
    public class WebHostStartup
    {
        // This method is required by Katana:
        public void Configuration(IAppBuilder app)
        {
            var webApiConfiguration = ConfigureWebApi();

            // Use the extension method provided by the WebApi.Owin library:
            app.UseWebApi(webApiConfiguration);
        }


        private HttpConfiguration ConfigureWebApi()
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute (
                "DefaultApi",
                "api/{controller}");//,
                //new { playerId = RouteParameter.Optional });
            return config;
        }
    }
}