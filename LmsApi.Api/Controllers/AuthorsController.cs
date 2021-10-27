using AutoMapper;
using LmsApi.Core.Dtos;
using LmsApi.Core.Entities;
using LmsApi.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsApi.Api.Controllers
{
    [Route("api/literatures/{literatureId}/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AuthorsController(IUnitOfWork uow, IMapper mapper)
        {
            unitOfWork = uow ?? throw new ArgumentNullException(nameof(uow));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        //api/literatures/{literatureId}/authors/collection
        [HttpPost("collection")]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> PostCollection([FromRoute]int literatureId, [FromBody]IEnumerable<AuthorForCreateUpdateDto> dto)
        {
            try
            {
                var literature = await unitOfWork.LiteraturesRepo.GetAsync(literatureId);
                if (literature == null) return BadRequest("Literature does not exist");

                var author = mapper.Map<IEnumerable<Author>>(dto);
                foreach (var item in author)
                {
                    item.Literatures.Add(literature);
                    unitOfWork.AuthorsRepo.Add(item);
                }

                if (await unitOfWork.CompleteAsync())
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure...");
            }
            return BadRequest("Failed to save new author");
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAll(int literatureId)
        {
            try
            {
                var result =
                    await unitOfWork.AuthorsRepo.GetAllAsync(filter: a => a.Literatures.FirstOrDefault().Id == literatureId);

                return Ok(mapper.Map<IEnumerable<AuthorDto>>(result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure...");
            }
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDto>> Post([FromRoute]int literatureId, [FromBody]AuthorForCreateUpdateDto dto)
        {
            try
            {
                var literature = await unitOfWork.LiteraturesRepo.GetAsync(literatureId);
                if (literature == null) return BadRequest("Literature does not exist");

                var author = mapper.Map<Author>(dto);
                author.Literatures.Add(literature);
                unitOfWork.AuthorsRepo.Add(author);

                if (await unitOfWork.CompleteAsync())
                {
                    return CreatedAtAction(nameof(Get), new { literatureId = literature.Id, id = author.Id }, mapper.Map<AuthorDto>(author));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure...");
            }
            return BadRequest("Failed to save new author");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AuthorDto>> Get(int literatureId, int id)
        {
            try
            {
                var result =
                    await unitOfWork.AuthorsRepo.GetAsync(id, filter: a => a.Literatures.FirstOrDefault().Id == literatureId);

                if (result == null) return NotFound();

                return Ok(mapper.Map<AuthorDto>(result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure...");
            }
        }

        

        


        [HttpPut("{id:int}")]
        public async Task<ActionResult<AuthorDto>> Put(int literatureId, int id, AuthorForCreateUpdateDto dto)
        {
            try
            {
                var author = await unitOfWork.AuthorsRepo.GetAsync(id);
                if (author == null) return NotFound("Could not find the author");

                mapper.Map(dto, author);

                if (await unitOfWork.CompleteAsync())
                {
                    return Ok(mapper.Map<AuthorDto>(author));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure...");
            }
            return BadRequest("Failed to update author");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int literatureId, int id)
        {
            try
            {
                var existingAuthor = await unitOfWork.AuthorsRepo.FindAsync(id);
                if (existingAuthor == null) return NotFound($"Could not find the author with id: {id}.");

                unitOfWork.AuthorsRepo.Delete(existingAuthor);

                if (await unitOfWork.CompleteAsync())
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure.");
            }
            return BadRequest("Failed to delete the author");
        }

    }
}
