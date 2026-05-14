// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------
namespace Qx.Sprite.EFCore.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Qx.Sprite.Core;
    using Qx.Sprite.Permission;
    using System.Security.Cryptography;
    using Xunit;

    public class RepositoryTests : DependencyTestBase
    {
        public RepositoryTests(DependencyInjectionFixture fixture) : base(fixture)
        {
        }

        [Fact()]
        public void PageCurdTest()
        {
            var repo = this.provider.GetService<IEfRepository<int, Page>>();
            repo.CheckNotNull("PermissionDbContext");

            using (var transaction = repo.EnableTransaction())
            {
                try
                {
                    // 1. Create - 插入数据
                    var newPage = new Page
                    {
                        Name = "TestPage",
                        Path = "/test-page",
                        AppId = "test-app",
                        Redirect = "/test-redirect",
                        ParentId = null,
                        Meta = new PageMeta
                        {
                            Title = "Test Page Title",
                            Icon = "test-icon",
                            Permission = new[] { "read", "write" },
                            ActiveMenu = "test-menu",
                            NoCache = false,
                            Hidden = false,
                            AlwaysShow = true
                        },
                        EndPoints = new List<EndPoint>
                        {
                            new EndPoint
                            {
                                Namespace = "Qx.Sprite.Permission.Controller",
                                Controller = "TestController",
                                Action = "GetTest",
                                Enabled = true,
                                Title = "获取测试数据"
                            },
                            new EndPoint
                            {
                                Namespace = "Qx.Sprite.Permission.Controller",
                                Controller = "TestController",
                                Action = "PostTest",
                                Enabled = true,
                                Title = "提交测试数据"
                            }
                        },
                        Operations = new List<Operation>
                        {
                            new Operation
                            {
                                Code = "add",
                                Title = "新增"
                            },
                            new Operation
                            {
                                Code = "edit",
                                Title = "编辑"
                            },
                            new Operation
                            {
                                Code = "delete",
                                Title = "删除"
                            }
                        },
                        Children = new List<Page>
                        {
                            new Page
                            {
                                Name = "ChildPage1",
                                Path = "/child-page-1",
                                AppId = "test-app",
                                Meta = new PageMeta
                                {
                                    Title = "子页面1",
                                    Icon = "child-icon-1",
                                    Permission = new[] { "read" },
                                    ActiveMenu = "child-menu-1",
                                    NoCache = false,
                                    Hidden = false,
                                    AlwaysShow = true
                                }
                            },
                            new Page
                            {
                                Name = "ChildPage2",
                                Path = "/child-page-2",
                                AppId = "test-app",
                                Meta = new PageMeta
                                {
                                    Title = "子页面2",
                                    Icon = "child-icon-2",
                                    Permission = new[] { "read", "write" },
                                    ActiveMenu = "child-menu-2",
                                    NoCache = true,
                                    Hidden = false,
                                    AlwaysShow = false
                                }
                            }
                        }
                    };

                    repo.Add(newPage);
                    var insertCount = repo.SaveChanges();
                    Assert.True(insertCount > 0, "页面插入失败");
                    Assert.True(newPage.Id > 0, "页面ID未生成");

                    // 2. Read - 读取数据
                    var savedPageId = newPage.Id; // 获取数据库生成的ID
                    var retrievedPage = repo.Get(savedPageId);
                    Assert.NotNull(retrievedPage);
                    Assert.Equal("TestPage", retrievedPage.Name);
                    Assert.Equal("/test-page", retrievedPage.Path);
                    Assert.Equal("test-app", retrievedPage.AppId);
                    Assert.Equal("/test-redirect", retrievedPage.Redirect);
                    Assert.Null(retrievedPage.ParentId);

                    // 验证元数据
                    Assert.NotNull(retrievedPage.Meta);
                    Assert.Equal("Test Page Title", retrievedPage.Meta.Title);
                    Assert.Equal("test-icon", retrievedPage.Meta.Icon);
                    Assert.Equal(new[] { "read", "write" }, retrievedPage.Meta.Permission);
                    Assert.Equal("test-menu", retrievedPage.Meta.ActiveMenu);
                    Assert.False(retrievedPage.Meta.NoCache);
                    Assert.False(retrievedPage.Meta.Hidden);
                    Assert.True(retrievedPage.Meta.AlwaysShow);

                    // 验证审计字段
                    Assert.NotNull(retrievedPage.Creator);
                    Assert.NotEqual(default, retrievedPage.CreationTime);

                    // 验证EndPoints数据
                    Assert.NotNull(retrievedPage.EndPoints);
                    Assert.Equal(2, retrievedPage.EndPoints.Count);
                    var endPoints = retrievedPage.EndPoints.ToList();
                    Assert.Equal("Qx.Sprite.Permission.Controller", endPoints[0].Namespace);
                    Assert.Equal("TestController", endPoints[0].Controller);
                    Assert.Equal("GetTest", endPoints[0].Action);
                    Assert.True(endPoints[0].Enabled);
                    Assert.Equal("获取测试数据", endPoints[0].Title);
                    Assert.Equal("Qx.Sprite.Permission.Controller", endPoints[1].Namespace);
                    Assert.Equal("TestController", endPoints[1].Controller);
                    Assert.Equal("PostTest", endPoints[1].Action);
                    Assert.True(endPoints[1].Enabled);
                    Assert.Equal("提交测试数据", endPoints[1].Title);

                    // 验证Operations数据
                    Assert.NotNull(retrievedPage.Operations);
                    Assert.Equal(3, retrievedPage.Operations.Count);
                    var operations = retrievedPage.Operations.ToList();
                    Assert.Equal("add", operations[0].Code);
                    Assert.Equal("新增", operations[0].Title);
                    Assert.Equal("edit", operations[1].Code);
                    Assert.Equal("编辑", operations[1].Title);
                    Assert.Equal("delete", operations[2].Code);
                    Assert.Equal("删除", operations[2].Title);

                    // 验证Children数据
                    Assert.NotNull(retrievedPage.Children);
                    Assert.Equal(2, retrievedPage.Children.Count);
                    var children = retrievedPage.Children.ToList();
                    Assert.Equal("ChildPage1", children[0].Name);
                    Assert.Equal("/child-page-1", children[0].Path);
                    Assert.Equal("子页面1", children[0].Meta.Title);
                    Assert.Equal("child-icon-1", children[0].Meta.Icon);
                    Assert.Equal(new[] { "read" }, children[0].Meta.Permission);
                    Assert.Equal("ChildPage2", children[1].Name);
                    Assert.Equal("/child-page-2", children[1].Path);
                    Assert.Equal("子页面2", children[1].Meta.Title);
                    Assert.Equal("child-icon-2", children[1].Meta.Icon);
                    Assert.Equal(new[] { "read", "write" }, children[1].Meta.Permission);
                    Assert.True(children[1].Meta.NoCache);

                    // 3. Update - 修改数据
                    retrievedPage.Name = "UpdatedTestPage";
                    retrievedPage.Path = "/updated-test-page";
                    retrievedPage.Meta.Title = "Updated Test Page Title";
                    retrievedPage.Meta.Permission = new[] { "read", "write", "delete" };

                    // 修改EndPoints数据
                    var firstEndPoint = retrievedPage.EndPoints.First();
                    firstEndPoint.Action = "UpdatedGetTest";
                    firstEndPoint.Title = "更新后的获取测试数据";

                    // 修改Operations数据
                    var firstOperation = retrievedPage.Operations.First();
                    firstOperation.Title = "新增操作";

                    // 修改Children数据
                    var firstChild = retrievedPage.Children.First();
                    firstChild.Name = "UpdatedChildPage1";
                    firstChild.Meta.Title = "更新后的子页面1";

                    // 更新子页面的ParentId为实际的父页面ID
                    foreach (var child in retrievedPage.Children)
                    {
                        child.ParentId = retrievedPage.Id;
                    }

                    repo.Update(retrievedPage);
                    var updateCount = repo.SaveChanges();
                    Assert.True(updateCount > 0, "页面更新失败");

                    // 验证更新后的数据
                    var updatedPage = repo.FirstOrDefault(p => p.Id == newPage.Id);
                    Assert.NotNull(updatedPage);
                    Assert.Equal("UpdatedTestPage", updatedPage.Name);
                    Assert.Equal("/updated-test-page", updatedPage.Path);
                    Assert.Equal("Updated Test Page Title", updatedPage.Meta.Title);
                    Assert.Equal(new[] { "read", "write", "delete" }, updatedPage.Meta.Permission);

                    // 验证最后修改时间被更新
                    Assert.NotNull(updatedPage.LastModifier);
                    Assert.NotEqual(default, updatedPage.LastModificationTime);

                    // 验证更新后的EndPoints数据
                    Assert.NotNull(updatedPage.EndPoints);
                    Assert.Equal(2, updatedPage.EndPoints.Count);
                    var updatedEndPoints = updatedPage.EndPoints.ToList();
                    Assert.Equal("UpdatedGetTest", updatedEndPoints[0].Action);
                    Assert.Equal("更新后的获取测试数据", updatedEndPoints[0].Title);

                    // 验证更新后的Operations数据
                    Assert.NotNull(updatedPage.Operations);
                    Assert.Equal(3, updatedPage.Operations.Count);
                    var updatedOperations = updatedPage.Operations.ToList();
                    Assert.Equal("新增操作", updatedOperations[0].Title);

                    // 验证更新后的Children数据
                    Assert.NotNull(updatedPage.Children);
                    Assert.Equal(2, updatedPage.Children.Count);
                    var updatedChildren = updatedPage.Children.ToList();
                    Assert.Equal("UpdatedChildPage1", updatedChildren[0].Name);
                    Assert.Equal("更新后的子页面1", updatedChildren[0].Meta.Title);

                    // 验证更新后的数据
                    var finalUpdatedPage = repo.Get(savedPageId);
                    Assert.Equal("UpdatedTestPage", finalUpdatedPage.Name);
                    Assert.Equal("/updated-test-page", finalUpdatedPage.Path);

                    // 验证相关数据更新
                    Assert.Equal(2, finalUpdatedPage.EndPoints.Count);
                    var finalEndPoints = finalUpdatedPage.EndPoints.ToList();
                    Assert.Equal("UpdatedGetTest", finalEndPoints[0].Action);
                    Assert.Equal("更新后的获取测试数据", finalEndPoints[0].Title);

                    Assert.Equal(3, finalUpdatedPage.Operations.Count);
                    var finalOperations = finalUpdatedPage.Operations.ToList();
                    Assert.Equal("新增操作", finalOperations[0].Title);

                    Assert.Equal(2, finalUpdatedPage.Children.Count);
                    var finalChildren = finalUpdatedPage.Children.ToList();
                    Assert.Equal("UpdatedChildPage1", finalChildren[0].Name);
                    Assert.Equal("更新后的子页面1", finalChildren[0].Meta.Title);

                    // 5. Delete - 删除数据
                    repo.Remove(updatedPage);
                    var deleteCount = repo.SaveChanges();
                    Assert.True(deleteCount > 0, "页面删除失败");

                    // 验证删除后的数据
                    var deletedPage = repo.FirstOrDefault(p => p.Id == savedPageId);
                    Assert.Null(deletedPage);

                    // 回滚事务（测试用例不需要真正提交到数据库）
                    repo.Commit();
                }
                catch (Exception)
                {
                    // 如果发生异常，回滚事务
                    repo.Rollback();
                    throw;
                }
            }
        }

        [Fact()]
        public void RoleCurdTest()
        {
            var roleRepo = this.provider.GetService<IEfRepository<int, Role>>();
            var pageRepo = this.provider.GetService<IEfRepository<int, Page>>();
            var userRoleRepo = this.provider.GetService<IEfRepository<long, User>>();
            roleRepo.CheckNotNull("Role repository should not be null");
            pageRepo.CheckNotNull("Page repository should not be null");
            userRoleRepo.CheckNotNull("UserRole repository should not be null");

            //using (var transaction = roleRepo.EnableTransaction())
            {
                try
                {
                    // 1. Create - 创建测试页面用于权限关联
                    var testPage = new Page
                    {
                        Name = "TestPageForRole",
                        Path = "/test-page-for-role",
                        AppId = "test-app",
                        Meta = new PageMeta
                        {
                            Title = "Test Page For Role",
                            Icon = "test-icon",
                            Permission = new[] { "read", "write" },
                            ActiveMenu = "test-menu",
                            NoCache = false,
                            Hidden = false,
                            AlwaysShow = true
                        }
                    };

                    pageRepo.Add(testPage);
                    pageRepo.SaveChanges();
                    Assert.True(testPage.Id > 0, "测试页面创建失败");

                    // 2. Create - 插入角色数据
                    var newRole = new Role
                    {
                        AppId = "test-app",
                        Title = "TestRole",
                        TenantId = "test-tenant",
                        Permissions = new List<Permission>
                        {
                            new Permission
                            {
                                PageId = testPage.Id,
                                ActionPermission = new[] { 1, 2, 3 }, // 对应增删改操作
                                EndPoints = new[] { 1, 2 } // 对应两个接入点
                            }
                        },
                    };

                    roleRepo.Add(newRole);
                    var insertCount = roleRepo.SaveChanges();
                    Assert.True(insertCount > 0, "角色插入失败");
                    Assert.True(newRole.Id > 0, "角色ID未生成");

                    // 3. Create - 创建用户角色关联数据
                    var userRole1 = new User
                    {
                        UserId = 1001,
                        Mobile = "13800138000",
                        Name = "张三",
                        TenantId = "test-tenant",
                        Roles = [newRole] // 关联到新创建的角色
                    };

                    var userRole2 = new User
                    {
                        UserId = 1002,
                        Mobile = "13800138001",
                        Name = "李四",
                        TenantId = "test-tenant",
                        Roles = [newRole] // 关联到同一个角色
                    };

                    userRoleRepo.Add(userRole1);
                    userRoleRepo.Add(userRole2);
                    var userRoleInsertCount = userRoleRepo.SaveChanges();
                    Assert.True(userRoleInsertCount > 0, "用户角色关联插入失败");
                    Assert.True(userRole1.Id > 0, "用户角色1 ID未生成");
                    Assert.True(userRole2.Id > 0, "用户角色2 ID未生成");

                    // 4. Read - 读取角色数据并验证UserRoles关联
                    var savedRoleId = newRole.Id;
                    var retrievedRole = roleRepo.Get(savedRoleId);
                    Assert.NotNull(retrievedRole);
                    Assert.Equal("TestRole", retrievedRole.Title);
                    Assert.Equal("test-app", retrievedRole.AppId);
                    Assert.Equal("test-tenant", retrievedRole.TenantId);

                    // 验证UserRoles关联 - 从Role侧验证
                    Assert.NotNull(retrievedRole.Users);
                    Assert.Equal(2, retrievedRole.Users.Count);
                    var userRoles = retrievedRole.Users.ToList();
                    Assert.Contains(userRoles, ur => ur.UserId == 1001 && ur.Name == "张三");
                    Assert.Contains(userRoles, ur => ur.UserId == 1002 && ur.Name == "李四");

                    // 验证审计字段
                    Assert.NotNull(retrievedRole.Creator);
                    Assert.NotEqual(default, retrievedRole.CreationTime);

                    // 验证权限数据
                    Assert.NotNull(retrievedRole.Permissions);
                    Assert.Equal(1, retrievedRole.Permissions.Count);
                    var permission = retrievedRole.Permissions.First();
                    Assert.Equal(testPage.Id, permission.PageId);
                    Assert.Equal(new[] { 1, 2, 3 }, permission.ActionPermission);
                    Assert.Equal(new[] { 1, 2 }, permission.EndPoints);

                    // 5. Read - 从UserRole侧验证角色关联
                    var retrievedUserRole1 = userRoleRepo.Get(userRole1.Id);
                    Assert.NotNull(retrievedUserRole1);
                    Assert.NotNull(retrievedUserRole1.Roles);
                    Assert.Equal(1, retrievedUserRole1.Roles.Count());
                    Assert.Equal(newRole.Id, retrievedUserRole1.Roles.First().Id);
                    Assert.Equal("TestRole", retrievedUserRole1.Roles.First().Title);

                    // 6. Update - 修改角色数据并验证UserRoles关联更新
                    retrievedRole.Title = "UpdatedTestRole";
                    retrievedRole.TenantId = "updated-tenant";

                    // 修改权限数据
                    var existingPermission = retrievedRole.Permissions.First();
                    existingPermission.ActionPermission = new[] { 1, 2, 3, 4 }; // 增加一个操作权限
                    existingPermission.EndPoints = new[] { 1, 2, 3 }; // 增加一个接入点

                    roleRepo.Update(retrievedRole);
                    var updateCount = roleRepo.SaveChanges();
                    Assert.True(updateCount > 0, "角色更新失败");

                    // 验证更新后的数据
                    var updatedRole = roleRepo.FirstOrDefault(r => r.Id == savedRoleId);
                    Assert.NotNull(updatedRole);
                    Assert.Equal("UpdatedTestRole", updatedRole.Title);
                    Assert.Equal("updated-tenant", updatedRole.TenantId);

                    // 验证UserRoles关联在更新后仍然有效
                    Assert.Equal(2, updatedRole.Users.Count);
                    var updatedUserRoles = updatedRole.Users.ToList();
                    Assert.Contains(updatedUserRoles, ur => ur.UserId == 1001 && ur.Name == "张三");
                    Assert.Contains(updatedUserRoles, ur => ur.UserId == 1002 && ur.Name == "李四");

                    // 验证最后修改时间被更新
                    Assert.NotNull(updatedRole.LastModifier);
                    Assert.NotEqual(default, updatedRole.LastModificationTime);

                    // 验证权限数据更新
                    Assert.Equal(1, updatedRole.Permissions.Count);
                    var updatedPermission = updatedRole.Permissions.First();
                    Assert.Equal(new[] { 1, 2, 3, 4 }, updatedPermission.ActionPermission);
                    Assert.Equal(new[] { 1, 2, 3 }, updatedPermission.EndPoints);

                    // 7. Update - 修改UserRole关联（添加新用户到角色）
                    var userRole3 = new User
                    {
                        UserId = 1003,
                        Mobile = "13800138002",
                        Name = "王五",
                        TenantId = "test-tenant",
                        Roles = new[] { updatedRole }
                    };

                    userRoleRepo.Add(userRole3);
                    userRoleRepo.SaveChanges();

                    // 验证UserRoles关联更新
                    var roleAfterUserRoleAdd = roleRepo.Get(savedRoleId);
                    Assert.Equal(3, roleAfterUserRoleAdd.Users.Count);
                    Assert.Contains(roleAfterUserRoleAdd.Users, ur => ur.UserId == 1003 && ur.Name == "王五");

                    // 8. Update - 修改UserRole关联（从角色中移除用户）
                    roleAfterUserRoleAdd.Users.Remove(userRole1);
                    userRoleRepo.Remove(userRole1); // 移除张三的关联
                    userRoleRepo.SaveChanges();
                    
                    // 重新获取角色数据以刷新导航属性
                    roleAfterUserRoleAdd = roleRepo.Get(savedRoleId);

                    // 验证UserRoles关联在移除后仍然有效
                    var roleAfterUserRoleRemove = roleRepo.Get(savedRoleId);
                    Assert.Equal(2, roleAfterUserRoleRemove.Users.Count);
                    Assert.DoesNotContain(roleAfterUserRoleRemove.Users, ur => ur.UserId == 1001);
                    Assert.Contains(roleAfterUserRoleRemove.Users, ur => ur.UserId == 1002 && ur.Name == "李四");
                    Assert.Contains(roleAfterUserRoleRemove.Users, ur => ur.UserId == 1003 && ur.Name == "王五");

                    // 9. Delete - 删除角色数据（应该级联删除相关的UserRole关联）
                    roleRepo.Remove(updatedRole);
                    var deleteCount = roleRepo.SaveChanges();
                    Assert.True(deleteCount > 0, "角色删除失败");

                    // 验证删除后的数据
                    var deletedRole = roleRepo.FirstOrDefault(r => r.Id == savedRoleId);
                    Assert.Null(deletedRole);

                    // 验证相关的UserRole关联是否还存在（应该仍然存在，因为UserRole是独立实体）
                    var remainingUserRole = userRoleRepo.Get(userRole2.Id);
                    Assert.NotNull(remainingUserRole); // UserRole实体应该仍然存在
                    // 注意：由于是多对多关系，角色的删除不应该自动删除UserRole实体，
                    // 但UserRole中的Roles数组应该不再包含已删除的角色

                    // 清理测试数据 - 删除测试页面和用户角色
                    pageRepo.Remove(testPage);
                    pageRepo.SaveChanges();

                    userRoleRepo.Remove(userRole2);
                    userRoleRepo.Remove(userRole3);
                    userRoleRepo.SaveChanges();

                    // 提交事务
                    roleRepo.Commit();
                }
                catch (Exception)
                {
                    // 如果发生异常，回滚事务
                    roleRepo.Rollback();
                    throw;
                }
            }
        }
    }
}