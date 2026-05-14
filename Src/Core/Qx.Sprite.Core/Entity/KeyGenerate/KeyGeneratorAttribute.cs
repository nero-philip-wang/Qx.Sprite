// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;

    /// <summary>
    /// 标记字段采用的Id生成器
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyGeneratorAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyGeneratorAttribute"/> class.
        /// </summary>
        /// <param name="algorithm"></param>
        public KeyGeneratorAttribute(LongKeyGeneratorType algorithm)
        {
            this.Algorithm = algorithm;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyGeneratorAttribute"/> class.
        /// </summary>
        /// <param name="algorithm"></param>
        public KeyGeneratorAttribute(StringKeyGeneratorType algorithm)
        {
            this.Algorithm = algorithm;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyGeneratorAttribute"/> class.
        /// </summary>
        /// <param name="algorithm"></param>
        public KeyGeneratorAttribute(GuidKeyGeneratorType algorithm)
        {
            this.Algorithm = algorithm;
        }

        /// <summary>
        /// Gets 算法
        /// </summary>
        public Enum Algorithm { get; }
    }
}