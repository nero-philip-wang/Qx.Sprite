---
name: "qx-sprite-framework"
description: "Qx.Sprite是一个基于.NET的模块化Web开发框架，支持Pack插件机制、DDD领域驱动设计、EFCore仓储、JWT认证、自动依赖注入等。Invoke when developing business applications using Qx.Sprite framework or when user asks about how to use this framework."
---

# Qx.Sprite 框架使用指南

Qx.Sprite 是一个基于 .NET 的模块化 Web 开发框架，提供了完整的领域驱动设计（DDD）支持、Pack 插件机制、自动依赖注入、JWT/AppId 认证、缓存、定时任务等功能。

## 1. 应用程序入口

### Program.cs 基础结构

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

### appsettings.json 基础配置

```json
{
  "AspNetPack": {
    "Core": {
      "FilePrefix": ["YourProjectName"]
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

## 2. Pack 包模块机制

Pack 是 Qx.Sprite 的核心扩展机制，所有功能都通过 Pack 模块化加载。

### 创建自定义 Pack

```csharp
using Qx.Sprite.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;

public class MyCustomPack : AspNetPack
{
    public MyCustomPack(IConfiguration configuration, IHostEnvironment env) 
        : base(configuration, env)
    {
    }

    // 加载级别：Core(0), System(1), Application(2)
    public override PackLevel Level => PackLevel.Application;

    // 加载顺序，数字越小越先加载
    public override int Order => 10;

    // 是否启用
    public override bool IsEnabled => true;

    // 注册服务
    public override IServiceCollection AddServices(IServiceCollection services)
    {
        // 注册你的服务
        services.AddScoped<IMyService, MyService>();
        return base.AddServices(services);
    }

    // 应用中间件
    public override void UsePack(IApplicationBuilder app)
    {
        // 配置中间件
        base.UsePack(app);
    }
}
```

### Pack 加载级别

- **PackLevel.Core (0)**: 核心层，如 AutoDependencyInjectionPack
- **PackLevel.System (1)**: 系统层，如 JwtAuthenticationPack、EFCorePack
- **PackLevel.Application (2)**: 应用层，业务相关的 Pack

### 抽象 Pack（需要继承实现）

使用 `[TodoPack]` 特性标记抽象 Pack：

```csharp
[TodoPack]
public abstract class MyAbstractPack : AspNetPack
{
    public MyAbstractPack(IConfiguration configuration, IHostEnvironment env) 
        : base(configuration, env)
    {
    }

    // 子类必须实现抽象方法
    protected abstract void ConfigureSomething();
}
```

## 3. 领域驱动设计（DDD）

### 实体（Entity）

```csharp
using Qx.Sprite.Domain;

// 基础实体
public class Product : Entity<long>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

// 审计实体（自动记录创建信息）
public class Product : CreationAuditedEntity<long>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

// 完整审计实体（创建+修改）
public class Product : FullAuditedEntity<long>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

### 聚合根（AggregateRoot）

```csharp
using Qx.Sprite.Domain;

public class Order : AggregateRoot<long>
{
    public string OrderNo { get; set; }
    public decimal TotalAmount { get; set; }
    
    // 聚合根自动包含：
    // - Id: 主键
    // - ConcurrencyStamp: 并发戳（乐观锁）
    // - ExtraData: 扩展数据字典
}
```

### 值对象（ValueObject）

```csharp
using Qx.Sprite.Domain;

public class Address : ValueObject
{
    public string Province { get; set; }
    public string City { get; set; }
    public string Detail { get; set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Province;
        yield return City;
        yield return Detail;
    }
}
```

### 主键生成器

```csharp
using Qx.Sprite.Core;

// Guid 主键
[KeyGenerator(GuidKeyGeneratorType.SequentialAsBinary)]
public class Product : Entity<Guid>
{
    public string Name { get; set; }
}

// Long 主键（雪花算法）
[KeyGenerator(LongKeyGeneratorType.TwitterSnowFlake)]
public class Order : Entity<long>
{
    public string OrderNo { get; set; }
}

// String 主键
[KeyGenerator(StringKeyGeneratorType.TwitterSnowFlake)]
public class Category : Entity<string>
{
    public string Name { get; set; }
}
```

