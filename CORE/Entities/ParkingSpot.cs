using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Entities
{
    public class ParkingSpot : Entity
    {
        [Column("parking_garage_id")]
        public int GarageId { get; set; }

        [Column("disabled")]
        public bool IsDisabled { get; set; }

        [Column("code_name")]
        public string CodeName { get; set; }
    }
}
