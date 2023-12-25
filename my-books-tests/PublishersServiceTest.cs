using Microsoft.EntityFrameworkCore;
using my_books.Data;
using my_books.Data.Models;
using my_books.Data.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace my_books_tests
{
    public class PublishersServiceTest
    {
        private static DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "BookDbTest")
            .Options;

        AppDbContext context;
        PublishersService publishersService;

        [OneTimeSetUp]
        public void Setup()
        {
            context = new AppDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            SeedDatabase();
            publishersService = new PublishersService(context);
        }

        [Test]
        public void GetAllPublishers_WithNoSortBy_WithNoSearchString_WithNoPageNumber_Test()
        {
            var result = publishersService.GetAllPublishers("", "", null);

            Assert.That(result.Count, Is.EqualTo(5));
            Assert.AreEqual(result.Count, 5);
        }


        [Test]
        public void GetAllPublishers_WithNoSortBy_WithNoSearchString_WithPageNumber_Test()
        {
            var result = publishersService.GetAllPublishers("", "", 2);

            Assert.That(result.Count, Is.EqualTo(1));
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