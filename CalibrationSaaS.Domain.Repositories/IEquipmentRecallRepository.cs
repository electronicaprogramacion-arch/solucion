using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Domain.Repositories
{
    public interface IEquipmentRecallRepository
    {
        /// <summary>
        /// Get equipment recalls based on filter criteria
        /// </summary>
        /// <param name="filter">Filter criteria including customer, date ranges</param>
        /// <returns>Collection of equipment recalls</returns>
        Task<IEnumerable<EquipmentRecall>> GetEquipmentRecalls(EquipmentRecallFilter filter);

        /// <summary>
        /// Get equipment recalls with pagination
        /// </summary>
        /// <param name="pagination">Pagination parameters with filter criteria</param>
        /// <returns>Paginated result set of equipment recalls</returns>
        Task<ResultSet<EquipmentRecall>> GetEquipmentRecallsPaginated(Pagination<EquipmentRecall> pagination);

        /// <summary>
        /// Get equipment recalls count for dashboard/summary purposes
        /// </summary>
        /// <param name="filter">Filter criteria</param>
        /// <returns>Count of equipment recalls matching criteria</returns>
        Task<int> GetEquipmentRecallsCount(EquipmentRecallFilter filter);

        /// <summary>
        /// Save changes to the database
        /// </summary>
        /// <returns>True if save was successful</returns>
        Task<bool> Save();
    }
}
