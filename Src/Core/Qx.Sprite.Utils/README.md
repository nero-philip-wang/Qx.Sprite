# Qx.Sprite.Utils 项目说明

本项目是一个基于 .NET 8.0 的工具库，提供了一系列扩展方法和 JSON 处理工具。以下是项目中所有 public class 的功能总结：

## 扩展方法类 (Extensions)

### 1. AttributeFinder (静态类)
**功能**: 检查和获取 MemberInfo 上的特性
- `HasAttribute<T>()`: 检查某个 MemberInfo 是否被标记了指定特性
- `GetAttribute<T>()`: 获取某个 MemberInfo 上的特性数组

### 2. ConfigurationExtensions (静态类)
**功能**: IConfiguration 的扩展方法
- `Get<T>()`: 将配置节点反序列化为强类型对象

### 3. DateTimeExtensions (静态类)
**功能**: DateTime 类型的扩展方法
- `Timestamp()`: 将 DateTime 转换为时间戳
- `ParseFromTimestamp()`: 将时间戳转换为 DateTime
- 提供时间戳原点 `T1970` 常量

### 4. ObjectExtensions (静态类)
**功能**: 对象类型的扩展方法
- `As<T>()`: 类型转换，支持枚举、类、接口、Guid 等类型
- `CheckNotNull()`: 检查对象是否为 null，为 null 时抛出异常
- `IsNullOrEmpty()`: 检查对象是否为空或 null
- `IsNotNull()`: 非空检查
- `ToJsonString()`: 将对象序列化为 JSON 字符串
- `DeepClone<T>()`: 深拷贝对象（已标记为过时）

### 5. RandomExtensions (静态类)
**功能**: Random 类型的扩展辅助操作类
- 提供中文姓氏数组 `Xings`
- 包含各种随机数生成的扩展方法（具体方法需查看完整源码）

### 6. StringExtensions (静态类)
**功能**: 字符串类型的扩展辅助操作类
- `IsMatch()`: 正则表达式匹配
- `Matches()`: 获取所有正则匹配项
- `MatchNumbers()`: 匹配所有数字字符串
- `IsMatchNumber()`: 检测是否包含数字
- `Join<T>()`: 将集合转换为分隔符连接的字符串
- 包含大量字符串处理的扩展方法（具体方法需查看完整源码）

## JSON 处理类 (Json)

### 7. JsonUtils (静态部分类)
**功能**: JSON 格式化配置工具
- `IsEnumAsString`: 控制枚举是否输出为字符串
- `IsExtraDataConverter`: 控制是否使用 ExtraDataJsonConverter
- `IsLongAsString`: 控制 long 类型是否输出为字符串
- `JsonOptions`: 获取预配置的 JsonSerializerOptions

### JSON 转换器 (Json/Converters)

### 8. EmptyStringToNullConverter<T> (泛型类)
**功能**: 空字符串转换为 null 的 JSON 转换器
- 继承自 `JsonConverter<T?>`，用于处理可空结构体类型
- 将空字符串或空白字符串转换为 null

### 9. EmptyStringToNullConverterFactory (类)
**功能**: 空字符串转 null 转换器的工厂类
- 继承自 `JsonConverterFactory`
- 为所有可空类型创建对应的转换器

### 10. ExtraDataJsonConverter (类)
**功能**: 处理包含 ExtraData 的对象的 JSON 转换器
- 继承自 `JsonConverter<object>`
- 将 ExtraData 中的属性序列化到实体主表中
- 支持动态属性的序列化和反序列化

### 11. LongStringConverter (类)
**功能**: long 类型与字符串互转的 JSON 转换器
- 继承自 `JsonConverter<long>`
- 将 long 类型序列化为字符串，反序列化时从字符串转为 long

### 12. NullableLongStringConverter (类)
**功能**: 可空 long 类型与字符串互转的 JSON 转换器
- 继承自 `JsonConverter<long?>`
- 处理可空 long 类型的字符串转换

### JSON 字段相关 (Json/JsonField)

### 13. IHasExtraData (接口)
**功能**: JSON 字段接口
- 定义 `ExtraData` 属性用于存储额外的动态字段
- 提供 `SetProperty()` 和 `GetProperty()` 方法用于动态属性操作

### 14. JsonFieldAttribute (类)
**功能**: 标识实体类中的 JSON 格式字段
- 继承自 `NotMappedAttribute`
- 用于标记不需要映射到数据库的 JSON 字段

## 项目特点

1. **扩展性强**: 提供了大量的扩展方法，增强了基础类型的功能
2. **JSON 处理完善**: 包含多种 JSON 转换器，支持复杂的序列化需求
3. **动态属性支持**: 通过 IHasExtraData 接口支持动态属性的处理
4. **类型安全**: 大量使用泛型和空值检查，提高代码的类型安全性
5. **性能优化**: 使用缓存机制优化反射操作的性能

## 依赖项

- Humanizer.Core (2.14.1)
- Microsoft.Extensions.Configuration (8.0.0)
- Microsoft.Extensions.Configuration.Binder (8.0.2)

## 版本信息

- 目标框架: .NET 8.0
- 当前版本: 8.0.0.0
- 许可证: MIT License
- 版权: Copyright (c) QxStudio