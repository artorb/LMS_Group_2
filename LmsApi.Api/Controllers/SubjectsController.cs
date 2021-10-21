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
    [Route("api/literatures/{literatureId}/subjects")]
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
        public async Task<ActionResult<IEnumerable<Subject>>> Get(int literatureId)
        {
            try
            {
                var result = await unitOfWork.SubjectsRepo.GetAllAsync(filter: s => s.Literatures.FirstOrDefault().Id == literatureId);
                return Ok(); // Need to add mapper
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}
