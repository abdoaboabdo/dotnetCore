using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Vega.Controllers.Resources;
using Vega.Core;
using Vega.Core.Models;

namespace Vega.Controllers
{
    [Route("/api/vehicles/{vehicleId}/photos")]
    public class PhotosController : Controller
    {
        private readonly IWebHostEnvironment _host;
        private readonly IVehicleRepository repository;
        private readonly IPhotoRepository photoRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly PhotoSettings photoSettings;

        public PhotosController(IWebHostEnvironment host, 
            IVehicleRepository repository,
            IPhotoRepository photoRepository, 
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptionsSnapshot<PhotoSettings> options)
        {
            this.photoSettings=options.Value;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.repository = repository;
            this.photoRepository = photoRepository;
            _host = host;

        }


        public async Task<IEnumerable<PhotoResource>> GetPhotos(int VehicleId)
        {
            var photos= await photoRepository.GetPhotos(VehicleId);
            return mapper.Map<IEnumerable<Photo>,IEnumerable<PhotoResource>>(photos);
        }


        [HttpPost]
        public async Task<IActionResult> Upload(int vehicleId, IFormFile file)
        {
            var vehicle = await repository.GetVehicle(vehicleId, includeRelated: false);
            if (vehicle == null)
            {
                return NotFound();
            }
            if (file == null)
            {
                return BadRequest("Null File");
            }
            if (file.Length == 0)
            {
                return BadRequest("Empty file");
            }
            if (file.Length > photoSettings.MaxBYtes)
            {
                return BadRequest("Max file size exceeded");
            }
            if (!photoSettings.IsSupported(file.FileName))
            {
                return BadRequest("Invalid file type");
            }
            var uploadsFolderPath = Path.Combine(_host.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var photo = new Photo { FileName = fileName };
            vehicle.Photos.Add(photo);
            await unitOfWork.CompleteAsync();

            return Ok(mapper.Map<Photo,PhotoResource>(photo));
        }
    }
}