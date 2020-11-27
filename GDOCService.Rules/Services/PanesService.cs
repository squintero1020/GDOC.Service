using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GDOCService.DataAccess.DataContext;
using GDOCService.DataAccess.Models;
using GDOCService.Rules.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedService.Responses.Response;

namespace GDOCService.Rules.Services
{
    public class PanesService : IPanesService
    {
        public GDOCContext _context;
        private readonly HttpClient _httpClient;

        public PanesService(GDOCContext context, HttpClient httpClient) 
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<PetitionResponse> GetByID(string Name)
        {
            var pan = await _context.Panes.Where(x=> x.Name == Name).ToListAsync();

            return new PetitionResponse
            {
                success = true,
                message = "Consulta especifica",
                result = pan
            };
        }

        public async Task<PetitionResponse> Get()
        {
            var pan = await _context.Panes.ToListAsync();

            return new PetitionResponse 
            {
                success = true,
                message = "Consulse",
                result = pan
            };
        }

        public async Task<PetitionResponse> Add(Panes panes)
        {
            _context.Panes.Add(panes);
            if (await _context.SaveChangesAsync() < 0)
            {
                return new PetitionResponse
                {
                    success = false,
                    message = "Error al guardar"
                };
            }

            return new PetitionResponse
            {
                success = true,
                message = "Guarde"
            };
        }

        public async Task<PetitionResponse> Update(Panes panes)
        {
            _context.Entry<Panes>(panes).State = EntityState.Modified;

            if (await _context.SaveChangesAsync() < 0)
            {
                return new PetitionResponse
                {
                    success = false,
                    message = "Error al a"
                };
            }

            return new PetitionResponse
            {
                success = true,
                message = "Guarde"
            };
        }
    }
}
