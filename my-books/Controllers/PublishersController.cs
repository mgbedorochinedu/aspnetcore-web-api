using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using my_books.Data.Models;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using my_books.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private PublishersService _publishersService;
        private readonly ILogger<PublishersController> _logger;

        public PublishersController(PublishersService publishersService, ILogger<PublishersController> logger)
        {
            _publishersService = publishersService;
            _logger = logger;
        }

        //Get All Publisher Controller
        [HttpGet("get-all-publishers")]
        public IActionResult GetAllPublisher(string sortBy, string searchString, int pageNumber)
        {
            //throw new Exception("This is an Exception thrown in GetAllPublisher");
            try
            {
                _logger.LogInformation("This is just a log in GetAllPublisher");
                var _result = _publishersService.GetAllPublishers(sortBy, searchString, pageNumber);
                return Ok(_result);
            }
            catch(Exception)
            {
                return BadRequest("Sorry, we could not load the publishers");
            }
        }

        //Add Publisher Controller
        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody] PublisherVM publisher)
        {
            try
            {
                var newPublisher = _publishersService.AddPublisher(publisher);
                return Created(nameof(AddPublisher), newPublisher);
            }
            //We use PublisherNameException catch first, because it is more specific exception type
            catch(PublisherNameException ex)
            {
                return BadRequest($"{ex.Message}, Publisher name: {ex.PublisherName}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Get Publisher By Id Controller
        [HttpGet("get-publisher-by-id/{id}")]
        public Publisher GetPublisherById(int id)
        {
            var _response = _publishersService.GetPublisherById(id);

            if(_response != null)
            {
                //return Ok(_response);
                return _response;
            }
            else
            {
                //return NotFound();
                return null;
            }
        }

        //Get Publisher Books With Authors Controller
        [HttpGet("get-publisher-books-with-authors/{id}")]
        public IActionResult GetPublisherData(int id)
        {
            var _response = _publishersService.GetPublisherData(id);
            return Ok(_response);
        }

        //Deleting Relational Data - Publisher Controller
        [HttpDelete("delete-publisher-by-id/{id}")]
        public IActionResult DeletePublisherById(int id)
        {
            try
            {
                _publishersService.DeletePublisherById(id);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
