using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryService
{
    public class LibraryAssetService : ILibraryAsset
    {
        // constuctor for database
        private LibraryContext _context;

        public LibraryAssetService(LibraryContext context)
        {
            _context = context;
        }


        // implementacija definicija funkcija iz interfejsa
        public void AddAssets(LibraryAsset newAsset)
        {
            _context.Add(newAsset);
            _context.SaveChanges();
        }

        public IEnumerable<LibraryAsset> GetAllAssets()
        {
            return _context.LibraryAssets.Include(a => a.Status).Include(a => a.Location);
        }

        
        public LibraryAsset GetById(int id)
        {
            return _context.LibraryAssets.Include(e => e.Location).Include(e => e.Status).FirstOrDefault(e => e.ID == id);
        }

        public LibraryBranch GetCurrentLocation(int id)
        {
            return GetById(id).Location;
        }

        public string GetDeweyIndex(int id)
        {
            // Discriminator

            if (_context.Books.Any(e => e.ID == id))
            {
                return _context.Books.FirstOrDefault(e => e.ID == id).DeweyIndex;
            }
            else
                return "";
        } 

        public string GetISBN(int id)
        {
            if (_context.Books.Any(e => e.ID == id))
            {
                return _context.Books.FirstOrDefault(e => e.ID == id).ISBN;
            }
            else
                return "";
        }

        public string GetTitle(int id)
        {
            return _context.LibraryAssets.FirstOrDefault(e => e.ID == id).Title;
        }

        public string GetType(int id)
        {
            var isBook= _context.LibraryAssets.OfType<Book>().Where(e => e.ID == id).Any();

            if (isBook)
                return "Book";
            else
                return "Video";
        }

        public string GetAuthorOrDirector(int id)
        {
            var isBook = _context.LibraryAssets.OfType<Book>().Where(e => e.ID == id).Any();     // if is book than is Author

            var isVideo = _context.LibraryAssets.OfType<Book>().Where(e => e.ID == id).Any();    // if is video than is director

            if (isBook)
                return _context.Books.FirstOrDefault(e => e.ID == id).Author;
            else if(isVideo)
                return _context.Videos.FirstOrDefault(e => e.ID == id).Director;
            else
                return "Unknown";
        }
    }
}
