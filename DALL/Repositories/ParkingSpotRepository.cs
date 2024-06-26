﻿using CORE.Entities;
using CORE.Interfaces.IRepositories;
using DALL.Context;

namespace DALL.Repositories
{
    public class ParkingSpotRepository : Repository<ParkingSpot>, IParkingSpotRepository
    {
        public ParkingSpotRepository(GarageContext context) : base(context) { }
    }
}
