// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Mvc
{
    using System;
    using System.Reflection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.AspNetCore.SignalR;
    using Qx.Sprite.Core;

    /// <summary>
    /// 映射所有Hub
    /// </summary>
    public static class MapAllHubsExtensions
    {
        /// <summary>
        /// 映射所有Hub
        /// </summary>
        /// <param name="routes"></param>
        /// <returns></returns>
        /// <exception cref="Exception"> Exception </exception>
        public static IEndpointRouteBuilder MapHubs(this IEndpointRouteBuilder routes)
        {
            var mapHub = typeof(HubEndpointRouteBuilderExtensions)
                .GetMethod(nameof(HubEndpointRouteBuilderExtensions.MapHub), new[] { typeof(IEndpointRouteBuilder), typeof(string) })
                ?? throw new Exception("Could not find MapHub method");

            var finder = new AppDomainAssemblyFinder();
            var hubs = finder.FindTypesInheritFrom<Hub>();
            foreach (var hub in hubs)
            {
                mapHub.MakeGenericMethod(hub).Invoke(null, new object[] { routes, hub.Name });
            }

            return routes;
        }
    }
}