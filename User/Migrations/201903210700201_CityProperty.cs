namespace User.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CityProperty : DbMigration
    {
        /// <summary>
        /// Up�������������ݿ�����ʱ����Ҫ�Լܹ��������޸�
        /// </summary>
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "City", c => c.Int(nullable: false));
        }
        
        /// <summary>
        /// �������ָ�ʱ
        /// </summary>
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "City");
        }
    }
}
