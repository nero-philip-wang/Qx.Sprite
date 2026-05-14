// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// ValueObject
    /// </summary>
    /// <remarks>Inspired from https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/implement-value-objects</remarks>
    public abstract class ValueObject
    {
        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            ValueObject other = (ValueObject)obj;
            IEnumerator<object> thisValues = this.GetAtomicValues().GetEnumerator();
            IEnumerator<object> otherValues = other.GetAtomicValues().GetEnumerator();
            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (ReferenceEquals(thisValues.Current, null) ^
                    ReferenceEquals(otherValues.Current, null))
                {
                    return false;
                }

                if (thisValues.Current != null &&
                    !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }

            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.GetAtomicValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        /// <summary>
        /// ==
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        protected static bool EqualOperator(ValueObject? left, ValueObject? right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }

            return ReferenceEquals(left, null) || left.Equals(right);
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        protected static bool NotEqualOperator(ValueObject? left, ValueObject? right)
        {
            return !(EqualOperator(left, right));
        }

        /// <summary>
        /// 获取值对象的原子值
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<object> GetAtomicValues();

        // Other utilility methods
    }
}