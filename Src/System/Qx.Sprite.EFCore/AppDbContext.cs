// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.EFCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Qx.Sprite.Core;

    /// <summary>
    /// 数据库上下文
    /// </summary>
    public abstract class AppDbContext : DbContext
    {
        /// <summary>
        /// 记录类型和上下文的对照表
        /// </summary>
        public static Dictionary<Guid, Type> EntityInDbContext = new Dictionary<Guid, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        public AppDbContext()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options"></param>
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets 数据库上下文的Schema
        /// </summary>
        protected virtual string Schema { get; } = "dbo";

        /// <summary>
        /// Gets 数据库上下文的表名前缀
        /// </summary>
        protected virtual string TableNamePrefix { get; } = string.Empty;

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// 创建模型
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 默认schema
            modelBuilder.HasDefaultSchema(this.Schema);

            // 运行每个实体自己的 OnModelCreating
            foreach (var type in new AppDomainAssemblyFinder().FindTypesInheritFrom<IOnModelCreating>())
            {
                // 如果基类已经运行了，就跳过
                var has = type.HasAttribute<IgnoreBaseModelCreatingAttribute>();
                if (has) continue;

                var method = type.GetMethod("OnModelCreating");
                var instance = Activator.CreateInstance(type, true);
                if (instance != null)
                {
                    method?.Invoke(instance, new object[] { modelBuilder });
                }
            }

            var allEntities = modelBuilder.Model.GetEntityTypes().Where(c => !c.IsKeyless);
            foreach (var entityType in allEntities)
            {
                // 增加到实体-上下文的对照表
                if (!EntityInDbContext.ContainsKey(entityType.ClrType.GUID)) EntityInDbContext.Add(entityType.ClrType.GUID, this.GetType());

                // 重命名表名
                if (!this.TableNamePrefix.IsNullOrWhiteSpace())
                    entityType.SetTableName($"{this.TableNamePrefix}_{entityType.GetTableName()}");

                // 处理 JsonField
                var properties = entityType.ClrType.GetProperties().Where(p => p.HasAttribute<JsonFieldAttribute>());
                foreach (var property in properties)
                {
                    var propertyType = property.PropertyType;
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(property.Name)
                        .HasConversion(new JsonFieldConverter(propertyType));
                }
            }
        }
    }
}