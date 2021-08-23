using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarImagesController : ControllerBase
    {
        private ICarImageService _carImageService;

        public CarImagesController(ICarImageService carImageService)
        {
            _carImageService = carImageService;
        }

        [HttpGet("getallimages")]
        public IActionResult GetImagesAll()
        {
            var result = _carImageService.GetAll();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result);
        }

        [HttpPost("addimage")]
        public IActionResult Add([FromForm(Name = ("Image"))] IFormFile file, [FromForm] CarImage carImage)
        {
            var result = _carImageService.Add(file, carImage);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("deleteimage")]
        public IActionResult Delete([FromForm(Name = ("ImageId"))] int id)
        {
            var carImage = _carImageService.GetById(id).Data;
            var result = _carImageService.Delete(carImage);
            if (result.Success && carImage != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getallbycarid")]
        public IActionResult GetImagesByCarId(int carId)
        {
            var result = _carImageService.GetAllByCarId(carId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
