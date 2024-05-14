using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Entities
{
    public class Tariff : Entity
    {
        [Column("parking_garage_id")]
        public int GarageId {  get; set; }

        [Column("cents")]
        public int Rate { get; set; }

        [Column("start_time")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }
    }
}
