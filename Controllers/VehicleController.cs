using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vega.Controllers.Resources;
using Vega.Core.Models;
using Vega.Core;
using Vega.Persistence;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Vega.Extensions;
using System.Collections.ObjectModel;

namespace Vega.Controllers
{
    [Route("/api/vehicles")]
    public class VehicleController : Controller
    {
        private readonly IMapper mapper;
        private readonly IVehicleRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly VegaDbContext context;
        public VehicleController(IMapper mapper, IVehicleRepository repository, IUnitOfWork unitOfWork, VegaDbContext context)
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
            this.repository = repository;
            this.mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> CreateVechicle([FromBody] SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var vehicle = mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);
            vehicle.LastUpdate = DateTime.Now;
            repository.Add(vehicle);
            await unitOfWork.CompleteAsync();
            vehicle = await repository.GetVehicle(vehicle.Id);
            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVechicle(int id, [FromBody] SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var vehicle = await repository.GetVehicle(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            
            vehicle.VehicleFeatures=new Collection<VehicleFeature>();
            context.Entry(vehicle).State = EntityState.Modified;
            await unitOfWork.CompleteAsync();
            vehicle = mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;
            context.Entry(vehicle).State = EntityState.Modified;
            await unitOfWork.CompleteAsync();

            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await repository.GetVehicle(id, includeRelated: false);
            if (vehicle == null)
            {
                return NotFound();
            }
            repository.Remove(vehicle);
            await unitOfWork.CompleteAsync();
            return Ok(id);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehivle(int id)
        {
            var vehicle = await repository.GetVehicle(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            var vehicleResource = mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(vehicleResource);
        }
        [HttpGet]
        public async Task<QueryResultResource<VehicleResource>> GetVehicles(VehicleQueryResource filterResource){
            var filter = mapper.Map<VehicleQueryResource,VehicleQuery>(filterResource);
            var queryResult=await repository.GetVehicles(filter);
            return mapper.Map<QueryResult<Vehicle>,QueryResultResource<VehicleResource>>(queryResult);
        }
    }
}