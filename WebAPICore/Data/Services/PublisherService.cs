using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPICore.Data.Models;
using WebAPICore.Data.ViewModels;

namespace WebAPICore.Data.Services
{
    public class PublisherService
    {
        private AppDbContext _context;

        public PublisherService(AppDbContext context)
        {
            _context = context;
        }

        public void AddPulisher(PublisherVM publisher)
        {
            var _publisher = new Publisher() 
            { 
                Name = publisher.Name
            };

            _context.Publishers.Add(_publisher);
            _context.SaveChanges();

        }

        public PublisherWithBooksAndAuthorVM GetPublisherData(int publisherId)
        {
            var _publisherData = _context.Publishers.Where(n => n.Id == publisherId)
                .Select(x => new PublisherWithBooksAndAuthorVM()
                {
                    Name = x.Name,
                    BookAuthors = x.Books.Select(x=> new BookAuthorVM()
                    { 
                        BookName = x.Title,
                        BookAuthors = x.Book_Authors.Select(x=>x.Author.FullName).ToList()
                    }).ToList()
                }).FirstOrDefault();

            return _publisherData;
        }

        internal void DeletePublisherById(int id)
        {
            var _publisher = _context.Publishers.FirstOrDefault(n => n.Id==id);
            
            if(_publisher!=null)
            {
                _context.Publishers.Remove(_publisher);
                _context.SaveChanges();
            }

        }
    }
}
