using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace User.Models
{
    /// <summary>
    /// 定义创建用户帐号所需要的基本属性
    /// </summary>
    public class CreateModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    /// <summary>
    /// 登录验证模型
    /// </summary>
    public class LoginModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }

    /// <summary>
    /// 该类实现在系统中传递角色细节和用户细节，按成员进行归类
    /// </summary>
    public class RoleEditModel
    {
        public AppRole Role { get; set; }
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }
    }

    /// <summary>
    /// 该类在用户递交它们的修改时，从模型绑定系统接受到一个类
    /// </summary>
    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }

    public class UserViewModels
    {
    }
}