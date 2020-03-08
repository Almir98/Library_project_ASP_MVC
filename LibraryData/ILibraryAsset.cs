using LibraryData.Models;
using System.Collections.Generic;

namespace LibraryService
{
    public interface ILibraryAsset
    {
        IEnumerable<LibraryAsset> GetAllAssets();
        LibraryAsset GetById(int id);
        
        void AddAssets(LibraryAsset newAsset);

        string GetAuthorOrDirector(int id);

        string GetDeweyIndex(int id);
        string GetType(int id);
        string GetTitle(int id);
        string GetISBN(int id);

        LibraryBranch GetCurrentLocation(int id);
    }
}