### 多租户支持

实现 `IMultiTenant` 接口以支持多租户：

```csharp
using Qx.Sprite.Core;
using Qx.Sprite.Domain;

public class Product : FullAuditedEntity<long>, IMultiTenant
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    
    // 多租户标识
    public object? TenantId { get; set; }
}
```

框架会自动：
- 在查询时过滤当前租户的数据
- 在保存时自动设置当前租户标识

### 选择实体类型的建议

| 场景 | 推荐类型 | 说明 |
|------|----------|------|
| 简单配置表 | `Entity<TKey>` | 无审计需求 |
| 基础业务实体 | `CreationAuditedEntity<TKey>` | 只需要记录创建信息 |
| 一般业务实体 | `AuditedEntity<TKey>` | 需要记录创建和修改 |
| 核心业务实体 | `FullAuditedEntity<TKey>` | 需要完整审计和软删除 |
| 复杂业务聚合 | `FullAuditedAggregateRoot<TKey>` | 需要并发控制和扩展数据 |
| 多租户场景 | 实现 `IMultiTenant` | 配合审计实体使用 |

## 相关链接
- [Qx.Sprite 领域模型设计指南](../qx-sprite-domain-modeling/skill.md)

## 4. 仓储使用（Repository）

### 定义仓储接口

```csharp
using Qx.Sprite.Core;

public interface IProductRepository : IRepository<long, Product>
{
    // 自定义查询方法
    Task<Product?> GetByNameAsync(string name);
    Task<List<Product>> GetByPriceRangeAsync(decimal min, decimal max);
}
```

### 实现仓储

```csharp
using Qx.Sprite.Domain;
using Qx.Sprite.EFCore;
using Microsoft.EntityFrameworkCore;

public class ProductRepository : EfRepository<long, Product>, IProductRepository
{
    public ProductRepository(
        IServiceProvider provider, 
        IMapper mapper,
        IMultiTenant tenant,
        IUserInfo user) 
        : base(provider, mapper, tenant, user)
    {
    }

    public async Task<Product?> GetByNameAsync(string name)
    {
        return await this.GetQuerySet()
            .FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<List<Product>> GetByPriceRangeAsync(decimal min, decimal max)
    {
        return await this.GetQuerySet()
            .Where(p => p.Price >= min && p.Price <= max)
            .ToListAsync();
    }
}
```

### 在控制器中使用仓储
> 建议使用仓储可以更方便地进行数据库操作,而不是直接使用DbContext。仓储提供了在更新时自动记录审计信息的能力。

```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{id}")]
    public async Task<ProductDto> Get(long id)
    {
        var product = _repository.Get(id);
        return _mapper.Map<ProductDto>(product);
    }

    [HttpGet]
    public async Task<CountableList<ProductListDto>> GetList([FromQuery] SearchArgs args)
    {
        return _repository.GetList<ProductListDto>(args);
    }

    [HttpPost]
    public async Task<ProductDto> Create([FromBody] CreateProductInput input)
    {
        var entity = _mapper.Map<Product>(input);
        entity = _repository.Add(entity, true);
        return _mapper.Map<ProductDto>(entity);
    }

    [HttpPut("{id}")]
    public async Task<ProductDto> Update(long id, [FromBody] UpdateProductInput input)
    {
        var entity = _repository.Get(id);
        _mapper.Map(input, entity);
        entity = _repository.Update(entity, true);
        return _mapper.Map<ProductDto>(entity);
    }

    [HttpDelete("{id}")]
    public async Task Delete(long id)
    {
        _repository.Delete(id, true);
    }
}
```

## 5. 自动依赖注入

### 标记接口方式

```csharp
using Qx.Sprite.Core;

// 瞬时生命周期
public interface IMyTransientService : ITransient
{
    void DoSomething();
}

// 作用域生命周期
public interface IMyScopedService : IScoped
{
    void DoSomething();
}

// 单例生命周期
public interface IMySingletonService : ISingleton
{
    void DoSomething();
}
```

