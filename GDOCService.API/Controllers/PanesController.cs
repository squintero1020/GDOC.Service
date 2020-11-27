using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GDOCService.DataAccess.DataContext;
using GDOCService.DataAccess.Models;
using GDOCService.Rules.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedService.Responses.Response;

namespace GDOCService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Panes")]
    public class PanesController : ControllerBase
    {
        public GDOCContext _context;
        public IPanesService _panes;

        public PanesController(GDOCContext context, IPanesService panes)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _panes = panes ?? throw new ArgumentNullException(nameof(panes));
        }

        /// <summary>
        /// Obtener lista de panes
        /// </summary>
        /// <returns>Obtener lista de panes.</returns>
        /// <response code="200">Obtener lista de panes</response>
        /// <response code="400">Error en el proceso</response>  
        /// 
        [HttpGet, Route("GetName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<PetitionResponse> GetName(string name) => await _panes.GetByID(name);

        /// <summary>
        /// Obtener lista de panes
        /// </summary>
        /// <returns>Obtener lista de panes.</returns>
        /// <response code="200">Obtener lista de panes</response>
        /// <response code="400">Error en el proceso</response>  
        /// 
        [HttpGet, Route("Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<PetitionResponse> Get() => await _panes.Get();


        /// <summary>
        /// Guardar panes
        /// </summary>
        /// <returns>Obtener lista de panes.</returns>
        /// <response code="200">Obtener lista de panes</response>
        /// <response code="400">Error en el proceso</response>  
        /// 
        [HttpPost, Route("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<PetitionResponse> Add(Panes panes) => await _panes.Add(panes);



        /// <summary>
        /// Guardar panes
        /// </summary>
        /// <returns>Obtener lista de panes.</returns>
        /// <response code="200">Obtener lista de panes</response>
        /// <response code="400">Error en el proceso</response>  
        /// 
        [HttpPut, Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<PetitionResponse> Update(Panes panes) => await _panes.Update(panes);

    }
}
