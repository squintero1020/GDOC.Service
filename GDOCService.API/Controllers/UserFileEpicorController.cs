using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GDOCService.Rules.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharedService.Responses.Response;

namespace GDOCService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Usuarios Epicor")]
    public class UserFileEpicorController : ControllerBase
    {
        private readonly ILogger<UserFileEpicorController> _logger;
        private readonly IConfiguration _IConfiguration;
        private readonly IUserFileEpicorService _userFileEpicor;

        public UserFileEpicorController(ILogger<UserFileEpicorController> logger, IConfiguration Configuration, IUserFileEpicorService userFileEpicor) =>
            (_logger, _IConfiguration, _userFileEpicor) =
                (logger ?? throw new ArgumentNullException(nameof(logger)),
                    Configuration ?? throw new ArgumentNullException(nameof(Configuration)),
                        userFileEpicor ?? throw new ArgumentNullException(nameof(userFileEpicor)));


        /// <summary>
        /// Obtiene los usuarios de la empresa por un where especifico.
        /// </summary>
        /// <param name="where">Where de la consulta de usuarios.</param>
        /// <returns>Obtiene los usuarios de la empresa por un where especifico.</returns>
        /// <response code="200">Obtiene los usuarios de la empresa por un where especifico.</response>
        /// <response code="400">Error en el proceso</response>  
        /// 
        [HttpGet, Route("GetRows")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<PetitionResponse> GetRows(string where) =>
            await _userFileEpicor.GetRows(where);

    }
}
