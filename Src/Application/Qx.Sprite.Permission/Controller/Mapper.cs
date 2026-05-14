// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Permission
{
    using AutoMapper;
    using Qx.Sprite.ObjectMapper;
    using Qx.Sprite.Permission.Controller;

    /// <summary>
    /// Provides configuration for object-to-object mapping profiles used in the application.
    /// </summary>
    /// <remarks>The Mapper class defines mapping relationships between domain entities and data transfer
    /// objects (DTOs) to facilitate conversion between different object models. It is typically used during application
    /// startup to configure mapping rules for use throughout the application.</remarks>
    public class Mapper : IMapperConfiguration
    {
        /// <inheritdoc/>
        public void AddAutoMapper(IMapperConfigurationExpression configAction)
        {
            configAction.CreateMap<Page, PageDetailDto>();
            configAction.CreateMap<PageEditDto, Page>();

            configAction.CreateMap<EndPoint, EndPointEditDto>();
            configAction.CreateMap<EndPointEditDto, EndPoint>();
            configAction.CreateMap<Operation, OperationEditDto>();
            configAction.CreateMap<OperationEditDto, Operation>();

            configAction.CreateMap<Role, RoleListDto>();
            configAction.CreateMap<RoleEditDto, Role>();

            configAction.CreateMap<PermissionDto, Permission>();
            configAction.CreateMap<Permission, PermissionDto>();

            configAction.CreateMap<UserEditDto, User>();
            configAction.CreateMap<User, UserDetailDto>();
        }
    }
}