using BataCMS.Data;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Repositories
{
    public class ImageRepository : IImageRepository
    {

        private readonly AppDbContext _appDbContext;
        public ImageRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void DeleteImage(Image image)
        {
            _appDbContext.Image.Remove(image);
            _appDbContext.SaveChanges();


        }
    }
}
