﻿using CORE.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Entities
{
    public class Payment : BaseEntity
    {
        [ForeignKey("car_id")]
        public int CarId { get; set; }

        [ForeignKey("parking_garage")]
        public int GarageId { get; set; }

        [Column("cents")]
        public int Price { get; set; }

        [Column("type)")]
        public PaymentType Type { get; set; }

        [Column("payment_datetime")]
        public DateTime Date { get; set; }
    }
}