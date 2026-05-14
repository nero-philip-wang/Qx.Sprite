// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System.Security.Cryptography;

    /// <summary>
    /// RSA密钥
    /// </summary>
    public interface IRSAKey
    {
        /// <summary>
        /// Gets 公钥
        /// </summary>
        RSAParameters PublicKeys { get; }

        /// <summary>
        /// Gets 私钥
        /// </summary>
        RSAParameters PrivateKeys { get; }
    }
}
