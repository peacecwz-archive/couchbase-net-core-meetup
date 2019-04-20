using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CouchbaseNetDemo.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CouchbaseNetDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _bookRepository.GetById(id);
            
            return result == null 
                ? (IActionResult) NotFound() 
                : Ok(result);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BookEntity book)
        {
            var isExists = await _bookRepository.IsExists(book.Id);
            if (isExists)
            {
                return Conflict();
            }

            var isSuccess = await _bookRepository.Add(book);
            return isSuccess
                ? StatusCode((int) HttpStatusCode.Created)
                : StatusCode((int) HttpStatusCode.InternalServerError);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] BookEntity book)
        {
            var isSuccess = await _bookRepository.Upsert(id, book);
           
            return isSuccess 
                ? NoContent() 
                : StatusCode((int) HttpStatusCode.InternalServerError);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(string id, [FromBody] JsonPatchDocument request)
        {
            //TODO: Refactor
            var result = await _bookRepository.Patch(id, request);
            return result 
                ? NoContent() 
                : StatusCode((int) HttpStatusCode.InternalServerError);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var isSuccess = await _bookRepository.Delete(id);
            return isSuccess 
                ? NoContent() 
                : StatusCode((int) HttpStatusCode.InternalServerError);
        }
    }
}