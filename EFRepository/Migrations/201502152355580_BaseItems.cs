namespace Nance.EFRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaseItems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NanceUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DisplayName = c.String(),
                        UserName = c.String(),
                        Email = c.String(),
                        MMSNumber = c.String(),
                        CurrentBudgetTemplate_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BudgetTemplates", t => t.CurrentBudgetTemplate_Id)
                .Index(t => t.CurrentBudgetTemplate_Id);
            
            CreateTable(
                "dbo.Budgets",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PeriodStart = c.DateTime(nullable: false),
                        PeriodEnd = c.DateTime(nullable: false),
                        NanceUser_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NanceUsers", t => t.NanceUser_Id)
                .Index(t => t.NanceUser_Id);
            
            CreateTable(
                "dbo.BudgetItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DueDate = c.DateTime(),
                        PaidDate = c.DateTime(),
                        Notes = c.String(),
                        Budget_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Budgets", t => t.Budget_Id)
                .Index(t => t.Budget_Id);
            
            CreateTable(
                "dbo.BudgetItemPayments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(),
                        PaidDate = c.DateTime(),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        Notes = c.String(),
                        BudgetItem_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BudgetItems", t => t.BudgetItem_Id)
                .Index(t => t.BudgetItem_Id);
            
            CreateTable(
                "dbo.BudgetDebits",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(),
                        TransactionId = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(),
                        Budget_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Budgets", t => t.Budget_Id)
                .Index(t => t.Budget_Id);
            
            CreateTable(
                "dbo.BudgetTemplates",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstPayDay = c.Int(),
                        SecondPayDay = c.Int(),
                        NanceUser_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NanceUsers", t => t.NanceUser_Id)
                .Index(t => t.NanceUser_Id);
            
            CreateTable(
                "dbo.BudgetItemTemplates",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Day = c.Int(),
                        DayOfWeek = c.Int(),
                        Name = c.String(),
                        Description = c.String(),
                        Due = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BudgetTemplate_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BudgetTemplates", t => t.BudgetTemplate_Id)
                .Index(t => t.BudgetTemplate_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.BudgetItemTemplates", new[] { "BudgetTemplate_Id" });
            DropIndex("dbo.BudgetTemplates", new[] { "NanceUser_Id" });
            DropIndex("dbo.BudgetDebits", new[] { "Budget_Id" });
            DropIndex("dbo.BudgetItemPayments", new[] { "BudgetItem_Id" });
            DropIndex("dbo.BudgetItems", new[] { "Budget_Id" });
            DropIndex("dbo.Budgets", new[] { "NanceUser_Id" });
            DropIndex("dbo.NanceUsers", new[] { "CurrentBudgetTemplate_Id" });
            DropForeignKey("dbo.BudgetItemTemplates", "BudgetTemplate_Id", "dbo.BudgetTemplates");
            DropForeignKey("dbo.BudgetTemplates", "NanceUser_Id", "dbo.NanceUsers");
            DropForeignKey("dbo.BudgetDebits", "Budget_Id", "dbo.Budgets");
            DropForeignKey("dbo.BudgetItemPayments", "BudgetItem_Id", "dbo.BudgetItems");
            DropForeignKey("dbo.BudgetItems", "Budget_Id", "dbo.Budgets");
            DropForeignKey("dbo.Budgets", "NanceUser_Id", "dbo.NanceUsers");
            DropForeignKey("dbo.NanceUsers", "CurrentBudgetTemplate_Id", "dbo.BudgetTemplates");
            DropTable("dbo.BudgetItemTemplates");
            DropTable("dbo.BudgetTemplates");
            DropTable("dbo.BudgetDebits");
            DropTable("dbo.BudgetItemPayments");
            DropTable("dbo.BudgetItems");
            DropTable("dbo.Budgets");
            DropTable("dbo.NanceUsers");
        }
    }
}