### 特性标记方式（指定接口）

```csharp
using Qx.Sprite.Core;

[ServiceType(typeof(IProductService))]
public class ProductService : IProductService
{
    // 自动注册为 IProductService -> ProductService
}

// 支持多个接口
[ServiceType(typeof(IProductService))]
[ServiceType(typeof(IProductQueryService))]
public class ProductService : IProductService, IProductQueryService
{
}

// 带Key的注册
[ServiceType(typeof(IProductService), Key = "default")]
[ServiceType(typeof(IProductService), Key = "v2")]
public class ProductService : IProductService
{
}
```

## 6. 基础控制器和自动 CRUD

### 基础控制器

```csharp
using Qx.Sprite.Mvc;

[ApiController]
[Route("api/v{version:apiVersion}/[area]/[controller]/")]
[ApiVersion("1.0")]
[Area("default")]
public abstract class ApiBaseController : ControllerBase
{
}

// 使用
public class ProductsController : ApiBaseController
{
}
```

### 自动 CRUD 控制器
> 自动 CRUD 控制器采用 Restful API规范。
```csharp
using Qx.Sprite.Mvc;

public class ProductsController : AutoCurdService<
    IProductRepository,      // 仓储类型
    Product,                 // 实体类型
    ProductDto,              // 详情DTO
    ProductListDto,          // 列表DTO
    long,                    // 主键类型
    CreateProductInput,      // 创建输入
    UpdateProductInput>      // 更新输入
{
    public ProductsController(
        IProductRepository repository, 
        IMapper mapper) 
        : base(repository, mapper)
    {
    }

    // 自动提供以下端点：
    // GET api/v1/default/products/{id} - 获取详情
    // GET api/v1/default/products - 获取列表
    // POST api/v1/default/products - 创建
    // PUT api/v1/default/products/{id} - 更新
    // DELETE api/v1/default/products/{id} - 删除
}
```

## 7. MVC输出规则

### 正常输出
> 正常输出包括单个对象和列表，系统会MVC层会自动添加ResponseMessage对象作为封装，如不想被封装，需要在方法上添加 `[RawResult]` 特性。
```csharp
// 单个对象 - 直接返回
[HttpGet("{id}")]
public async Task<ProductDto> Get(long id)
{
    var product = _repository.Get(id);
    return _mapper.Map<ProductDto>(product);
}

// 列表 - 使用 CountableList
[HttpGet]
public async Task<CountableList<ProductListDto>> GetList([FromQuery] QueryArgs args)
{
    return _repository.GetList<ProductListDto>(args);
}

public class QueryArgs : SortingArgs
{
    public string? OrderId { get; set; }
    public string? Name { get; set; }
}
```

### SortingArgs 参数说明

`SortingArgs` 包含以下字段：

| 字段 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| OrderBy | string | "id desc" | 排序字段，继承自 SortingArgs |
| Skip | int | 0 | 跳过多少条数据，继承自 PagingArgs |
| Take | int | 20 | 取多少条数据，继承自 PagingArgs |
| NeedTotal | bool | false | 是否需要提供总数，仅当 Skip=0 时有效 |

### 错误处理

```csharp
using Qx.Sprite.Core;

// 抛出业务异常
[HttpPost]
public async Task<ProductDto> Create([FromBody] CreateProductInput input)
{
    if (input.Price < 0)
    {
        throw new BusinessException("价格不能为负数");
    }
    
    // 404 错误
    var product = _repository.Get(id); // 找不到会自动抛出 404
    
    // 自定义状态码
    throw new BusinessException("权限不足", HttpStatusCode.Forbidden);
    throw new BusinessException("服务器错误", HttpStatusCode.InternalServerError);
}
```

## 8. Utils 扩展方法和 JSON 序列化

