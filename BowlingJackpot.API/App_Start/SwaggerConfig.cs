using System.Web.Http;
using WebActivatorEx;
using BowlingJackpot.API;
using Swashbuckle.Application;
using BowlingJackpotModel;
using Swashbuckle.Swagger;
using System.Collections.Generic;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace BowlingJackpot.API
{
    public class SwaggerConfig
    {
        public static void Register()
        {
#if DEBUG
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "BowlingJackpot.API");
                        c.IncludeXmlComments(GetXmlCommentsPath());
                    })
                .EnableSwaggerUi();
#endif
        }

        private static string GetXmlCommentsPath()
        {
            return System.String.Format(@"{0}\content\xml\BowlingJackpot.API.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}