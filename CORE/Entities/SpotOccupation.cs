using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Entities
{
    public class SpotOccupation : BaseEntity
    {
        [ForeignKey("parking_spot_id")]
        public int ParkingSpotId { get; set; }

        [ForeignKey("car_id")]
        public int CarId { get; set; }

        [Column("expected_start_datetime")]
        public DateTime ExpectedStartDate { get; set; }

        [Column("expected_end_datetime")]
        public DateTime ExpectedEndDate { get; set; }

        [Column("actual_start_datetime")]
        public DateTime ActualStartDate { get; set; }

        [Column("actual_end_datetime")]
        public DateTime ActualEndDate { get; set; }
    }
}
