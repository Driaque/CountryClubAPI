using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using CountryClubAPI.Models;

namespace CountryClubAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<User>("Users");
            builder.EntitySet<Comment>("Comments");
            builder.EntitySet<Family>("Families");
            builder.EntitySet<Friend>("Friends");
            builder.EntitySet<Message>("Messages");
            builder.EntitySet<Post>("Posts");
            builder.EntitySet<Interest>("Interests");
            builder.EntitySet<PostLikedbyUser>("PostLikedbyUsers");
            builder.EntitySet<User_Has_Interest>("User_Has_Interest");
            builder.EntitySet<Post_has_Comment>("Post_has_Comment");
            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());

            //CORS
           // var cors = new EnableCorsAttribute("http://localhost:46204", "*", "*");
            var cors = new EnableCorsAttribute(
                            "http://localhost:46204",
                            "*",
                            "*",
                            "DataServiceVersion, MaxDataServiceVersion"
                        );
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
