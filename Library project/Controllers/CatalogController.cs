using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryData;
using LibraryService;
using Library_project.ViewModels.Catalog;
using Library_project.ViewModels.CheckOut;

namespace Library_project.Controllers
{
    public class CatalogController : Controller
    {
        private ILibraryAsset _interface;
        private ICheckOut _checkOutInterface;
        public CatalogController(ILibraryAsset asset,ICheckOut checkout)
        {
            _interface = asset;
            _checkOutInterface = checkout;
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

        public IActionResult Detail(int id)
        {
            var asset = _interface.GetById(id);

            var currentHolds = _checkOutInterface.GetCurrentHolds(id).Select(e => new AssetHoldModel
            {
                HoldPlaced=_checkOutInterface.GetCurrentHoldPlaced(e.ID),
                PatronName=_checkOutInterface.GetCurrentHoldPatronName(e.ID)
            });

            var model = new Catalog_DetailVM
            {
                 ID=asset.ID,
                 title=asset.Title,
                 authorOrDirector=_interface.GetAuthorOrDirector(id),
                 type= _interface.GetType(id),
                year=asset.Year,
                ISBN= _interface.GetISBN(id),
                deweyCallNumber=_interface.GetDeweyIndex(id),
                status=asset.Status.Name,
                cost=asset.Cost,
                currentLocation=_interface.GetCurrentLocation(id).Name,
                imageUrl=asset.ImageUrl,

                CheckOutHistory=_checkOutInterface.GetCheckOutHistorie(id),
                LatestCheckOut=_checkOutInterface.GetLatestCheckOut(id),
                patronName=_checkOutInterface.GetCurrentCheckOutPatron(id),
                CurrenHolds=currentHolds
            };
            return View(model);
        }

        public IActionResult CheckOut(int assetid)   // assetID
        {
            var asset = _interface.GetById(assetid);

            var model = new CheckOutModel
            {
                AssetID=asset.ID,
                Title=asset.Title,
                ImageUrl=asset.ImageUrl,
                LibraryCardID="",
                IsCheckedOut=_checkOutInterface.IsCheckedOut(assetid)
            };
            return View(model);
        }


        public IActionResult MarkLost(int assetid)
        {
            _checkOutInterface.MarkLost(assetid);
            return RedirectToAction("Detail",new { id = assetid });
        }

        public IActionResult MarkFound(int assetid)
        {
            _checkOutInterface.MarkFound(assetid);
            return RedirectToAction("Detail", new { id = assetid });
        }

        public IActionResult Hold(int id)
        {
            var asset = _interface.GetById(id);

            var model = new CheckOutModel
            {
                AssetID = asset.ID,
                Title = asset.Title,
                ImageUrl = asset.ImageUrl,
                LibraryCardID = "",
                IsCheckedOut = _checkOutInterface.IsCheckedOut(id),
                HoldCount=_checkOutInterface.GetCurrentHolds(id).Count()
            };
            return View(model);
        }


        [HttpPost]
        public IActionResult PlaceCheckOut(int assetID,int libraryCardID)
        {
            _checkOutInterface.CheckInItem(assetID, libraryCardID);
            return RedirectToAction("Detail",new { id=assetID});
        }

        [HttpPost]
        public IActionResult PlaceHold(int assetID, int libraryCardID)
        {
            _checkOutInterface.PlaceHold(assetID, libraryCardID);
            return RedirectToAction("Detail", new { id = assetID });
        }
    };
}
