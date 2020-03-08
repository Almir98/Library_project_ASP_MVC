using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_project.ViewModels.Catalog
{
    public class AssetIndexModel
    {
        public IEnumerable<AssetIndexListingModel> ListAssets { get; set; }     // ova samo sadrzi listu od prethodnog view modela
    }
}
    