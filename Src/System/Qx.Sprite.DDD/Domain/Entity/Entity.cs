// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Qx.Sprite.Core;

    /// <summary>
    /// Entity
    /// </summary>
    /// <typeparam name="TKey">主键</typeparam>
    [Serializable]
    public abstract class Entity<TKey> : IHasKey<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity{TKey}"/> class.
        /// </summary>
        protected Entity()
        {
            this.Id = default!;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity{TKey}"/> class.
        /// </summary>
        /// <param name="id"></param>
        protected Entity(TKey id)
        {
            this.Id = id;
        }

        /// <inheritdoc/>
        public virtual TKey Id { get; set; }

        /// <summary>
        /// 判断两个实体是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Entity<TKey>? left, Entity<TKey>? right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null);
            }

            return left.Equals(right);
        }

        /// <summary>
        /// 不相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Entity<TKey>? left, Entity<TKey>? right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Entity<TKey>))
            {
                return false;
            }

            // Same instances must be considered as equal
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            // Transient objects are not considered as equal
            Entity<TKey> other = (Entity<TKey>)obj;
            if (this.Id!.Equals(default) && other.Id!.Equals(default))
            {
                return false;
            }

            // Must have a IS-A relation of types or must be same type
            var typeOfThis = this.GetType().GetTypeInfo();
            var typeOfOther = other.GetType().GetTypeInfo();
            if (!typeOfThis.IsAssignableFrom(typeOfOther) && !typeOfOther.IsAssignableFrom(typeOfThis))
            {
                return false;
            }

            // Different tenants may have an entity with same Id.
            if (this is IMultiTenant thisTenant && other is IMultiTenant otherTenant && thisTenant.TenantId != otherTenant.TenantId)
            {
                return false;
            }

            return this.Id!.Equals(other.Id);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            if (this.Id!.Equals(default)) return 0;
            return this.Id.GetHashCode();
        }

        /// <summary>
        /// 获取实体的所有键值
        /// </summary>
        /// <returns></returns>
        public object[] GetKeys()
        {
            return [this.Id!];
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[ENTITY: {this.GetType().Name}] Id = {this.Id}";
        }
    }
}