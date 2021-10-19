using AutoMapper;
using LmsApi.Core.Dtos;
using LmsApi.Core.Entities;
using LmsApi.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsApi.Api.Controllers
{
    [Route("api/literatures")]
    [ApiController] // By default it's trying to do a body binding [FromBody]...
    public class LiteraturesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public LiteraturesController(IUnitOfWork uow, IMapper mapper)
        {
            unitOfWork = uow ?? throw new ArgumentNullException(nameof(uow));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/literatures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LiteratureDto>>> GetAll(bool include = false)
        {
            try
            {
                var result = include ?
                    await unitOfWork.LiteraturesRepo.GetAllWithIncludeAsync(includes: l => l.Include(l => l.Subject)
                                                                                            .Include(l => l.Level)
                                                                                            .Include(l => l.Authors)) : 
                    await unitOfWork.LiteraturesRepo.GetAllAsync();

                return Ok(mapper.Map<IEnumerable<LiteratureDto>>(result));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex.InnerException}");
            }
        }

        // GET: api/literatures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Literature>> Get(int id, bool include = false)
        {
            try
            {
                var result = include ?
                    await unitOfWork.LiteraturesRepo.GetWithIncludeAsync(id, includes: src => src.Include(l => l.Subject).Include(l => l.Level)) :
                    await unitOfWork.LiteraturesRepo.GetAsync(id);

                if (result == null) return NotFound();

                return Ok(mapper.Map<LiteratureDto>(result));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex.InnerException}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<LiteratureDto>> Post(LiteratureForCreateUpdateDto dto)
        {
            try
            {
                /*if (await unitOfWork.LiteraturesRepo.GetAsync(dto.) != null)
                {
                    return BadRequest("Literature with the specified title already exists.");
                }*/

                var litterature = mapper.Map<Literature>(dto);
                unitOfWork.LiteraturesRepo.Add(litterature);

                if (await unitOfWork.CompleteAsync())
                {
                    return CreatedAtAction(nameof(Get), new { id = litterature.Id }, mapper.Map<LiteratureDto>(litterature));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex}");
            }
            return BadRequest(); // E.g. if the CompleteAsync fails...
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LiteratureDto>> Put(int id, LiteratureForCreateUpdateDto dto)
        {
            try
            {
                var existingLiterature = await unitOfWork.LiteraturesRepo.FindAsync(id);
                if (existingLiterature == null) return NotFound($"Could not find the literature with id: {id}.");
                if (existingLiterature.Title == dto.Title) return BadRequest("Literature with the specified title already exists.");

                mapper.Map(dto, existingLiterature);

                if (await unitOfWork.CompleteAsync())
                {
                    return Ok(mapper.Map<LiteratureDto>(existingLiterature));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure.");
            }
            return BadRequest(); // E.g. if the CompleteAsync fails...
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existingLiterature = await unitOfWork.LiteraturesRepo.FindAsync(id);
                if (existingLiterature == null) return NotFound($"Could not find the literature with id: {id}.");

                unitOfWork.LiteraturesRepo.Delete(existingLiterature);

                if (await unitOfWork.CompleteAsync())
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure.");
            }
            return BadRequest("Failed to delete the literature");
        }

        /*
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Literature>>> SearchByTitle(string searchQuery, bool include = false)
        {
            try
            {
                var result = include ?
                    await unitOfWork.LiteraturesRepo.GetAllWithIncludeAsync(includes: src => src.Include(l => l.Subject).Include(l => l.Level)) :
                    await unitOfWork.LiteraturesRepo.GetAllAsync();

                if (!result.Any()) return NotFound();

                return Ok(mapper.Map<IEnumerable<LiteratureDto>>(result));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex}");
            }
        }*/

    }
}