### 字符串扩展
#### 正则表达式相关
```csharp
using Qx.Sprite.Core;

// 正则匹配
bool isMatch = "test@email.com".IsMatch(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");

// 常用验证
bool isEmail = "test@email.com".IsEmail();
bool isIp = "192.168.1.1".IsIpAddress();
bool isNumeric = "12345".IsNumeric();
bool isUrl = "https://example.com".IsUrl();
bool isIdCard = "110101199001011234".IsIdentityCardId();
bool isMobile = "13800138000".IsMobileNumber();
bool isMobileStrict = "13800138000".IsMobileNumber(isRestrict: true);

// 提取数字
string firstNum = "abc123def456".MatchFirstNumber();  // "123"
string lastNum = "abc123def456".MatchLastNumber();    // "456"
var allNums = "abc123def456".MatchNumbers();          // ["123", "456"]

// 截取字符串
string result = "hello[start]content[end]world".Substring("[start]", "[end]");
// 结果: "content"
```

#### 编码/解码
```csharp
// Base64
string base64 = "hello".ToBase64String();
string original = base64.FromBase64String();

// URL 编码/解码
string encoded = "hello world".ToUrlEncode();
string decoded = encoded.ToUrlDecode();

// HTML 编码/解码
string htmlEncoded = "<div>test</div>".ToHtmlEncode();
string htmlDecoded = htmlEncoded.ToHtmlDecode();

// 十六进制
string hex = "hello".ToHexString();
byte[] bytes = hex.ToHexBytes();

// Unicode
string unicode = "编码".ToUnicodeString();  // "\u7f16\u7801"
string decoded = unicode.FromUnicodeString();
```

#### 字符串处理
```csharp
// 空检查
bool isEmpty = "".IsNullOrWhiteSpace();
bool isMissing = "".IsMissing();

// 格式化
string formatted = "Hello, {0}".FormatWith("World");

// 反转
string reversed = "hello".ReverseString();  // "olleh"

// 大小写转换
string lowerFirst = "Hello".LowerFirstChar();  // "hello"
string upperFirst = "hello".UpperFirstChar();  // "Hello"

// 复数/单数转换
string plural = "category".ToPlural();     // "categories"
string singular = "categories".ToSingular(); // "category"

// 驼峰转连字符
string kebab = "CamelCaseString".UpperToLowerAndSplit();  // "camel-case-string"

// 相似度计算
int distance = "hello".LevenshteinDistance("hallo", out double similarity);
double sim = "hello".GetSimilarityWith("hallo");

// MD5
string md5 = "hello".ToMd5Hash();

// 文本长度（汉字算2个字符）
int len = "中文abc".TextLength();  // 7
```

#### URL 操作
```csharp
// 添加查询参数
string url = "https://api.example.com/users".AddUrlQuery("page=1", "size=10");
// 结果: https://api.example.com/users?page=1&size=10

// 获取查询参数
string page = url.GetUrlQuery("page");  // "1"

// 添加 Hash
string urlWithHash = "https://example.com".AddHashFragment("section1");
// 结果: https://example.com#section1
```

#### 集合转字符串
```csharp
var list = new List<int> { 1, 2, 3 };
string str = list.Join();           // "1,2,3"
string str2 = list.Join("-");       // "1-2-3"

// 自定义转换
string str3 = list.Join(x => $"[{x}]");  // "[1],[2],[3]"
```

### 对象扩展

```csharp
using Qx.Sprite.Core;

// 类型转换
int num = "123".As<int>();           // 123
MyEnum enum = "Value1".As<MyEnum>(); // MyEnum.Value1
Guid guid = "...".As<Guid>();

// 默认值转换
int num = "invalid".As<int>(0);      // 0

// 空检查
object obj = null;
obj.CheckNotNull("paramName");       // 抛出 ArgumentNullException

// 是否为空
bool isEmpty1 = ((string)null).IsNullOrEmpty();
bool isEmpty2 = "".IsNullOrEmpty();
bool isEmpty3 = new int[0].IsNullOrEmpty();

// 非空检查
bool notNull = obj.IsNotNull();

// 转 JSON 字符串
string json = myObject.ToJsonString();

// 深拷贝 (已过时，建议使用其他方式)
var clone = myObject.DeepClone();
```

### DateTime 扩展

