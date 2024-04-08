using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Entities
{
    public class Car : Entity
    {
        [Column("account_id")]
        public int AccountId { get; set; }

        [Column("license_plate")]
        public string LicensePlate { get; set; }
    }
}
