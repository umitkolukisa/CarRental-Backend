using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.BusinessRules;
using Core.Utilities.Helpers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;

namespace Business.Concrete
{
    public class CarImageManager : ICarImageService
    {
        ICarImageDal _carImageDal;

        public CarImageManager(ICarImageDal carImageDal)
        {
            _carImageDal = carImageDal;
        }

        [ValidationAspect(typeof(CarImageValidator))]
        public IResult Add(IFormFile file, CarImage carImage)
        {
            IResult result = BusinessRule.Run
                (CheckCarMaxImageLimit(carImage.CarId));

            var carImageResult = FileHelper.Upload(file);
            if (!carImageResult.Success)
            {
                return new ErrorResult(carImageResult.Message);
            }
            carImage.ImagePath = carImageResult.Message;
            carImage.Date = DateTime.Now;
            _carImageDal.Add(carImage);
            return new SuccessResult(Messages.ImageAdded);
        }

        public IResult Update(IFormFile file, CarImage carImage)
        {
            var image = _carImageDal.Get(cI => cI.ImageId == carImage.ImageId);
            if (image == null)
            {
                return new ErrorResult(Messages.ImageNotFound);
            }
            var updated = FileHelper.Update(file, image.ImagePath);
            if (!updated.Success)
            {
                return new ErrorResult(updated.Message);
            }
            carImage.ImagePath = updated.Message;
            _carImageDal.Update(carImage);
            return new SuccessResult(Messages.ImageUpdated);
        }

        public IResult Delete(CarImage carImage)
        {
            var image = _carImageDal.Get(cI => cI.ImageId == carImage.ImageId);
            if (image != null)
            {
                FileHelper.Delete(image.ImagePath);
                _carImageDal.Delete(carImage);
                return new SuccessResult(Messages.ImageDeleted);
            }
            return new ErrorResult(Messages.ImageNotFound);
        }

        public IDataResult<List<CarImage>> GetAll()
        {
            return new SuccessDataResult<List<CarImage>>(_carImageDal.GetAll(), Messages.AllCarImagesListed);
        }

        public IDataResult<List<CarImage>> GetAllByCarId(int carId)
        {
            IResult result = BusinessRule.Run(CarImageCheck(carId));
            if (result != null)
            {
                return new ErrorDataResult<List<CarImage>>(result.Message);
            }
            return new SuccessDataResult<List<CarImage>>(CarImageCheck(carId).Data, Messages.CarImagesListedByCarId);
        }

        public IDataResult<CarImage> GetById(int carImageId)
        {
            return new SuccessDataResult<CarImage>(_carImageDal.Get(cI => cI.ImageId == carImageId));
        }

        //private metotlar
        private IDataResult<List<CarImage>> CarImageCheck(int carId)
        {
            try
            {
                string path = @"\images\default.jpg";
                var result = _carImageDal.GetAll(c => c.CarId == carId).Any();
                if (!result)
                {
                    List<CarImage> carimage = new List<CarImage>();
                    carimage.Add(new CarImage { CarId = carId, ImagePath = path, Date = DateTime.Now });
                    return new SuccessDataResult<List<CarImage>>(carimage);
                }
            }
            catch (Exception exception)
            {
                return new ErrorDataResult<List<CarImage>>(exception.Message);
            }

            return new SuccessDataResult<List<CarImage>>(_carImageDal.GetAll(p => p.CarId == carId).ToList());
        }
        private IResult CheckCarMaxImageLimit(int id)
        {
            var result = _carImageDal.GetAll(c => c.CarId == id).Count;
            if (result > 5)
            {
                return new ErrorResult("Limit doldu.");
            }
            return new SuccessResult();
        }

    }
}
