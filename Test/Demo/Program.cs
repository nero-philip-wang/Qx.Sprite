// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------
namespace Demo
{
    using Qx.Sprite.Core;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            AspNetPackConfig.InitAll();
            builder.Host.AddPack();
            var app = builder.Build();
            app.UsePack();
            app.Run();
        }
    }
}