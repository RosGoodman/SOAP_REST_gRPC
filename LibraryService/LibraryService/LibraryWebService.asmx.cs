﻿using LibraryService.Models;
using LibraryService.Services;
using LibraryService.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace LibraryService
{
    /// <summary>
    /// Сводное описание для LibraryWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    // [System.Web.Script.Services.ScriptService]
    public class LibraryWebService : System.Web.Services.WebService
    {
        private readonly ILibraryRepositoryService _libraryRepositoryService;

        public LibraryWebService()
        {
            _libraryRepositoryService = new LibraryRepository(new LibraryDatabaseContext());
        }

        [WebMethod]
        public List<BookModel> GetBooksByTitle(string title)
        {
            return _libraryRepositoryService.GetByTitle(title).ToList();
        }

        [WebMethod]
        public List<BookModel> GetBooksByAuthor(string author)
        {
            return _libraryRepositoryService.GetByAuthor(author).ToList();
        }

        [WebMethod]
        public List<BookModel> GetBooksByCategory(string category)
        {
            return _libraryRepositoryService.GetByCategory(category).ToList();
        }
    }
}
