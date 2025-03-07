using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMI_Laser_Marking.Models
{
    [Table("PackagingManager")]
    public class PackagingManager
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Required]
        [StringLength(60)]
        [Column(TypeName = "nvarchar")]
        public string QRCODE_ID { get; set; }
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        [Index]
        public string Order_No { get; set; }
        public int CurrentPCS { get; set; } = 0;
        public int Box_no { get; set; } = 0;
        public int pcs_in_box { get; set; } = 0;
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string paStatus { get; set; }
    }
}