```csharp
using Qx.Sprite.Core;

// 转时间戳
long timestamp = DateTime.Now.Timestamp();

// 时间戳转 DateTime
DateTime dt = DateTimeExtensions.ParseFromTimestamp(1609459200);
```

### Random 扩展

```csharp
using Qx.Sprite.Core;

var random = new Random();

// 随机布尔
bool flag = random.NextBoolean();

// 随机枚举
MyEnum value = random.NextEnum<MyEnum>();

// 随机字节数组
byte[] bytes = random.NextBytes(16);

// 从数组取随机项
int item = random.NextItem(new[] { 1, 2, 3, 4, 5 });

// 随机时间
DateTime randomDate = random.NextDateTime();
DateTime randomDate2 = random.NextDateTime(DateTime.Now, DateTime.Now.AddYears(1));

// 随机字符串
string numbers = random.NextNumberString(6);           // 6位数字
string letters = random.NextLetterString(6);           // 6位字母
string mixed = random.NextLetterAndNumberString(6);    // 6位字母数字混合

// 生成中文姓名
string name = random.NextChineseName();                // 如: "张伟"

// 生成手机号
string phone = random.NextTel();                       // 如: "13800138000"

// 生成身份证号
string idCard = random.NextIdCard();                   // 18位身份证号

// 生成民族
string nation = random.NextNation();                   // 如: "汉族"

// 生成地址
string address = random.NextAddress();                 // 如: "北京市朝阳区..."

// 生成邮箱
string email = random.NextEmail();                     // 如: "abc@example.com"

// 生成随机列表
List<int> list = random.NextList(() => random.Next(100), 10);
```
### 特性查找扩展

```csharp
using Qx.Sprite.Core;
using System.Reflection;

PropertyInfo prop = typeof(MyClass).GetProperty("MyProperty");

// 检查是否有特性
bool hasAttr = prop.HasAttribute<RequiredAttribute>();

// 获取特性
var attrs = prop.GetAttribute<DisplayAttribute>();
```

### 配置扩展

```csharp
using Qx.Sprite.Core;
using Microsoft.Extensions.Configuration;

IConfiguration config = ...;

// 强类型获取配置
var settings = config.Get<MySettings>("SectionName");
```

### JSON 序列化

```csharp
using Qx.Sprite.Core;

// 使用框架默认配置序列化
var json = JsonSerializer.Serialize(obj, JsonUtils.JsonOptions);
var obj = JsonSerializer.Deserialize<MyClass>(json, JsonUtils.JsonOptions);

// 配置选项说明：
// - 小驼峰命名
// - 枚举转为字符串
// - Long 转为字符串（防止精度丢失）
// - 忽略循环引用
// - 支持 ExtraData 扩展字段
```

### ExtraData 扩展字段

```csharp
public class User : IHasExtraData
{
    public long Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object?> ExtraData { get; set; } = new();
}

// 使用
var user = new User { Id = 1, Name = "张三" };
user.SetProperty("age", 25);
user.SetProperty("city", "北京");

var json = JsonSerializer.Serialize(user, JsonUtils.JsonOptions);
// {"id":1,"name":"张三","age":25,"city":"北京"}

// 反序列化
var user2 = JsonSerializer.Deserialize<User>(json, JsonUtils.JsonOptions);
var age = user2.GetProperty("age");  // 25
```

## 9. AutoMapper 配置

### 创建映射配置

```csharp
using Qx.Sprite.ObjectMapper;
using AutoMapper;

public class ProductMapperConfiguration : IMapperConfiguration
{
    public void AddAutoMapper(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Product, ProductDto>();
        cfg.CreateMap<Product, ProductListDto>();
        cfg.CreateMap<CreateProductInput, Product>();
        cfg.CreateMap<UpdateProductInput, Product>();
    }
}
```

### 使用映射

```csharp
public class ProductService
{
    private readonly IMapper _mapper;

    public ProductService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public ProductDto MapToDto(Product entity)
    {
        return _mapper.Map<ProductDto>(entity);
    }

    public void UpdateEntity(UpdateProductInput input, Product entity)
    {
        _mapper.Map(input, entity); // 更新现有对象
    }
}
```

## 10. JWT / AppId 认证

