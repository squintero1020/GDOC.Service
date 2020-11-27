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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharedService.Responses.Response;

namespace GDOCService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Tiendas")]
    public class StoresController : ControllerBase
    {
        private readonly GDOCContext _context;
        private readonly ILogger<StoresController> _logger;
        private readonly IConfiguration _IConfiguration;
        private readonly IStoreService _Store;

        public StoresController(GDOCContext context, ILogger<StoresController> logger, IConfiguration Configuration, IStoreService storeService) =>
            (_context, _logger, _IConfiguration, _Store) =
            (context ?? throw new ArgumentNullException(nameof(context)),
                logger ?? throw new ArgumentNullException(nameof(logger)),
                    Configuration ?? throw new ArgumentNullException(nameof(Configuration)),
                        storeService ?? throw new ArgumentNullException(nameof(storeService)));

        /// <summary>
        /// Obtiene las tiendas por empresa.
        /// </summary>
        /// <param name="companyid">Código de empresa.</param>
        /// <returns>Obtiene las tiendas por empresa.</returns>
        /// <response code="200">Resultado de las tiendas, por empresa</response>
        /// <response code="400">Error en el proceso</response>  
        /// 
        [HttpGet, Route("GetByCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<PetitionResponse> GetByCompany(int companyid) =>
            await _Store.GetByCompany(_context, companyid);

        /// <summary>
        /// Obtiene las tiendas por empresa.
        /// </summary>
        /// <param name="companyid">Código de empresa.</param>
        /// <param name="storeid">ID de la tienda</param>
        /// <returns>Obtiene las tiendas por empresa.</returns>
        /// <response code="200">Resultado de las tiendas, por empresa</response>
        /// <response code="400">Error en el proceso</response>  
        /// 
        [HttpGet, Route("GetByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<PetitionResponse> GetByID(int companyid, int storeid) =>
            await _Store.GetByID(_context, companyid, storeid);
        
        
        /// <summary>
        /// Guarda tienda por empresa
        /// </summary>
        /// <param name="companyid">Código de empresa.</param>
        /// <param name="store">Objeto tienda por empresa.</param>
        /// <returns>Obtiene las tiendas por empresa.</returns>
        /// <response code="200">Resultado de las tiendas, por empresa</response>
        /// <response code="400">Error en el proceso</response>  
        /// 
        [HttpPost, Route("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<PetitionResponse> Add(int companyid, Stores store) =>
            await _Store.Add(_context, companyid,store);
        

        /// <summary>
        /// Guarda tienda por empresa
        /// </summary>
        /// <param name="companyid">Código de empresa.</param>
        /// <param name="storeid">ID de la tienda.</param>
        /// <param name="store">Objeto tienda por empresa.</param>
        /// <returns>Obtiene las tiendas por empresa.</returns>
        /// <response code="200">Resultado de las tiendas, por empresa</response>
        /// <response code="400">Error en el proceso</response>  
        /// 

        [HttpPut, Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<PetitionResponse> Update(int companyid,int storeid, Stores store) =>
            await _Store.Update(_context, companyid, storeid,store);
        
    }
}
