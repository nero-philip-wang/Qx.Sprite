// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Permission
{
    /// <summary>
    /// 角色权限数据传输对象
    /// </summary>
    public class PermissionDto
    {
        /// <summary>
        /// Gets or sets 权限ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets 角色ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets 页面ID
        /// </summary>
        public int PageId { get; set; }

        /// <summary>
        /// Gets or sets 操作权限数组
        /// </summary>
        public int[] ActionPermission { get; set; } = System.Array.Empty<int>();

        /// <summary>
        /// Gets or sets 接入点权限数组
        /// </summary>
        public int[] EndPoints { get; set; } = System.Array.Empty<int>();
    }
}