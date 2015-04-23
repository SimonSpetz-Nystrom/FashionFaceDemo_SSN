﻿using FashionFaceMVC.Hubs;
using Owin;
using Microsoft.Owin;
[assembly: OwinStartup(typeof(Hubs.Startup))]
namespace Hubs
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }

}