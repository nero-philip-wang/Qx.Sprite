// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <inheritdoc/>
    public class RSAKey : IRSAKey, ISingleton
    {
        private string publicKeysPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data/Secret/key.public.pem");
        private string privateKeysPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data/Secret/key.private.pem");

        private RSAParameters publicKeys;
        private RSAParameters privateKeys;

        /// <inheritdoc/>
        public RSAParameters PublicKeys =>
            this.publicKeys.Equals(default(RSAParameters)) ? this.FromFile().PublicKeys : this.publicKeys;

        /// <inheritdoc/>
        public RSAParameters PrivateKeys =>
            this.publicKeys.Equals(default(RSAParameters)) ? this.FromFile().PrivateKeys : this.privateKeys;

        private (RSAParameters PublicKeys, RSAParameters PrivateKeys) FromFile()
        {
            if (File.Exists(this.publicKeysPath) && File.Exists(this.privateKeysPath))
            {
                using (var rsa = new RSACryptoServiceProvider(2048))
                {
                    var pem = File.ReadAllText(this.privateKeysPath);
                    rsa.ImportFromPem(pem);
                    return (this.publicKeys = rsa.ExportParameters(false), this.privateKeys = rsa.ExportParameters(true));
                }
            }
            else
            {
                return this.GenerateAndSaveKey();
            }
        }

        /// <summary>
        /// 生成并保存 RSA 公钥与私钥
        /// </summary>
        /// <returns></returns>
        private (RSAParameters PublicKeys, RSAParameters PrivateKeys) GenerateAndSaveKey()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    this.privateKeys = rsa.ExportParameters(true);
                    this.publicKeys = rsa.ExportParameters(false);
                    Directory.CreateDirectory(Path.GetDirectoryName(this.publicKeysPath)!);
                    File.WriteAllText(this.publicKeysPath, rsa.ExportRSAPublicKeyPem());
                    File.WriteAllText(this.privateKeysPath, rsa.ExportRSAPrivateKeyPem());
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }

            return (this.publicKeys, this.privateKeys);
        }
    }
}