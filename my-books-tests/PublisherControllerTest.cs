using Microsoft.EntityFrameworkCore;
using my_books.Controllers;
using my_books.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_books_tests
{
    class PublisherControllerTest
    {
        private static DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(databaseName: "BookDbTest")
           .Options;


        AppDbContext context;
        PublishersController _publishersController;


        [OneTimeSetUp]
        public void Setup()
        {
            context = new AppDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            //SeedDatabase();
            //_publishersService = new PublishersService(context);
        }














        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }


    }
}
