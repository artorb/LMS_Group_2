using AutoMapper;
using LmsApi.Core.Dtos;
using LmsApi.Core.Entities;
using LmsApi.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LmsApi.Api.Controllers
{
    [Route("api/subjects")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SubjectsController(IUnitOfWork uow, IMapper mapper)
        {
            unitOfWork = uow ?? throw new ArgumentNullException(nameof(uow));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjectsToSelectListItem()
        {
            try
            {
                var subjects = await unitOfWork.SubjectsRepo.GetAllAsync();

                return Ok(mapper.Map<IEnumerable<SelectListItemDto>>(subjects));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure...");
            }
        }
    }
}
