﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GamerInfo.MVC.Startup))]
namespace GamerInfo.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
