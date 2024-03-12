using CORE.Entities;
using CORE.Interfaces.IRepositories;
using DALL.Context;

namespace DALL.Repositories
{
    public class SpotOccupationRepository : BaseRepository<SpotOccupation>, ISpotOccupationRepository
    {
        public SpotOccupationRepository(GarageContext context) : base(context) { }
    }
}
