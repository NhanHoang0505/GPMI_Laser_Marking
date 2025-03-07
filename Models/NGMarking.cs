using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMI_Laser_Marking.Models
{
    [Table("NGMarking")]
    public class NGMarking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Required]
        public int ID { get; set; }
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string Part_ID { get; set; }
        [StringLength(10)]
        [Column(TypeName = "nvarchar")]
        public string Marking_Status { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime Date_Time_Marking { get; set; }
        [StringLength(15)]
        [Column(TypeName = "nvarchar")]
        public string Station { get; set; }
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string Marking_Account { get; set; }
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string Part_Number { get; set; }

    }
}
