using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData
{
    public interface ICheckOut
    {
        CheckOut GetByID(int id);
        CheckOut GetLatestCheckOut(int assetID);
        

        void Add(CheckOut newCheckout);
        void CheckOutItem(int assetID, int libraryCardID);
        void CheckInItem(int assetID, int libraryCardID);
        void MarkLost(int id);
        void MarkFound(int id);
        void PlaceHold(int assetID, int libraryCardID);


        IEnumerable<CheckOut> GetAll();
        IEnumerable<CheckOutHistory> GetCheckOutHistorie(int id);
        IEnumerable<Hold> GetCurrentHolds(int id);


        string GetCurrentHoldPatronName(int id);
        string GetCurrentCheckOutPatron(int assetID);
        DateTime GetCurrentHoldPlaced(int id);
        bool IsCheckedOut(int id);
    }
}
