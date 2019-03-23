using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace User.Models
{
    //城市枚举
    public enum Cities
    {
        LONDON,PARIS,CHICAGO
    }

    //国家枚举
    public enum Countries
    {
        NONE,UK,FRANCE,USA
    }

    /// <summary>
    /// 用户派生类继承至IdentityUser类
    /// </summary>
    public class AppUser : IdentityUser
    {
        //一些附加的属性
        public Cities City { get; set; }                //城市
        public Countries Country { get; set; }          //国家

        /// <summary>
        /// 根据城市设置国家
        /// </summary>
        /// <param name="city">用户城市名</param>
        public void SetCountryFromCity(Cities city)
        {
            switch (city)
            {
                case Cities.LONDON:
                    Country = Countries.UK;
                    break;
                case Cities.PARIS:
                    Country = Countries.FRANCE;
                    break;
                case Cities.CHICAGO:
                    Country = Countries.USA;
                    break;
                default:
                    Country = Countries.NONE;
                    break;
            }
        }
    }
}