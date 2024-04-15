﻿using CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Interfaces.IRepositories
{
    public interface ISpotOccupationRepository : IRepository<SpotOccupation>
    {
        List<int> GetAvailableSpaces(List<int> parkingSpotIds, DateTime expectedStartDate, DateTime expectedEndDate);
        SpotOccupation FindByCarIdAndDate(int carId, DateTime startDate, DateTime endDate);
		public SpotOccupation GetLatestByCarId(int carId);
	}
}
