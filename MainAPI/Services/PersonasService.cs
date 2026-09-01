using MainAPI.Data;
using MainAPI.Models;
using MainAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MainAPI.Services
{
    public class PersonasService : IPersonasService
    {
        private readonly MainDbContext _context;

        public PersonasService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetPersonasAsync()
        {
            return await _context.Personas.Where(p => p.Activo != false).ToListAsync();
        }

        public async Task<object> PostPersonaAsync(PersonaDto d)
        {
            var e = new Persona { LoginUserId = d.LoginUserId, PrimerNombre = d.PrimerNombre, SegundoNombre = d.SegundoNombre, TercerNombre = d.TercerNombre, PrimerApellido = d.PrimerApellido, SegundoApellido = d.SegundoApellido, Activo = true };
            _context.Personas.Add(e);
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<object> GetGestionUsuariosAsync()
        {
            var personas = await _context.Personas
                .Select(p => new
                {
                    IdPersona = p.IdPersona,
                    LoginUserId = p.LoginUserId,
                    NombreCompleto = p.PrimerNombre + " " + p.PrimerApellido,
                    EsAdmin = _context.PerfilAdministradors.Any(a => a.IdPersona == p.IdPersona && a.Activo != false),
                    EsDocente = _context.PerfilCatedraticos.Any(c => c.IdPersona == p.IdPersona && c.Activo != false),
                    EsEstudiante = _context.PerfilEstudiantes.Any(e => e.IdPersona == p.IdPersona && e.Activo != false),
                    Activo = p.Activo ?? true
                })
                .ToListAsync();

            var resultado = personas.Select(p => new
            {
                p.IdPersona,
                p.LoginUserId,
                p.NombreCompleto,
                RolPrincipal = p.EsAdmin ? "Administrador" : (p.EsDocente ? "Docente" : (p.EsEstudiante ? "Estudiante" : "Sin Rol")),
                Activo = p.Activo
            }).ToList();

            return resultado;
        }

        public async Task<(bool IsSuccess, string Message)> DeshabilitarPersonaAsync(int id)
        {
            var p = await _context.Personas.FindAsync(id);
            if (p == null) return (false, "Persona no encontrada");

            p.Activo = false;
            await _context.SaveChangesAsync();
            return (true, "Persona deshabilitada correctamente");
        }

        public async Task<(bool IsSuccess, string Message)> HabilitarPersonaAsync(int id)
        {
            var p = await _context.Personas.FindAsync(id);
            if (p == null) return (false, "Persona no encontrada");

            p.Activo = true;
            await _context.SaveChangesAsync();
            return (true, "Persona habilitada correctamente");
        }
    }
}
