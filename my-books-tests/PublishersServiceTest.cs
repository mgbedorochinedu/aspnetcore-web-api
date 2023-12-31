using Microsoft.EntityFrameworkCore;
using my_books.Data;
using my_books.Data.Models;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using my_books.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace my_books_tests
{
    public class PublishersServiceTest
    {
        private static DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "BookDbTest")
            .Options;

        AppDbContext context;
        PublishersService _publishersService;

        [OneTimeSetUp]
        public void Setup()
        {
            context = new AppDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            SeedDatabase();
            _publishersService = new PublishersService(context);
        }



        [Test, Order(1)]
        public void GetAllPublishers_WithNoSortBy_WithNoSearchString_WithNoPageNumber_Test()
        {
            var result = _publishersService.GetAllPublishers("", "", null);

            Assert.That(result.Count, Is.EqualTo(5));
            Assert.AreEqual(result.Count, 5);
        }


        [Test, Order(2)]
        public void GetAllPublishers_WithNoSortBy_WithNoSearchString_WithPageNumber_Test()
        {
            // Act
            var result = _publishersService.GetAllPublishers("", "", 2);
            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public void GetAllPublishers_WithNoSortBy_WithSearchString_WithNoPageNumber_Test()
        {

            // Arrange
            string searchString = "Publisher 4";

            // Act
            var result = _publishersService.GetAllPublishers("", searchString, null);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.FirstOrDefault().Name, Is.EqualTo("Publisher 4"));
            CollectionAssert.AreEquivalent(result.Where(p => p.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList(), result);    
        }


        [Test, Order(4)]
        public void GetAllPublishers_WithSortBy_WithSearchString_WithNoPageNumber_Test()
        {
            //Act
            var result = _publishersService.GetAllPublishers("name_desc", "", null);
            //Assert
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.FirstOrDefault().Name, Is.EqualTo("Publisher 6"));
            Assert.That(result.LastOrDefault().Name, Is.EqualTo("Publisher 2"));
            //Assert DescendingOrder
            CollectionAssert.AreEqual(result.OrderByDescending(p => p.Name).ToList(), result);
        }

        [Test, Order(5)]
        public void GetPublisherId_ExistingId_ReturnsPublisher_Test()
        {
            //Arrange
            int exisitingId = 1;
            string publisherName = "Publisher 1";

            //Act
            var result = _publishersService.GetPublisherById(exisitingId);

            //Assert
            Assert.That(result.Name, Is.EqualTo(publisherName));
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(exisitingId));
            Assert.AreEqual(exisitingId, result.Id);

        }

        [Test, Order(6)]
        public void GetPublisherId_NonExistingId_ReturnsNull_Test()
        {
            //Arrange
            int nonExisitingId = 999;

            //Act
            var result = _publishersService.GetPublisherById(nonExisitingId);

            //Assert
            Assert.IsNull(result);
            Assert.That(result, Is.Null);
        }

        [Test, Order(7)]
        public void AddPublisher_WithFailure_Test()
        {
            //Act
            var newPublisher = new PublisherVM()
            {
                Name = "123 With Exception"
            };

            //Assert
            Assert.That(() => _publishersService.AddPublisher(newPublisher),
                Throws.Exception.TypeOf<PublisherNameException>().With.Message.EqualTo("Name starts with number"));

        }

        [Test, Order(8)]
        public void AddPublisher_WithoutFailure_Test()
        {
            //Arrange
            var newPublisher = new PublisherVM()
            {
                Name = "Without Exception"
            };
            //Act
            var result = _publishersService.AddPublisher(newPublisher);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Does.StartWith("Without"));
            Assert.That(result.Id, Is.Not.Null);
        }


        [Test, Order(9)]
        public void GetPublisherData_Test()
        {
            //Arrange
            int exisitingPublisherId = 1;
            string publisherName = "Publisher 1";
            string firstBook = "Book 1 Title";

            //Act
            var result = _publishersService.GetPublisherData(exisitingPublisherId);

            //Assert
            Assert.That(result.Name, Is.EqualTo(publisherName));
            Assert.That(result.BookAuthors, Is.Not.Empty);
            Assert.That(result.BookAuthors.Count, Is.GreaterThan(0));

            var firstBookName = result.BookAuthors.OrderBy(x => x.BookName).FirstOrDefault().BookName;
            Assert.That(firstBookName, Is.EqualTo(firstBook));
            Assert.That(result, Is.Not.Null);

        }



        [Test, Order(10)]
        public void DeletePublisherById_ExistingId_RemovesPublisher_Test()
        {
            //Arrange
            int existingPublisherId = 1;

            //Act
             _publishersService.DeletePublisherById(existingPublisherId);

            //Assert
            var deletedPublisher = context.Publishers.FirstOrDefault(x => x.Id == existingPublisherId);
            Assert.IsNull(deletedPublisher, $"Publisher with ID {existingPublisherId} should have been deleted.");

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

            var authors = new List<Author>()
            {
                new Author()
                {
                    Id = 1,
                    FullName = "Author 1"

                },
                new Author()
                {
                    Id = 2,
                    FullName = "Author 2"
                }
            };
            context.Authors.AddRange(authors);

            var books = new List<Book>()
            {
                new Book()
                {
                    Id = 1,
                    Title = "Book 1 Title",
                    Description = "Book 1 Description",
                    IsRead = false,
                    Genre = "Comedy",
                    CoverUrl = "https://www.chinedu.com",
                    DateAdded = DateTime.Now.AddDays(-10),
                    PublisherId = 1
                },
                  new Book()
                {
                    Id = 2,
                    Title = "Book 2 Title",
                    Description = "Book 2 Description",
                    IsRead = false,
                    Genre = "Music",
                    CoverUrl = "https://www.mgbedoro.com",
                    DateAdded = DateTime.Now.AddDays(-10),
                    PublisherId = 1
                }
            };
            context.Books.AddRange(books);

            var book_authors = new List<Book_Author>()
            {
                new Book_Author()
                {
                    Id = 1,
                    BookId = 1,
                    AuthorId = 1
                },
                 new Book_Author()
                {
                    Id = 2,
                    BookId = 1,
                    AuthorId = 2
                },
                  new Book_Author()
                {
                    Id = 3,
                    BookId = 2,
                    AuthorId = 2
                }
            };
            context.Book_Authors.AddRange(book_authors);


            context.SaveChanges();
        }

    }
}