---
name: "qx-sprite-domain-modeling"
description: "Provides domain modeling guidance for Qx.Sprite framework including Entity, Value Object, AggregateRoot, AuditedEntity patterns. Invoke when designing domain models using Qx.Sprite framework or working with DDD patterns like entities, value objects, aggregate roots."
---

# Qx.Sprite 领域模型设计指南

## 概述

Qx.Sprite 框架采用领域驱动设计（DDD）模式，提供了完整的领域模型基础设施，包括实体、值对象、聚合根、仓储等核心概念。

## 核心概念

### 1. 实体 (Entity)

实体是具有唯一标识的对象，即使属性相同，只要标识不同就是不同的实体。

#### 基础实体  

```csharp
using Qx.Sprite.Domain;

// 基础实体，使用 long 作为主键
public class Product : Entity<long>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

// 使用 Guid 作为主键
public class Category : Entity<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
}

// 使用 string 作为主键
public class Tag : Entity<string>
{
    public string Name { get; set; }
}
```

#### 实体相等性

框架自动实现了实体的相等性判断：
- 两个实体如果 Id 相同则相等
- 支持多租户场景下的实体区分
- 支持 `==` 和 `!=` 运算符

```csharp
var product1 = new Product { Id = 1, Name = "Product A" };
var product2 = new Product { Id = 1, Name = "Product B" };

// 返回 true，因为 Id 相同
bool areEqual = product1 == product2;
```

### 2. 审计实体

框架提供了多种审计实体，自动记录实体的创建、修改、删除信息。

#### 创建审计实体 (CreationAuditedEntity)

仅记录创建信息：

```csharp
using Qx.Sprite.Domain;

public class Product : CreationAuditedEntity<long>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

// 自动包含以下属性：
// - CreationTime: DateTime 创建时间
// - Creator: string? 创建人
```

#### 审计实体 (AuditedEntity)

记录创建和修改信息：

```csharp
using Qx.Sprite.Domain;

public class Order : AuditedEntity<long>
{
    public string OrderNo { get; set; }
    public decimal TotalAmount { get; set; }
}

// 自动包含以下属性：
// - CreationTime: DateTime 创建时间
// - Creator: string? 创建人
// - LastModificationTime: DateTime? 最后修改时间
// - LastModifier: string? 最后修改人
```

#### 完整审计实体 (FullAuditedEntity)

记录创建、修改、删除信息（推荐）：

```csharp
using Qx.Sprite.Domain;

public class Customer : FullAuditedEntity<long>
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}

// 自动包含以下属性：
// - CreationTime: DateTime 创建时间
// - Creator: string? 创建人
// - LastModificationTime: DateTime? 最后修改时间
// - LastModifier: string? 最后修改人
// - IsDeleted: bool 是否已删除
// - Deleter: string? 删除人
// - DeletionTime: DateTime? 删除时间
```

### 3. 聚合根 (Aggregate Root)

聚合根是领域模型的入口，封装了一组相关实体和值对象。

#### 基础聚合根

```csharp
using Qx.Sprite.Domain;

// 基础聚合根
public class Order : AggregateRoot<long>
{
    public string OrderNo { get; set; }
    public DateTime OrderTime { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    
    // 业务方法
    public void AddItem(OrderItem item)
    {
        Items.Add(item);
        RecalculateTotal();
    }
    
    public void RemoveItem(long itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            Items.Remove(item);
            RecalculateTotal();
        }
    }
    
    private void RecalculateTotal()
    {
        TotalAmount = Items.Sum(i => i.Quantity * i.UnitPrice);
    }
}
```

#### 审计聚合根

```csharp
using Qx.Sprite.Domain;

// 完整审计聚合根
public class Project : FullAuditedAggregateRoot<long>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ProjectStatus Status { get; set; }
    public List<Task> Tasks { get; set; } = new();
    
    public void Start()
    {
        if (Status != ProjectStatus.NotStarted)
            throw new BusinessException("项目已经开始或已完成");
            
        Status = ProjectStatus.InProgress;
    }
    
    public void Complete()
    {
        if (Status != ProjectStatus.InProgress)
            throw new BusinessException("项目未在进行中");
            
        if (Tasks.Any(t => !t.IsCompleted))
            throw new BusinessException("还有未完成的任务");
            
        Status = ProjectStatus.Completed;
    }
}
```

#### 聚合根特性

聚合根继承自 `AggregateRoot<TKey>`，具有以下特性：

1. **并发控制**: 包含 `ConcurrencyStamp` 属性用于乐观并发控制
2. **扩展数据**: 包含 `ExtraData` 字典用于存储动态属性

