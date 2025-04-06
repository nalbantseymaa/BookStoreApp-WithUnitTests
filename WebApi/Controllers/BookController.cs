
using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using WebApi.BookOperations.Command;
using WebApi.BookOperations.Query;
using WebApi.BookOperations.Validation;
using WebApi.DBOperations;
using static WebApi.BookOperations.Command.CreateBookCommand;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]s")]
    public class BookController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        public BookController(BookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            GetBooksQuery query = new GetBooksQuery(_context, _mapper);
            var result = query.Handle();
            return Ok(result);
        }

        //1 id li kitabın bilgilerini getiren endpointe yapılan request
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            BookDetailViewModel result = null;

            GetBookDetailQuery query = new GetBookDetailQuery(_context, _mapper);
            query.BookId = id;

            GetBookDetailQueryValidator validator = new GetBookDetailQueryValidator();

            validator.ValidateAndThrow(query);

            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] CreateBookModel newBook)
        {
            CreateBookCommand command = new CreateBookCommand(_context, _mapper);

            command.Model = newBook;
            CreateBookCommandValidator validator = new CreateBookCommandValidator();

            validator.ValidateAndThrow(command);
            command.Handle();
            // HANDLE  metodu yalnızca doğrulama işlemi başarılı olduğunda çalışır.

            // if (!result.IsValid)
            // {
            //     foreach (var failure in result.Errors)
            //         Console.WriteLine("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
            // }
            // else
            //     command.Handle();
            // }
            // catch (System.Exception ex)
            // {
            //     return BadRequest(ex.Message);
            // }
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] UpdateBookCommand.UpdateBookModel updatedBookModel)
        {
            UpdateBookCommand command = new UpdateBookCommand(_context);
            command.BookId = id;
            command.Model = updatedBookModel;
            UpdateBookCommandValidator validator = new UpdateBookCommandValidator();

            validator.ValidateAndThrow(command);
            command.Handle();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {

            DeleteBookCommand command = new DeleteBookCommand(_context);
            command.BookId = id;

            DeleteBookCommandValidator validator = new DeleteBookCommandValidator();
            validator.ValidateAndThrow(command);
            command.Handle();
            return Ok();
        }
    }

}
