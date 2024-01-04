using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using my_books.Controllers;
using my_books.Data;
using my_books.Data.Models;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_books_tests
{
    public class PublisherControllerTest
    {
        private static DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(databaseName: "PublisherDbControllerTest")
           .Options;


        AppDbContext context;
        PublishersService _publishersService;
        PublishersController _publishersController;

        [OneTimeSetUp]
        public void Setup()
        {
            context = new AppDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            SeedDatabase();
            _publishersService = new PublishersService(context);
            _publishersController = new PublishersController(_publishersService, new NullLogger<PublishersController>());
        }

        [Test, Order(1)]
        public void HTTPGET_GetAllPublishers_WithSortBy_WithSearchString_WithPageNumber_ReturnOk_Test()
        {
            IActionResult actionResult = _publishersController.GetAllPublisher("name_desc", "Publisher", 1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var actionResultData = (actionResult as OkObjectResult).Value as List<Publisher>;
            Assert.That(actionResultData.First().Name, Is.EqualTo("Publisher 6"));
            Assert.That(actionResultData.First().Id, Is.EqualTo(6));
            Assert.That(actionResultData.Count, Is.EqualTo(5));

            IActionResult actionResultSecondPage = _publishersController.GetAllPublisher("name_desc", "Publisher", 2);
            Assert.That(actionResultSecondPage, Is.TypeOf<OkObjectResult>());
            var actionResultDataSecondPage = (actionResultSecondPage as OkObjectResult).Value as List<Publisher>;
            Assert.That(actionResultDataSecondPage.First().Name, Is.EqualTo("Publisher 1"));
            Assert.That(actionResultDataSecondPage.First().Id, Is.EqualTo(1));
            Assert.That(actionResultDataSecondPage.Count, Is.EqualTo(1));
        }


        [Test, Order(2)]
        public void HTTPGET_GetPublisherById_ReturnOk_Test()
        {

            //Arrange
            int publisherId = 1;
            //Act
            IActionResult actionResult = _publishersController.GetPublisherById(publisherId);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var publisherData = (actionResult as OkObjectResult).Value as Publisher;
            //Assert
            Assert.That(publisherData.Name, Is.EqualTo("publisher 1").IgnoreCase);
            Assert.That(publisherData.Id, Is.EqualTo(1));
            Assert.That(publisherData, Is.Not.Null);
        }



        [Test, Order(3)]
        public void HTTPGET_GetPublisherById_ReturnNotFound_Test()
        {
            //Arrange
            int nonExistingPublisherId = 99;

            // Act
            IActionResult actionResult = _publishersController.GetPublisherById(nonExistingPublisherId);

            // Assert
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
        }

        [Test, Order(4)]
        public void HTTPPOST_AddPublisher_ReturnCreated_Test()
        {
            //Arrange
            var newPublisherVM = new PublisherVM()
            {
                Name = "New Publisher"
            };
            // Act
            IActionResult actionResult = _publishersController.AddPublisher(newPublisherVM);
            // Assert
            Assert.That(actionResult, Is.TypeOf<CreatedResult>());
        }

        [Test, Order(5)]
        public void HTTPPOST_AddPublisher_ReturnBadRequest_Test()
        {
            //Arrange
            var newPublisherVM = new PublisherVM()
            {
                Name = "123 New Publisher"
            };
            // Act
            IActionResult actionResult = _publishersController.AddPublisher(newPublisherVM);
            // Assert
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }


        [Test, Order(6)]
        public void HTTPDELETE_DeletePublisherById_ReturnOk_Test()
        {
            //Arrange
            int existingPublisherId = 1;
            //Act
            IActionResult actionResult = _publishersController.DeletePublisherById(existingPublisherId);
            //Assert
            Assert.That(actionResult, Is.TypeOf<OkResult>());
        }


        [Test, Order(7)]
        public void HTTPDELETE_DeletePublisherById_ReturnBadRequest_Test()
        {
            //Arrange
            int nonExistingPublisherId = 99;
            //Act
            IActionResult actionResult = _publishersController.DeletePublisherById(nonExistingPublisherId);
            //Assert
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }


        [Test, Order(8)]
        public void HTTPGET_GetPublisherData_ReturnOk_Test()
        {
            //Arrange
            int publisherId = 1;
            //Act
            IActionResult actionResult = _publishersController.GetPublisherData(publisherId);
            //Assert
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

        }






        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }


        private void SeedDatabase()
        {
            var publisher = new List<Publisher>
            {
                new Publisher()
                {
                    Id = 1,
                    Name = "Publisher 1"
                },
                new Publisher()
                {
                    Id = 2,
                    Name = "Publisher 2"
                },
                new Publisher()
                {
                    Id = 3,
                    Name = "Publisher 3"
                },
                new Publisher()
                {
                    Id = 4,
                    Name = "Publisher 4"
                },
                new Publisher()
                {
                    Id = 5,
                    Name = "Publisher 5"
                },
                new Publisher()
                {
                    Id = 6,
                    Name = "Publisher 6"
                }
            };
            context.Publishers.AddRange(publisher);
            context.SaveChanges();


        }












    }
}
