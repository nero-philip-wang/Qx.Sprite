# Qx.Sprite

基于 .NET 10.0 的模块化 Web 开发框架，提供 Pack 插件机制、DDD 领域驱动设计、仓储模式、JWT/AppId 认证、自动依赖注入等功能。

## 功能特性

- **Pack 插件机制**: 核心扩展机制，所有功能通过 Pack 模块化加载
- **领域驱动设计 (DDD)**: 实体、聚合根、值对象、多租户支持
- **EFCore 仓储**: 自动 CRUD、审计信息记录、软删除
- **认证授权**: JWT 认证、AppId 服务间调用认证
- **自动依赖注入**: 通过接口标记或特性自动注册服务
- **缓存**: EasyCaching 支持（内存/Redis）
- **定时任务**: Hangfire 定时任务调度
- **消息队列**: CAP 消息总线（RabbitMQ/Kafka）
- **日志**: Serilog + Log4Net 支持
- **API 文档**: Swagger 集成

## 模块列表

| 模块 | 说明 |
|------|------|
| `Qx.Sprite.Core` | 核心基础（Pack 基类、扩展方法、特性） |
| `Qx.Sprite.DDD` | 领域驱动设计（实体、聚合根、值对象） |
| `Qx.Sprite.EFCore` | EFCore 仓储实现 |
| `Qx.Sprite.Mvc` | MVC 基础类、自动 CRUD 控制器 |
| `Qx.Sprite.JwtAuth` | JWT 认证 |
| `Qx.Sprite.AppIdAuth` | AppId 服务认证 |
| `Qx.Sprite.MemoryCache` | 内存缓存 |
| `Qx.Sprite.HangfireJob` | Hangfire 定时任务 |
| `Qx.Sprite.Cap` | CAP 消息队列 |
| `Qx.Sprite.Swagger` | Swagger 文档 |
| `Qx.Sprite.Utils` | 工具库（扩展方法、JSON 处理） |
| `Qx.Sprite.Permission` | 权限管理 |
| `Qx.Sprite.TenantManager` | 多租户管理 |
| `Qx.Sprite.OAuth` | OAuth 第三方登录 |
| `Qx.Sprite.EnumToDictionary` | 枚举转字典 |

## 快速开始

### 1. 创建 Program.cs

```csharp
using Qx.Sprite.Core;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // 添加 Pack 模块服务
        builder.Host.AddPack();

        var app = builder.Build();

        // 使用 Pack 模块
        app.UsePack();

        app.Run();
    }
}
```

### 2. 配置 appsettings.json

```json
{
  "AspNetPack": {
    "Core": {
      "FilePrefix": ["YourProjectNameSpacePrefix"]
    },
    "ServeInfo": {
      "ServeBaseUrl": "http://localhost:5000",
      "Version": "1.0.0",
      "Title": "API应用服务器"
    },
    "CorsHosts": ["http://localhost:8080"]
  }
}
```

### 3. 创建自定义 Pack

```csharp
using Qx.Sprite.Core;

public class MyCustomPack : AspNetPack
{
    public MyCustomPack(IConfiguration configuration, IHostEnvironment env)
        : base(configuration, env)
    {
    }

    public override PackLevel Level => PackLevel.Application;
    public override int Order => 10;

    public override IServiceCollection AddServices(IServiceCollection services)
    {
        // 注册服务
        return base.AddServices(services);
    }

    public override void UsePack(IApplicationBuilder app)
    {
        base.UsePack(app);
    }
}
```

### 4. 创建实体

```csharp
using Qx.Sprite.Domain;

public class Product : FullAuditedEntity<long>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

### 5. 创建自动 CRUD 控制器

```csharp
using Qx.Sprite.Mvc;

public class ProductsController : AutoCurdService<
    IProductRepository,
    Product,
    ProductDto,
    ProductListDto,
    long,
    CreateProductInput,
    UpdateProductInput>
{
    public ProductsController(IProductRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }
}
```

## 项目结构

```
Qx.Sprite/
├── Src/
│   ├── Application/           # 应用层
│   │   ├── Qx.Sprite.EnumToDictionary/
│   │   ├── Qx.Sprite.Permission/
│   │   ├── Qx.Sprite.TenantManager/
│   │   └── Qx.Sprite.OAuth/
│   ├── Core/                  # 核心层
│   │   ├── Qx.Sprite.Core/
│   │   ├── Qx.Sprite.Utils/
│   │   ├── Qx.Sprite.Serilog/
│   │   └── Qx.Sprite.Log4Net/
│   └── System/                # 系统层
│       ├── Qx.Sprite.DDD/
│       ├── Qx.Sprite.EFCore/
│       ├── Qx.Sprite.Mvc/
│       ├── Qx.Sprite.JwtAuth/
│       ├── Qx.Sprite.AppIdAuth/
│       ├── Qx.Sprite.MemoryCache/
│       ├── Qx.Sprite.HangfireJob/
│       ├── Qx.Sprite.Cap/
│       ├── Qx.Sprite.Swagger/
│       └── Qx.Sprite.ObjectMapper/
└── Test/                      # 测试项目
```

## 版本要求

- .NET 10.0 或更高版本

## 相关文档

- [Qx.Sprite 框架使用指南](Docs/qx-sprite-framework/skill.md)
- [领域模型设计指南](Docs/qx-sprite-domain-modeling/skill.md)

## License

MIT License