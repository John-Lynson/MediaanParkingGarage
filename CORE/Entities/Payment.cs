using CORE.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Entities
{
    public class Payment : Entity
    {
        [Column("car_id")]
        public int CarId { get; set; }

        [Column("parking_garage_id")]
        public int GarageId { get; set; }

        [Column("cents")]
        public decimal Price { get; set; }

        [Column("type")]
        public PaymentType Type { get; set; }

        [Column("payment_datetime")]
        public DateTime Date { get; set; }

        [Column("is_paid")]
        public bool IsPaid { get; set; }

        [Column("mollie_payment_id")]
        public string? MolliePaymentId { get; set; }

        [Column("mollie_checkout_url")]
        public string? MollieCheckoutUrl { get; set; }

    }
}
