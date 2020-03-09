using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_project.ViewModels.CheckOut
{
    public class CheckOutModel
    {
        public string LibraryCardID { get; set; }
        public string Title { get; set; }
        public int AssetID { get; set; }
        public string ImageUrl { get; set; }
        public int HoldCount { get; set; }
        public bool IsCheckedOut { get; set; }
    }
}
