using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class CarManager : ICarService
    {
        ICarDal _carDal;

        public CarManager(ICarDal carDal)
        {
            _carDal = carDal;
        }

        public List<Car> GetAll()
        {
            return _carDal.GetAll();
        }

        public List<Car> GetAllByBrandId(int brandId)
        {
            return _carDal.GetAll(b => b.BrandId == brandId);
        }

        public List<Car> GetAllByColorId(int colorId)
        {
            return _carDal.GetAll(cl => cl.ColorId == colorId);
        }

        public Car GetById(int carId)
        {
            return _carDal.Get(c => c.CarId == carId);
        }

        public List<CarDetailDto> GetCarDetails()
        {
            return _carDal.GetCarDetails();
        }
    }
}