```csharp
// 使用扩展数据存储动态字段
var order = new Order();
order.ExtraData["ExternalOrderId"] = "EXT-12345";
order.ExtraData["Source"] = "Web";
```

### 4. 值对象 (Value Object)

值对象没有唯一标识，通过属性值来定义相等性。

```csharp
using Qx.Sprite.Domain;

// 地址值对象
public class Address : ValueObject
{
    public string Province { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public string Detail { get; set; }
    public string ZipCode { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Province;
        yield return City;
        yield return District;
        yield return Detail;
        yield return ZipCode;
    }
}

// 金额值对象
public class Money : ValueObject
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
    
    // 值对象可以包含业务逻辑
    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot add different currencies");
            
        return new Money { Amount = Amount + other.Amount, Currency = Currency };
    }
}
```

#### 在实体中使用值对象

```csharp
public class Customer : FullAuditedEntity<long>
{
    public string Name { get; set; }
    
    // 值对象作为复杂属性
    public Address HomeAddress { get; set; }
    public Address WorkAddress { get; set; }
    
    // 值对象集合
    public List<PhoneNumber> PhoneNumbers { get; set; } = new();
}
```

### 5. 多租户支持

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

### 6. JSON 字段

使用 `[JsonField]` 特性将复杂对象存储为 JSON：

```csharp
using Qx.Sprite.Core;
using Qx.Sprite.Domain;

public class Product : FullAuditedEntity<long>
{
    public string Name { get; set; }
    
    // 存储为 JSON 字符串
    [JsonField]
    public ProductMetadata Metadata { get; set; }
    
    // 存储为 JSON 数组
    [JsonField]
    public List<ProductAttribute> Attributes { get; set; } = new();
}

public class ProductMetadata
{
    public string Brand { get; set; }
    public string Manufacturer { get; set; }
    public DateTime ProductionDate { get; set; }
}

public class ProductAttribute
{
    public string Name { get; set; }
    public string Value { get; set; }
}
```

## 领域模型设计最佳实践

### 1. 实体设计原则

```csharp
// 好的设计：封装业务逻辑
public class Order : FullAuditedAggregateRoot<long>
{
    public string OrderNo { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public List<OrderItem> Items { get; private set; } = new();
    
    // 私有构造函数，强制通过工厂方法创建
    private Order() { }
    
    public static Order Create(string orderNo)
    {
        return new Order
        {
            OrderNo = orderNo,
            Status = OrderStatus.Pending,
            TotalAmount = 0
        };
    }
    
    // 业务方法封装状态变更
    public void AddItem(Product product, int quantity, decimal price)
    {
        if (Status != OrderStatus.Pending)
            throw new BusinessException("只能向待处理订单添加商品");
            
        var item = new OrderItem
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Quantity = quantity,
            UnitPrice = price
        };
        
        Items.Add(item);
        RecalculateTotal();
    }
    
    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new BusinessException("订单状态不正确");
            
        if (!Items.Any())
            throw new BusinessException("订单不能为空");
            
        Status = OrderStatus.Confirmed;
    }
    
    private void RecalculateTotal()
    {
        TotalAmount = Items.Sum(i => i.Quantity * i.UnitPrice);
    }
}
```

### 2. 领域事件（可选扩展）

```csharp
public class Order : FullAuditedAggregateRoot<long>
{
    // ... 其他属性
    
    private List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    public void Confirm()
    {
        // ... 验证逻辑
        
        Status = OrderStatus.Confirmed;
        
        // 添加领域事件
        _domainEvents.Add(new OrderConfirmedEvent(Id, OrderNo, TotalAmount));
    }
    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

// 领域事件
public class OrderConfirmedEvent : DomainEvent
{
    public long OrderId { get; }
    public string OrderNo { get; }
    public decimal TotalAmount { get; }
    
    public OrderConfirmedEvent(long orderId, string orderNo, decimal totalAmount)
    {
        OrderId = orderId;
        OrderNo = orderNo;
        TotalAmount = totalAmount;
    }
}
```

### 3. 领域服务

当业务逻辑不适合放在实体中时，使用领域服务。

## 完整示例

### 电商订单领域模型

