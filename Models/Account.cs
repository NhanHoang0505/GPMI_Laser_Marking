using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMI_Laser_Marking.Models
{
    [Table("Account")]
    public class Account
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        [Key, Required]
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string ID { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string FullName { get; set; }


        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string Password { get; set; }

        public bool Marking { get; set; } = false;
        public bool FQA { get; set; } = false;
        public bool Pakaging { get; set; } = false;
        public bool Shipping { get; set; } = false;
        public bool Manager { get; set; } = false;
        public bool Import { get; set; } = false;
   
    }
}
