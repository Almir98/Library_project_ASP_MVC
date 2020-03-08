using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryData;
using LibraryService;
using Library_project.ViewModels.Catalog;

namespace Library_project.Controllers
{
    public class CatalogController : Controller
    {
        private ILibraryAsset _interface;
        public CatalogController(ILibraryAsset asset)
        {
            _interface = asset;
        }


        public IActionResult Index()
        {
            var models = _interface.GetAllAssets();

            var listingResult = models.Select(e => new AssetIndexListingModel
            {
                ID=e.ID,
                imageUrl=e.ImageUrl,
                title=e.Title,
                authorOrDirector=_interface.GetAuthorOrDirector(e.ID),
                deweyCallNumber=_interface.GetDeweyIndex(e.ID),
                type=_interface.GetType(e.ID)

            });

            var model = new AssetIndexModel()
            {
                ListAssets=listingResult
            };
            return View(model);
        }





























    }
}