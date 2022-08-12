using LibraryService.Web.Models;
using LibraryService.Web.ViewModels;
using LibraryServiceReference;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryService.Web.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ILogger<LibraryController> _logger;

        public LibraryController(ILogger<LibraryController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(SearchTypeEnum searchType, string searchString)
        {
            //класс (в данном случае LibraryWebServiceSoapClient) берется из Reference в Connected Services
            LibraryWebServiceSoapClient libraryWebServiceSoapClient =
                new LibraryWebServiceSoapClient(LibraryWebServiceSoapClient.EndpointConfiguration.LibraryWebServiceSoap12);

            var bookCategoryViewModel = new BookCategoryViewModel
            {
                Books = new BookModel[] { }
            };

            if(!string.IsNullOrEmpty(searchString) && searchString.Length > 2)
            {
                switch (searchType)
                {
                    case SearchTypeEnum.Title:
                        bookCategoryViewModel.Books = libraryWebServiceSoapClient.GetBooksByTitle(searchString);
                        break;
                    case SearchTypeEnum.Author:
                        bookCategoryViewModel.Books = libraryWebServiceSoapClient.GetBooksByAuthor(searchString);
                        break;
                    case SearchTypeEnum.Category:
                        bookCategoryViewModel.Books = libraryWebServiceSoapClient.GetBooksByCategory(searchString);
                        break;
                }
            }

            return View(bookCategoryViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
