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

        // GET: api/literatures/filter
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<LiteratureAllDto>>> GetFiltredAll([FromQuery]string searchQuery = "", int selectId = 0)
        {
            try
            {
                var result =
                    await unitOfWork.LiteraturesRepo.GetAllAsync(filter: l => ((string.IsNullOrWhiteSpace(searchQuery) || l.Title.ToLower().Contains(searchQuery.ToLower())) || 
                                                                              ((string.IsNullOrEmpty(searchQuery) || l.Authors.Any(a => a.FirstName.ToLower().Contains(searchQuery.ToLower())) || l.Authors.Any(a => a.LastName.ToLower().Contains(searchQuery.ToLower()))))) &&
                                                                              (selectId == 0 || l.SubjectId == selectId), 
                                                                 include: l => l.Include(l => l.Category)
                                                                                .Include(l => l.Subject)
                                                                                .Include(l => l.Level)
                                                                                .Include(l => l.Authors));

                return Ok(mapper.Map<IEnumerable<LiteratureAllDto>>(result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure...");
            }
        }

        // GET: api/literatures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LiteratureAllDto>>> GetAll(bool include = false)
        {
            try
            {
                var result = include ?
                    await unitOfWork.LiteraturesRepo.GetAllAsync(include: l => l.Include(l => l.Category)
                                                                                .Include(l => l.Subject)
                                                                                .Include(l => l.Level)
                                                                                .Include(l => l.Authors)) : 
                    await unitOfWork.LiteraturesRepo.GetAllAsync();

                return Ok(mapper.Map<IEnumerable<LiteratureAllDto>>(result));
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure...");
            }
        }

        // GET: api/literatures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Literature>> Get(int id, bool include = false)
        {
            try
            {
                var result = include ?
                    await unitOfWork.LiteraturesRepo.GetAsync(id, 
                                    include: src => src.Include(l => l.Category).Include(l => l.Subject).Include(l => l.Level).Include(a => a.Authors)) :
                    await unitOfWork.LiteraturesRepo.GetAsync(id);

                if (result == null) return NotFound();

                return Ok(mapper.Map<LiteratureDto>(result));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex.InnerException}");
            }
        }

        //api/authors/Stephen/literauters/Rymden
        [HttpPost]
        public async Task<ActionResult<LiteratureDto>> Post(LiteratureForCreateDto dto)
        {
            try
            {
                var literatures = await unitOfWork.LiteraturesRepo.GetAllAsync();
                if (literatures.Any(l => l.Title == dto.Title))
                {
                    return BadRequest("Literature with the specified title already exists.");
                }

                var literature = mapper.Map<Literature>(dto);
                unitOfWork.LiteraturesRepo.Add(literature);

                if (await unitOfWork.CompleteAsync())
                {
                    return CreatedAtAction(nameof(Get), new { id = literature.Id }, mapper.Map<LiteratureDto>(literature));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure...");
            }
            return BadRequest(); // E.g. if the CompleteAsync fails...
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LiteratureDto>> Put(int id, LiteratureForUpdateDto dto)
        {
            try
            {
                var existingLiterature = await unitOfWork.LiteraturesRepo.FindAsync(id);
                if (existingLiterature == null) return NotFound($"Couldn't find the literature with id: {id}.");
               // if (existingLiterature.Title == dto.Title) return BadRequest("Literature with the specified title already exists.");                

                
                mapper.Map(dto, existingLiterature);

                if (await unitOfWork.CompleteAsync())
                {
                    return Ok(mapper.Map<LiteratureDto>(existingLiterature));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure...");
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure...");
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
