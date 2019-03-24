namespace User.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CityProperty : DbMigration
    {
        /// <summary>
        /// Up方法描述了数据库升级时，需要对架构所做的修改
        /// </summary>
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "City", c => c.Int(nullable: false));
        }
        
        /// <summary>
        /// 降级即恢复时
        /// </summary>
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "City");
        }
    }
}
