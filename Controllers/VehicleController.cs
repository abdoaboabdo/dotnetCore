using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vega.Controllers.Resources;
using Vega.Models;
using Vega.Persistence;

namespace Vega.Controllers
{
    [Route("/api/vehicles")]
    public class VehicleController:Controller
    {
        private readonly VegaDbContext context;
        private readonly IMapper mapper;
        public VehicleController(VegaDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CreateVechicle([FromBody]VehicleResource vehicleResource){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var vehicle = mapper.Map<VehicleResource,Vehicle>(vehicleResource);
            vehicle.LastUpdate=DateTime.Now;
            context.Vehicles.Add(vehicle);
            await context.SaveChangesAsync();
            var result = mapper.Map<Vehicle,VehicleResource>(vehicle);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVechicle(int id,[FromBody]VehicleResource vehicleResource)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var vehicle = await context.Vehicles.Include(v=>v.VehicleFeatures).SingleOrDefaultAsync(v=> v.Id == id);
            if (vehicle == null)
                return NotFound();
            mapper.Map<VehicleResource,Vehicle>(vehicleResource,vehicle);
            vehicle.LastUpdate=DateTime.Now;
            await context.SaveChangesAsync();
            var result = mapper.Map<Vehicle,VehicleResource>(vehicle);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await context.Vehicles.FindAsync(id);
            if (vehicle == null)
                return NotFound();
            context.Remove(vehicle);
            await context.SaveChangesAsync();
            return Ok(id);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehivle(int id)
        {
            var vehicle = await context.Vehicles.Include(v=>v.VehicleFeatures).SingleOrDefaultAsync(v=> v.Id == id);
            if (vehicle == null)
                return NotFound();
            var vehicleResource = mapper.Map<Vehicle,VehicleResource>(vehicle);
            return Ok(vehicleResource);
        }
    }
}