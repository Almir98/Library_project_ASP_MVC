using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_project.ViewModels.Catalog
{
    public class Catalog_DetailVM
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string authorOrDirector { get; set; }
        public string type { get; set; }
        public int year { get; set; }
        public string ISBN { get; set; }
        public string deweyCallNumber { get; set; }
        public string status { get; set; }
        public decimal cost { get; set; }
        public string currentLocation { get; set; }
        public string imageUrl { get; set; }
        public string patronName { get; set; }
        
        //public CheckOut LatestCheckOut { get; set; }

        public IEnumerable<CheckOutHistory> CheckOutHistory { get; set; }
        public IEnumerable<AssetHoldModel> CurrenHolds { get; set; }
    }

    public class AssetHoldModel
    {
        public string PatronName { get; set; }
        public DateTime HoldPlaced { get; set; }
    }

}
