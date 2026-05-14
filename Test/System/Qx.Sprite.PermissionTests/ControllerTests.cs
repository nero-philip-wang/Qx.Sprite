// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------
namespace Qx.Sprite.EFCore.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Qx.Sprite.Core;
    using Qx.Sprite.Permission;
    using Qx.Sprite.Permission.Controller;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Xunit;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using AutoMapper;

    public class ControllerTests : DependencyTestBase
    {
        public ControllerTests(DependencyInjectionFixture fixture) : base(fixture)
        {
        }

        [Fact()]
        public async Task PagesController_CRUD_Test()
        {
            // Arrange
            var pageRepository = this.provider.GetService<IEfRepository<int, Page>>();
            var mapper = this.provider.GetService<IMapper>();
            
            var controller = this.provider.GetService<PagesController>(); 
            
            // Test Create
            var createDto = new PageEditDto
            {
                AppId = "test-app",
                Name = "Test Page",
                Path = "/test-page",
                Redirect = "/test-redirect",
                Meta = new PageMeta
                {
                    Title = "Test Page Title",
                    Icon = "test-icon",
                    Permission = new[] { "read", "write" },
                    ActiveMenu = "test-menu",
                    NoCache = false,
                    Hidden = false,
                    AlwaysShow = true
                }
            };

            // Act - Create
            var result = await controller.CreateAsync(createDto);
            
            // Assert - Create
            Assert.NotNull(result);
            Assert.Equal("Test Page", result.Name);
            Assert.Equal("/test-page", result.Path);
            Assert.Equal("test-app", result.AppId);

            var createdId = result.Id;

            // Act - Read
            var getResult = await controller.GetAsync(createdId);
            
            // Assert - Read
            Assert.NotNull(getResult);
            Assert.Equal(createdId, getResult.Id);
            Assert.Equal("Test Page", getResult.Name);

            // Act - Update
            var updateDto = new PageEditDto
            {
                AppId = "test-app-updated",
                Name = "Updated Test Page",
                Path = "/updated-test-page",
                Meta = new PageMeta
                {
                    Title = "Updated Test Page Title",
                    Icon = "updated-icon"
                }
            };

            var updateResult = await controller.UpdateAsync(createdId, updateDto);
            
            // Assert - Update
            Assert.NotNull(updateResult);
            Assert.Equal("Updated Test Page", updateResult.Name);
            Assert.Equal("/updated-test-page", updateResult.Path);

            // Act - Delete
            await controller.DeleteAsync(createdId);
            
            // Assert - Delete
            await Assert.ThrowsAsync<BusinessException>(async () => await controller.GetAsync(createdId));
        }

        [Fact()]
        public async Task PagesController_UpdateOperations_Test()
        {
            // Arrange
            var pageRepository = this.provider.GetService<IEfRepository<int, Page>>();
            var mapper = this.provider.GetService<IMapper>();

            var controller = this.provider.GetService<PagesController>();

            // Create a test page first
            var createDto = new PageEditDto
            {
                AppId = "test-app",
                Name = "Test Page for Operations",
                Path = "/test-page-operations",
                Meta = new PageMeta { Title = "Test Page" }
            };

            var page = await controller.CreateAsync(createDto);
            var operations = new List<OperationEditDto>
            {
                new OperationEditDto { Code = "add", Title = "新增" },
                new OperationEditDto { Code = "edit", Title = "编辑" },
                new OperationEditDto { Code = "delete", Title = "删除" }
            };

            // Act
            await controller.UpdateOperations(page.Id, operations);

            // Assert
            var updatedPage = await controller.GetAsync(page.Id);
            Assert.NotNull(updatedPage);
            
            // Clean up
            await controller.DeleteAsync(page.Id);
        }

        [Fact()]
        public async Task PagesController_UpdateEndPoints_Test()
        {
            // Arrange
            var pageRepository = this.provider.GetService<IEfRepository<int, Page>>();
            var mapper = this.provider.GetService<IMapper>();

            var controller = this.provider.GetService<PagesController>();

            // Create a test page first
            var createDto = new PageEditDto
            {
                AppId = "test-app",
                Name = "Test Page for EndPoints",
                Path = "/test-page-endpoints",
                Meta = new PageMeta { Title = "Test Page" }
            };

            var page = await controller.CreateAsync(createDto);
            var endpoints = new List<EndPointEditDto>
            {
                new EndPointEditDto 
                { 
                    Namespace = "Qx.Sprite.Permission.Controller",
                    Controller = "TestController",
                    Action = "GetTest",
                    Enabled = true,
                    Title = "获取测试数据"
                },
                new EndPointEditDto 
                { 
                    Namespace = "Qx.Sprite.Permission.Controller",
                    Controller = "TestController", 
                    Action = "PostTest",
                    Enabled = true,
                    Title = "提交测试数据"
                }
            };

            // Act
            await controller.UpdateEndPoints(page.Id, endpoints);

            // Assert
            var updatedPage = await controller.GetAsync(page.Id);
            Assert.NotNull(updatedPage);
            
            // Clean up
            await controller.DeleteAsync(page.Id);
        }

        [Fact()]
        public async Task PagesController_ImportPages_Test()
        {
            // Arrange
            var pageRepository = this.provider.GetService<IEfRepository<int, Page>>();
            var mapper = this.provider.GetService<IMapper>();

            var controller = this.provider.GetService<PagesController>();

            var pages = new List<PageEditDto>
            {
                new PageEditDto
                {
                    AppId = "test-app",
                    Name = "Import Page 1",
                    Path = "/import-page-1",
                    Meta = new PageMeta { Title = "Import Page 1" }
                },
                new PageEditDto
                {
                    AppId = "test-app",
                    Name = "Import Page 2",
                    Path = "/import-page-2",
                    Meta = new PageMeta { Title = "Import Page 2" }
                }
            };

            // Act
            await controller.AddRange(pages);

            // Assert - verify pages were imported by checking if they exist
            var allPages = await controller.GetListAsync(new SearchArgs());
            Assert.NotNull(allPages);
            Assert.True(allPages.Total >= 2);
            
            // Clean up - find and delete imported pages
            var importedPages = allPages.Data.Where(p => p.Name.StartsWith("Import Page")).ToList();
            foreach (var page in importedPages)
            {
                await controller.DeleteAsync(page.Id);
            }
        }

        [Fact()]
        public async Task RolesController_CRUD_Test()
        {
            // Arrange
            var roleRepository = this.provider.GetService<IEfRepository<int, Role>>();
            var userRepository = this.provider.GetService<IEfRepository<long, User>>();
            var mapper = this.provider.GetService<IMapper>();

            var controller = this.provider.GetService<RolesController>();

            // Create a test user first
            var user = new User
            {
                UserId = 999999,
                Mobile = "13800138000",
                Name = "Test User for Role",
                TenantId = "test-tenant"
            };
            user = userRepository.Add(user, true);

            // Test Create
            var createDto = new RoleEditDto
            {
                AppId = "test-app",
                Title = "Test Role",
                TenantId = "test-tenant",
                UserIds = new List<long> { user.Id },
                Permissions = new List<PermissionDto>
                {
                    new PermissionDto
                    {
                        PageId = 1,
                        ActionPermission = new[] { 1, 2 },
                        EndPoints = new[] { 1, 2 }
                    }
                }
            };

            // Act - Create
            var result = await controller.CreateAsync(createDto);
            
            // Assert - Create
            Assert.NotNull(result);
            Assert.Equal("Test Role", result.Title);
            Assert.Equal("test-app", result.AppId);

            var createdId = result.Id;

            // Act - Read
            var getResult = await controller.GetAsync(createdId);
            
            // Assert - Read
            Assert.NotNull(getResult);
            Assert.Equal(createdId, getResult.Id);
            Assert.Equal("Test Role", getResult.Title);

            // Act - Update
            var updateDto = new RoleEditDto
            {
                AppId = "test-app-updated",
                Title = "Updated Test Role",
                TenantId = "test-tenant-updated",
                UserIds = new List<long> { user.Id },
                Permissions = new List<PermissionDto>
                {
                    new PermissionDto
                    {
                        PageId = 2,
                        ActionPermission = new[] { 3, 4 },
                        EndPoints = new[] { 3, 4 }
                    }
                }
            };

            var updateResult = await controller.UpdateAsync(createdId, updateDto);
            
            // Assert - Update
            Assert.NotNull(updateResult);
            Assert.Equal("Updated Test Role", updateResult.Title);

            // Act - Delete
            await controller.DeleteAsync(createdId);
            
            // Assert - Delete
            await Assert.ThrowsAsync<BusinessException>(async () => await controller.GetAsync(createdId));
            
            // Clean up user
            userRepository.Remove(user, true);
        }

        [Fact()]
        public async Task UserController_CRUD_Test()
        {
            // Arrange
            var userRepository = this.provider.GetService<IEfRepository<long, User>>();
            var operationRepository = this.provider.GetService<IEfRepository<int, Operation>>();
            var mapper = this.provider.GetService<IMapper>();

            var controller = this.provider.GetService<UsersController>();

            // Test Create
            var createDto = new UserEditDto
            {
                UserId = 888888,
                Mobile = "13900139000",
                Name = "Test User",
                TenantId = "test-tenant",
                RoleIds = new List<int>()
            };

            // Act - Create
            var result = await controller.CreateAsync(createDto);
            
            // Assert - Create
            Assert.NotNull(result);
            Assert.Equal("Test User", result.Name);
            Assert.Equal("13900139000", result.Mobile);
            Assert.Equal(888888, result.UserId);

            var createdId = result.Id;

            // Act - Read
            var getResult = await controller.GetAsync(createdId);
            
            // Assert - Read
            Assert.NotNull(getResult);
            Assert.Equal(createdId, getResult.Id);
            Assert.Equal("Test User", getResult.Name);

            // Act - Update
            var updateDto = new UserEditDto
            {
                UserId = 888888,
                Mobile = "13900139001",
                Name = "Updated Test User",
                TenantId = "test-tenant-updated",
                RoleIds = new List<int>()
            };

            var updateResult = await controller.UpdateAsync(createdId, updateDto);
            
            // Assert - Update
            Assert.NotNull(updateResult);
            Assert.Equal("Updated Test User", updateResult.Name);
            Assert.Equal("13900139001", updateResult.Mobile);

            // Act - Delete
            await controller.DeleteAsync(createdId);
            
            // Assert - Delete
            await Assert.ThrowsAsync<BusinessException>(async () => await controller.GetAsync(createdId));
        }

        [Fact()]
        public async Task UserController_GetRoute_Test()
        {
            // Arrange
            var userRepository = this.provider.GetService<IEfRepository<long, User>>();
            var operationRepository = this.provider.GetService<IEfRepository<int, Operation>>();
            var mapper = this.provider.GetService<IMapper>();
            var pageRepository = this.provider.GetService<IEfRepository<int, Page>>();
            var roleRepository = this.provider.GetService<IEfRepository<int, Role>>();

            var controller = this.provider.GetService<UsersController>();

            // Create test data
            var testPage = new Page
            {
                AppId = "test-app",
                Name = "Test Route Page",
                Path = "/test-route-page",
                Meta = new PageMeta { Title = "Test Route Page" }
            };
            testPage = pageRepository.Add(testPage, true);

            var testOperation = new Operation
            {
                PageId = testPage.Id,
                Code = "read",
                Title = "读取"
            };
            testOperation = operationRepository.Add(testOperation, true);

            var testRole = new Role
            {
                AppId = "test-app",
                Title = "Test Route Role",
                TenantId = "test-tenant"
            };
            testRole = roleRepository.Add(testRole, true);

            var testPermission = new Permission
            {
                RoleId = testRole.Id,
                PageId = testPage.Id,
                ActionPermission = new[] { testOperation.Id },
                EndPoints = new int[] { }
            };
            
            var testUser = new User
            {
                UserId = 777777,
                Mobile = "13700137000",
                Name = "Test Route User",
                TenantId = "test-tenant",
                Roles = new List<Role> { testRole }
            };
            testUser = userRepository.Add(testUser, true);

            // Act
            var result = await controller.GetRoute();
            
            // Assert
            Assert.NotNull(result);
            var routeList = result.ToList();
            Assert.True(routeList.Count > 0);
            
            // Clean up
            userRepository.Remove(testUser, true);
            roleRepository.Remove(testRole, true);
            pageRepository.Remove(testPage, true);
            operationRepository.Remove(testOperation, true);
        }
    }
}