### JWT 认证

```csharp
// 创建 Token
[HttpGet("token")]
public string GetToken()
{
    return new JwtUser<long, long>()
    {
        Id = 1,
        Title = "test",
        ExtraData = new Dictionary<string, object?> 
        { 
            { "Name", "test" }
        }
    }.CreateToken(DateTime.Now.AddDays(30), "all");
}

// 保护 API
[Authorize("jwt")]
[HttpGet("protected")]
public string ProtectedEndpoint()
{
    return "This is protected";
}

// 允许匿名
[Authorize("jwt")]
[AllowAnonymous]
[HttpGet("public")]
public string PublicEndpoint()
{
    return "This is public";
}
```

### AppId 认证（服务间调用）

```csharp
// 使用 AppId 保护
[Authorize("appid")]
[HttpGet("service-api")]
public string ServiceApi()
{
    return "Service API";
}
```

## 11. 缓存使用

### 基础缓存

```csharp
// 继承 EasyCachingPack
public class MyCachePack : EasyCachingPack
{
    public MyCachePack(IConfiguration configuration, IHostEnvironment env) 
        : base(configuration, env)
    {
    }
}

// 配置 appsettings.json
{
  "Cache": {
    "CacheType": "InMemory",
    "Redis": {
      "ConnectionString": "localhost:6379",
      "Database": 0
    }
  }
}
```

### 缓存拦截器

```csharp
using Qx.Sprite.Caching;

// 标记接口使用缓存拦截
[EasyCachingMark]
public interface IProductService
{
    // 方法返回值会被自动缓存
    Task<ProductDto> GetByIdAsync(long id);
    
    Task<List<ProductDto>> GetListAsync();
}

public class ProductService : IProductService
{
    public async Task<ProductDto> GetByIdAsync(long id)
    {
        // 结果被缓存，下次调用直接返回缓存值
        return await _repository.GetAsync(id);
    }
}
```

### 直接使用缓存 Provider

```csharp
using EasyCaching.Core;

public class ProductService
{
    private readonly IEasyCachingProvider _cache;

    public ProductService(IEasyCachingProvider cache)
    {
        _cache = cache;
    }

    public async Task<ProductDto> GetWithCache(long id)
    {
        var cacheKey = $"product:{id}";
        
        var cached = await _cache.GetAsync<ProductDto>(cacheKey);
        if (cached.Value != null)
        {
            return cached.Value;
        }
        
        var product = await _repository.GetAsync(id);
        await _cache.SetAsync(cacheKey, product, TimeSpan.FromMinutes(10));
        
        return product;
    }
}
```

## 12. Hangfire 定时任务

### 配置 Hangfire

```csharp
using Qx.Sprite.BackgroundJob;

// 继承 HangfirePack 并实现数据库配置
public class MyHangfirePack : HangfirePack
{
    public MyHangfirePack(IConfiguration configuration, IHostEnvironment env) 
        : base(configuration, env)
    {
    }

    protected override IGlobalConfiguration UseDb(IGlobalConfiguration configuration, string connectionString)
    {
        return configuration.UsePostgreSqlStorage(connectionString);
        // 或使用 SQL Server
        // return configuration.UseSqlServerStorage(connectionString);
    }
}
```

### 创建定时任务

```csharp
using Qx.Sprite.BackgroundJob;

[ServiceType(typeof(IRecurringJob))]
[ServiceType(typeof(DataSyncJob))]
public class DataSyncJob : IRecurringJob, ISingleton
{
    public string Title => "数据同步任务";
    
    public string CronExpression { get; set; } = "0 0 * * *"; // 每天零点

    public async Task Start()
    {
        // 执行定时任务逻辑
        await SyncDataAsync();
    }

    private async Task SyncDataAsync()
    {
        // 同步逻辑
    }
}
```

### 任务配置 JSON

```json
{
  "RecurringJobs": {
    "数据同步任务": {
      "CronExpression": "0 0 * * *"
    }
  }
}
```

## 13. CAP 消息队列

### 配置 CAP

