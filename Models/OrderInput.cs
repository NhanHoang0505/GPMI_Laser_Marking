using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMI_Laser_Marking.Models
{
    [Table("OrderInput")]
    public class OrderInput
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key,Required]
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string Order_No { get; set; }
        
        [Required]     
        [StringLength(50)]
        [Column(TypeName ="nvarchar")]
        public string Part_Number { get; set; }


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
        public DateTime Order_Date { get; set; }
        public int Order_Quantity { get; set; }
        public int Pcs_in_Box { get; set; }
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string Input_Account { get; set; }
        public string Note { get; set; }
        public byte[] Image { get; set; }

    }
}
