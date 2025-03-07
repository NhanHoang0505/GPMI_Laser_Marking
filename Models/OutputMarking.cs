using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMI_Laser_Marking.Models
{

    [Table("OutputMarking")]
    public class OutputMarking
    {        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Required]
        [StringLength(60)]
        [Column(TypeName = "nvarchar")]      
        public string Part_ID { get; set; }
        [StringLength(15)]
        [Column(TypeName = "nvarchar")]
        public string Station { get; set; }

        //
        [Column(TypeName = "smalldatetime")]
        public DateTime? Date_Time_Marking { get; set; }
        //
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string Marking_Account { get; set; }
        //




        #region FQA
        //FQA
        [StringLength(10)]
        [Column(TypeName = "nvarchar")]
        public string FQA_Status { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Date_Time_FQA { get; set; }
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string FQA_Account { get; set; }

        // pakaging
        [StringLength(60)]
        [Column(TypeName = "nvarchar")]
        [Index]
        public string QR_CodeID { get; set; }

        public int? Pcs_in_Box { get; set; }
        //

        public int? Box_No { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Date_Time_Pakaging { get; set; }
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string Pakaging_Account { get; set; }
        #endregion

        #region Shipping


        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        [Index]
        public string Shipping_Code { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Date_Time_Shipping { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar")]

        public string Shipping_Code_Account { get; set; }
        #endregion


        [Index]
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string Part_Number { get; set; }

        [Index]
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string Order_No { get; set; }
        [StringLength(60)]
        [Column(TypeName = "nvarchar")]
        public string Part_Name { get; set; }
        [StringLength(10)]
        [Column(TypeName = "nvarchar")]
        public string Version { get; set; }
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string Vendor_Code { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? Order_Date { get; set; }
        public int? Order_Quantity { get; set; } = 0;
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string CNCMachine { get; set; }
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string TestAirMachine { get; set; }
        public float? TestAirValue { get; set; }
        public DateTime? TestAirTime { get; set; }
        [StringLength(30)]
        [Column(TypeName = "nvarchar")]
        public string TestAirAccount { get; set; }
    }
}