```csharp
using Qx.Sprite.Cap;

public class MyCapPack : CapPack
{
    public MyCapPack(IConfiguration configuration, IHostEnvironment env) 
        : base(configuration, env)
    {
    }

    protected override void UseMessageQueue(CapOptions x, IConfiguration configuration)
    {
        // 使用 RabbitMQ
        x.UseRabbitMQ(opt =>
        {
            opt.HostName = configuration["RabbitMQ:Host"]!;
            opt.UserName = configuration["RabbitMQ:UserName"]!;
            opt.Password = configuration["RabbitMQ:Password"]!;
        });
        
        // 或使用 Kafka
        // x.UseKafka(configuration["Kafka:Servers"]!);
    }

    protected override CapOptions UseDb(CapOptions options, IConfiguration configuration)
    {
        // 使用 PostgreSQL 作为存储
        options.UsePostgreSql(configuration.GetConnectionString("Default")!);
        return options;
    }
}
```

### 发布消息

```csharp
using DotNetCore.CAP;

public class OrderService
{
    private readonly ICapPublisher _capBus;

    public OrderService(ICapPublisher capBus)
    {
        _capBus = capBus;
    }

    public async Task CreateOrderAsync(CreateOrderInput input)
    {
        // 创建订单...
        
        // 发布消息
        await _capBus.PublishAsync("order.created", new 
        {
            OrderId = order.Id,
            OrderNo = order.OrderNo,
            TotalAmount = order.TotalAmount
        });
    }
}
```

### 订阅消息

```csharp
using DotNetCore.CAP;

public class OrderEventHandler
{
    [CapSubscribe("order.created")]
    public async Task HandleOrderCreated(OrderCreatedEvent @event)
    {
        // 处理订单创建事件
        await SendNotificationAsync(@event);
    }

    [CapSubscribe("order.paid")]
    public async Task HandleOrderPaid(OrderPaidEvent @event)
    {
        // 处理订单支付事件
        await UpdateInventoryAsync(@event);
    }
}
```

## 14. Swagger 和 Example 值

### 配置 Swagger

```csharp
// 继承 SwaggerPack
public class MySwaggerPack : SwaggerPack
{
    public MySwaggerPack(IConfiguration configuration, IHostEnvironment env) 
        : base(configuration, env)
    {
    }
}

// appsettings.json
{
  "AspNetPack": {
    "Swagger": {
      "Url": "/swagger/v1/swagger.json",
      "Title": "My API"
    }
  }
}
```

### 为 DTO 添加 Example

```csharp
public class ProductDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    // 静态 Example 方法用于 Swagger 示例
    public static ProductDto Example(Random random, ProductDto? example)
    {
        if (example != null) return example;
        
        return new ProductDto
        {
            Id = random.NextInt64(1, 10000),
            Name = random.NextFullName(),
            Price = random.Next(100, 10000)
        };
    }
}
```

### 使用 Random 扩展生成示例数据

```csharp
public class UserDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }

    public static UserDto Example(Random random, UserDto? example) => 
        example ?? new UserDto
        {
            Id = random.NextInt64(1, 10000),
            Name = random.NextFullName(),
            PhoneNumber = random.NextPhoneNumber(),
            Email = random.NextEmail(),
            Address = random.NextAddress()
        };
}
```

## 完整项目结构示例

```
MyProject/
├── MyProject.Web/
│   ├── Program.cs
│   ├── appsettings.json
│   ├── Controllers/
│   │   └── ProductsController.cs
│   └── Packs/
│       ├── MyDbContextPack.cs
│       ├── MyJwtPack.cs
│       └── MySwaggerPack.cs
├── MyProject.Application/
│   ├── Services/
│   │   └── ProductService.cs
│   └── DTOs/
│       ├── ProductDto.cs
│       ├── CreateProductInput.cs
│       └── UpdateProductInput.cs
├── MyProject.Domain/
│   ├── Entities/
│   │   └── Product.cs
│   └── Repositories/
│       └── IProductRepository.cs
└── MyProject.Infrastructure/
    ├── Repositories/
    │   └── ProductRepository.cs
    └── DbContext/
        └── AppDbContext.cs
```
