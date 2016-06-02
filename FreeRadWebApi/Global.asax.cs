using FreeRadWebApi.Models;
using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;


namespace FreeRadWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Unity.MV5 DI
            UnityConfig.RegisterComponents();

            // All the dependency resolvers for MySql classes
            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());

            GlobalConfiguration.Configure(WebApiConfig.Register);
            
        }
    }
}
