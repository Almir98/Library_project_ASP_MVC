using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_project.ViewModels.Catalog
{
    public class AssetIndexListingModel
    {
        public int ID { get; set; }
     
        public string imageUrl { get; set; }
        public string title { get; set; }

        public string authorOrDirector { get; set; }
        public string type { get; set; }
        public string deweyCallNumber { get; set; }
        public string numberOfCopies { get; set; }
    }
}
