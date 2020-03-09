using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryService
{
    public class CheckOutService : ICheckOut
    {
        protected LibraryContext _context;

        // constuctor for database

        public CheckOutService(LibraryContext context)
        {
            _context = context;
        }


        public void Add(CheckOut newCheckout)
        {
            _context.Add(newCheckout);
            _context.SaveChanges();
        }
        

        public IEnumerable<CheckOut> GetAll()
        {
            return _context.CheckOuts;
        }

        public CheckOut GetByID(int id)
        {
            return _context.CheckOuts.FirstOrDefault(e => e.ID == id);
        }

        public IEnumerable<CheckOutHistory> GetCheckOutHistorie(int id)
        {
            return _context.CheckOutHistories.Include(e => e.LibraryAsset).Include(e=>e.LibraryCard).Where(e => e.LibraryAsset.ID == id);
        }

        public string GetCurrentHoldPatronName(int holdid)
        {
            var hold = _context.Holds.Include(e => e.LibraryAsset).Include(e => e.LibraryCard).FirstOrDefault(e => e.ID == holdid);

            var cardID = hold?.LibraryCard.ID;
            var patron = _context.Patron.Include(e => e.LibraryCard).FirstOrDefault(e => e.LibraryCard.ID == cardID);

            return patron?.FirstName + " " + patron?.LastName;
        }

        public DateTime GetCurrentHoldPlaced(int id)
        {
            return _context.Holds.Include(e => e.LibraryAsset).Include(e => e.LibraryCard).FirstOrDefault(e => e.ID == id).HoldPlaced;
        }

        public IEnumerable<Hold> GetCurrentHolds(int id)
        {
            return _context.Holds.Include(e=>e.LibraryAsset).Where(e => e.ID == id);
        }

        public void MarkFound(int id)
        {
            UpdateAssetsStatus(id,"Available");
            RemoveExistingCheckOuts(id);
            CloseExistingCheckOutHistory(id);

            _context.SaveChanges();
        }

        private void UpdateAssetsStatus(int id, string v)
        {
            var item = _context.LibraryAssets.Where(e => e.ID == id).FirstOrDefault();
            _context.Update(item);

            item.Status = _context.Statuses.FirstOrDefault(e => e.Name == "Available");
        }

        private void CloseExistingCheckOutHistory(int id)
        {
            var history = _context.CheckOutHistories.FirstOrDefault(e => e.LibraryAsset.ID == id && e.CheckedIn == null);
            if (history != null)
            {
                _context.Remove(history);
                history.CheckedIn = DateTime.Now;
            }
        }

        private void RemoveExistingCheckOuts(int id)
        {
            var check = _context.CheckOuts.FirstOrDefault(e => e.LibraryAsset.ID == id);
            if (check != null)
            {
                _context.Remove(check);
            }
        }

        public void MarkLost(int id)
        {
            UpdateAssetsStatus(id, "Lost");
            _context.SaveChanges();
        }

        public void PlaceHold(int assetID, int libraryCardID)
        {
            var asset = _context.LibraryAssets.FirstOrDefault(e => e.ID == assetID);
            var card = _context.LibraryCards.FirstOrDefault(e => e.ID == libraryCardID);

            if (asset.Status.Name == "Available")
            {
                UpdateAssetsStatus(assetID, "On Hold");
            }

            var hold = new Hold
            {
                HoldPlaced=DateTime.Now,
                LibraryAsset=asset,
                LibraryCard=card
            };
            _context.Add(hold);
            _context.SaveChanges();
        }


        public void CheckInItem(int assetID, int libraryCardID)
        {
            var item = _context.LibraryAssets.FirstOrDefault(e => e.ID == assetID);

            RemoveExistingCheckOuts(assetID);
            CloseExistingCheckOutHistory(assetID);

            var currentHolds = _context.Holds.Include(e => e.LibraryAsset).Include(e => e.LibraryCard).Where(e => e.LibraryAsset.ID == assetID);

            if(currentHolds.Any())
            {
                CheckToEarliestHold(assetID, currentHolds);
            }
            UpdateAssetsStatus(assetID, "Available");
            _context.SaveChanges();
        }

        private void CheckToEarliestHold(int assetID, IQueryable<Hold> currentHolds)
        {
            var earliest = currentHolds.OrderBy(e => e.HoldPlaced).FirstOrDefault();

            var card = earliest.LibraryCard;

            _context.Remove(earliest);
            _context.SaveChanges();
            CheckOutItem(assetID,card.ID);

        }

        public void CheckOutItem(int assetID, int libraryCardID)
        {
            if (IsCheckedOut(assetID))
            {
                return;
            }
            var item = _context.LibraryAssets.FirstOrDefault(e => e.ID == assetID);

            UpdateAssetsStatus(assetID, "Checked Out");

            var librarayCard = _context.LibraryCards.Include(e=>e.CheckOuts).FirstOrDefault(e => e.ID == libraryCardID);

            var date = DateTime.Now;
            var checkout = new CheckOut
            {
                LibraryAsset = item,
                LibraryCard = librarayCard,
                Since = date,
                Until = GetDefaultCheckOutTime(date)
            };
            _context.Add(checkout);

            var checkOutHistory = new CheckOutHistory
            {
                LibraryAsset=item,
                LibraryCard=librarayCard,
                CheckedOut=date
            };
            _context.Add(checkOutHistory);
            _context.SaveChanges();
        }

        private DateTime GetDefaultCheckOutTime(DateTime date)
        {
            return date.AddDays(30);
        }

        public bool IsCheckedOut(int assetID)
        {
            var isChecked = _context.CheckOuts.Where(e => e.LibraryAsset.ID == assetID).Any();
            return isChecked;
        }

        public CheckOut GetLatestCheckOut(int assetID)
        {
            return _context.CheckOuts.Where(e => e.LibraryAsset.ID == assetID).OrderByDescending(e=>e.Since).FirstOrDefault();
        }

        public string GetCurrentCheckOutPatron(int assetID)
        {
            var checkOut = _context.CheckOuts.Include(e => e.LibraryAsset).Include(e => e.LibraryCard).FirstOrDefault(e => e.LibraryAsset.ID == assetID);

            if (checkOut == null)
            {
                return "";
            }

            var cardID = checkOut.LibraryCard.ID;

            var patron = _context.Patron.Include(e => e.LibraryCard).Where(e => e.LibraryCard.ID == cardID).FirstOrDefault();

            return patron.FirstName + " " + patron.LastName;
        }

    }
}
