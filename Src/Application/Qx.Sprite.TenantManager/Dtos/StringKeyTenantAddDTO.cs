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
    /// 新增商户 DTO
    /// </summary>
    public class StringKeyTenantAddDTO
    {
        /// <summary>
        /// Gets or sets 账户名（作为主键）
        /// </summary>
        [Required]
        [StringLength(25)]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets 商户名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets 手机号
        /// </summary>
        [StringLength(12)]
        public string? MobileNo { get; set; }

        /// <summary>
        /// Gets or sets 邮箱
        /// </summary>
        [StringLength(50)]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets 过期开始时间
        /// </summary>
        public DateTime? PlanStartTime { get; set; }

        /// <summary>
        /// Gets or sets 过期结束时间
        /// </summary>
        public DateTime? PlanEndTime { get; set; }

        /// <summary>
        /// Gets or sets 最大用户数
        /// </summary>
        public int MaxUserLimit { get; set; }

        /// <summary>
        /// Gets or sets 最大部门数
        /// </summary>
        public int MaxDeptLimit { get; set; }

        /// <summary>
        /// Gets or sets 商户付费计划
        /// </summary>
        public TenantPlan TenantType { get; set; } = TenantPlan.FreePlan;
    }
}