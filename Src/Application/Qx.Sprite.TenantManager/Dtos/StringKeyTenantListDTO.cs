// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.TenantManager
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using NodaTime;

    /// <summary>
    /// 商户列表 DTO（用于列表展示）
    /// </summary>
    public class StringKeyTenantListDTO
    {
        /// <summary>
        /// Gets or sets 账户名
        /// </summary>
        [StringLength(25)]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets 商户名称
        /// </summary>
        [StringLength(50)]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets 手机号
        /// </summary>
        [StringLength(12)]
        public string? MobileNo { get; set; }

        /// <summary>
        /// Gets or sets 到期时间（用于列表快速查看）
        /// </summary>
        public DateTime? PlanEndTime { get; set; }

        /// <summary>
        /// Gets or sets 商户付费计划
        /// </summary>
        public TenantPlan TenantType { get; set; }
    }
}