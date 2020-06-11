using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Vega.Controllers.Resources;
using Vega.Core.Models;
using Vega.Persistence;

namespace Vega.Controllers
{
    public class MakesController : Controller
    {
        private readonly VegaDbContext context;
        private readonly IMapper mapper;
        public MakesController(VegaDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }
        [HttpGet("/api/makes")]
        public async Task<IEnumerable<MakeResource>> GetMakes()
        {
            var makes = await context.Makes.Include(m => m.Models).ToListAsync();
            return mapper.Map<List<Make>,List<MakeResource>>(makes);
        }

        [HttpPut("api/makes/{id}")]
        public async Task<IActionResult> update(int id,[FromBody]KeyValuePairResource makeResource)
        {
             if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var make=await context.Makes.FindAsync(id);
            if (make == null)
            {
                return NotFound();
            }
            make.Name=makeResource.Name;
            context.Entry(make).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return  Ok(make);
        }
    }
}