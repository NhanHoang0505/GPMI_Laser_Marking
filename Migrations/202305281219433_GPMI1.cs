namespace GPMI_Laser_Marking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GPMI1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        FullName = c.String(maxLength: 50),
                        Password = c.String(maxLength: 50),
                        Marking = c.Boolean(nullable: false),
                        FQA = c.Boolean(nullable: false),
                        Pakaging = c.Boolean(nullable: false),
                        Shipping = c.Boolean(nullable: false),
                        Manager = c.Boolean(nullable: false),
                        Import = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.NGMarking",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Part_ID = c.String(maxLength: 50),
                        Marking_Status = c.String(maxLength: 10),
                        Date_Time_Marking = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        Station = c.String(maxLength: 15),
                        Marking_Account = c.String(maxLength: 50),
                        Part_Number = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OrderInput",
                c => new
                    {
                        Order_No = c.String(nullable: false, maxLength: 50),
                        Part_Number = c.String(nullable: false, maxLength: 50),
                        Part_Name = c.String(maxLength: 60),
                        Version = c.String(maxLength: 10),
                        Vendor_Code = c.String(maxLength: 50),
                        Order_Date = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        Order_Quantity = c.Int(nullable: false),
                        Pcs_in_Box = c.Int(nullable: false),
                        Input_Account = c.String(maxLength: 50),
                        Note = c.String(),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.Order_No);
            
            CreateTable(
                "dbo.OutputMarking",
                c => new
                    {
                        Part_ID = c.String(nullable: false, maxLength: 60),
                        Station = c.String(maxLength: 15),
                        Date_Time_Marking = c.DateTime(storeType: "smalldatetime"),
                        Marking_Account = c.String(maxLength: 50),
                        FQA_Status = c.String(maxLength: 10),
                        Date_Time_FQA = c.DateTime(storeType: "smalldatetime"),
                        FQA_Account = c.String(maxLength: 50),
                        QR_CodeID = c.String(maxLength: 60),
                        Pcs_in_Box = c.Int(),
                        Box_No = c.Int(),
                        Date_Time_Pakaging = c.DateTime(storeType: "smalldatetime"),
                        Pakaging_Account = c.String(maxLength: 50),
                        Shipping_Code = c.String(maxLength: 50),
                        Date_Time_Shipping = c.DateTime(storeType: "smalldatetime"),
                        Shipping_Code_Account = c.String(maxLength: 50),
                        Part_Number = c.String(maxLength: 50),
                        Order_No = c.String(maxLength: 50),
                        Part_Name = c.String(maxLength: 60),
                        Version = c.String(maxLength: 10),
                        Vendor_Code = c.String(maxLength: 50),
                        Order_Date = c.DateTime(storeType: "smalldatetime"),
                        Order_Quantity = c.Int(),
                        CNCMachine = c.String(maxLength: 50),
                        TestAirMachine = c.String(maxLength: 50),
                        TestAirValue = c.Single(),
                        TestAirTime = c.DateTime(),
                        TestAirAccount = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.Part_ID)
                .Index(t => t.QR_CodeID)
                .Index(t => t.Shipping_Code)
                .Index(t => t.Part_Number)
                .Index(t => t.Order_No);
            
            CreateTable(
                "dbo.PackagingManager",
                c => new
                    {
                        QRCODE_ID = c.String(nullable: false, maxLength: 60),
                        Order_No = c.String(maxLength: 50),
                        CurrentPCS = c.Int(nullable: false),
                        Box_no = c.Int(nullable: false),
                        pcs_in_box = c.Int(nullable: false),
                        paStatus = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.QRCODE_ID)
                .Index(t => t.Order_No);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PackagingManager", new[] { "Order_No" });
            DropIndex("dbo.OutputMarking", new[] { "Order_No" });
            DropIndex("dbo.OutputMarking", new[] { "Part_Number" });
            DropIndex("dbo.OutputMarking", new[] { "Shipping_Code" });
            DropIndex("dbo.OutputMarking", new[] { "QR_CodeID" });
            DropTable("dbo.PackagingManager");
            DropTable("dbo.OutputMarking");
            DropTable("dbo.OrderInput");
            DropTable("dbo.NGMarking");
            DropTable("dbo.Account");
        }
    }
}
