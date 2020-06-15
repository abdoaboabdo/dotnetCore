using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vega.Core.Models;
using Vega.Core;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;
using Vega.Extensions;

namespace Vega.Persistence
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VegaDbContext context;
        public VehicleRepository(VegaDbContext context)
        {
            this.context = context;

        }
        public async Task<Vehicle> GetVehicle(int id,bool includeRelated = true)
        {
            if (!includeRelated)
            {
                return await context.Vehicles.FindAsync(id);
            }
                
            return await context.Vehicles
                .Include(v => v.VehicleFeatures)
                    .ThenInclude(vf => vf.Feature)
                .Include(v => v.Model)
                    .ThenInclude(m => m.Make)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public void Add(Vehicle vehicle)
        {
            context.Vehicles.Add(vehicle);
        }

        public void Remove(Vehicle vehicle)
        {
            context.Remove(vehicle);
        }

        public async Task<QueryResult<Vehicle>> GetVehicles(VehicleQuery queryObj)
        {
            var result=new QueryResult<Vehicle>();
            var query =  context.Vehicles
                .Include(v => v.Model)
                    .ThenInclude(m => m.Make)
                .Include(v => v.VehicleFeatures)
                    .ThenInclude(vf => vf.Feature)
                .AsQueryable();

            query=query.ApplyFiltering(queryObj);

            var columnsMap = new Dictionary<string,Expression<Func<Vehicle, object>>>(){
                ["make"] = v=>v.Model.Make.Name,
                ["model"] = v=>v.Model.Name,
                ["contactName"] = v=>v.ContactName,
                // ["id"] = v=>v.Id,
            };
            
            query=query.ApplOrdering(queryObj,columnsMap);
            result.TotalItems = await query.CountAsync();    

            query=query.ApplyPaging(queryObj);
            // if (queryObj.SortBy == "make")
            // {
            //     query = (queryObj.IsSortAscending) ? query.OrderBy(v=>v.Model.Make.Name) : query.OrderByDescending(v=>v.Model.Make.Name);
            // }
            result.Items = await query.ToListAsync();
            return result;
        }


    }
}