```csharp
// ==================== 值对象 ====================
public class Address : ValueObject
{
    public string Province { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public string Detail { get; set; }
    public string ZipCode { get; set; }
    public string ContactName { get; set; }
    public string ContactPhone { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Province;
        yield return City;
        yield return District;
        yield return Detail;
        yield return ZipCode;
    }
}

public class Money : ValueObject
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "CNY";

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}

// ==================== 实体 ====================
public class OrderItem : Entity<long>
{
    public long ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => Quantity * UnitPrice;
}

// ==================== 聚合根 ====================
public class Order : FullAuditedAggregateRoot<long>, IMultiTenant
{
    // 基本信息
    public string OrderNo { get; private set; }
    public OrderStatus Status { get; private set; }
    public object? TenantId { get; set; }
    
    // 金额信息
    public decimal TotalAmount { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal PayableAmount => TotalAmount - DiscountAmount;
    
    // 配送信息
    public Address ShippingAddress { get; set; }
    public string? TrackingNumber { get; private set; }
    
    // 订单项
    public List<OrderItem> Items { get; private set; } = new();
    
    // 备注
    public string? CustomerRemark { get; set; }
    public string? CancellationReason { get; private set; }
    
    // 扩展数据
    [JsonField]
    public Dictionary<string, object> ExtraData { get; set; } = new();
    
    private Order() { }
    
    public static Order Create(string orderNo, Address shippingAddress)
    {
        return new Order
        {
            OrderNo = orderNo,
            Status = OrderStatus.Pending,
            ShippingAddress = shippingAddress,
            TotalAmount = 0,
            DiscountAmount = 0
        };
    }
    
    public void AddItem(long productId, string productName, string productImage, 
        int quantity, decimal unitPrice)
    {
        EnsureCanModify();
        
        var item = new OrderItem
        {
            ProductId = productId,
            ProductName = productName,
            ProductImage = productImage,
            Quantity = quantity,
            UnitPrice = unitPrice
        };
        
        Items.Add(item);
        RecalculateTotal();
    }
    
    public void RemoveItem(long itemId)
    {
        EnsureCanModify();
        
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            Items.Remove(item);
            RecalculateTotal();
        }
    }
    
    public void ApplyDiscount(decimal amount)
    {
        EnsureCanModify();
        
        if (amount > TotalAmount)
            throw new BusinessException("折扣金额不能大于订单金额");
            
        DiscountAmount = amount;
    }
    
    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new BusinessException("订单状态不正确");
            
        if (!Items.Any())
            throw new BusinessException("订单不能为空");
            
        Status = OrderStatus.Confirmed;
    }
    
    public void Pay(string paymentMethod, string transactionId)
    {
        if (Status != OrderStatus.Confirmed)
            throw new BusinessException("订单未确认，无法支付");
            
        Status = OrderStatus.Paid;
        
        ExtraData["PaymentMethod"] = paymentMethod;
        ExtraData["TransactionId"] = transactionId;
        ExtraData["PaymentTime"] = DateTime.UtcNow;
    }
    
    public void Ship(string trackingNumber)
    {
        if (Status != OrderStatus.Paid)
            throw new BusinessException("订单未支付，无法发货");
            
        Status = OrderStatus.Shipped;
        this.TrackingNumber = trackingNumber;
    }
    
    public void Complete()
    {
        if (Status != OrderStatus.Shipped)
            throw new BusinessException("订单未发货，无法完成");
            
        Status = OrderStatus.Completed;
    }
    
    public void Cancel(string reason)
    {
        if (Status != OrderStatus.Pending && Status != OrderStatus.Confirmed)
            throw new BusinessException($"当前状态 {Status} 不允许取消");
            
        Status = OrderStatus.Cancelled;
        CancellationReason = reason;
    }
    
    private void EnsureCanModify()
    {
        if (Status != OrderStatus.Pending)
            throw new BusinessException("只能修改待处理订单");
    }
    
    private void RecalculateTotal()
    {
        TotalAmount = Items.Sum(i => i.TotalPrice);
        
        // 确保折扣不超过新总额
        if (DiscountAmount > TotalAmount)
            DiscountAmount = TotalAmount;
    }
}

// ==================== 枚举 ====================
public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Paid = 2,
    Shipped = 3,
    Completed = 4,
    Cancelled = 5
}
```

## 选择实体类型的建议

| 场景 | 推荐类型 | 说明 |
|------|----------|------|
| 简单配置表 | `Entity<TKey>` | 无审计需求 |
| 基础业务实体 | `CreationAuditedEntity<TKey>` | 只需要记录创建信息 |
| 一般业务实体 | `AuditedEntity<TKey>` | 需要记录创建和修改 |
| 核心业务实体 | `FullAuditedEntity<TKey>` | 需要完整审计和软删除 |
| 复杂业务聚合 | `FullAuditedAggregateRoot<TKey>` | 需要并发控制和扩展数据 |
| 多租户场景 | 实现 `IMultiTenant` | 配合审计实体使用 |

## 相关链接

- [Qx.Sprite 框架开发指南](./qx-sprite-framework.md)
- [Qx.Sprite 工具类指南](./qx-sprite-utils.md)
