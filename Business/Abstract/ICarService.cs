using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ICarService
    {
        List<Car> GetAll();
        Car GetById(int carId);
        List<Car> GetAllByBrandId(int brandId);
        List<Car> GetAllByColorId(int colorId);
        List<CarDetailDto> GetCarDetails();
    }
}
