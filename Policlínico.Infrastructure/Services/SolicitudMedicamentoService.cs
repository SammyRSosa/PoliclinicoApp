using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Policlínico.Application.DTOs;
using Policlínico.Application.Interfaces;
using Policlínico.Domain.Entities;
using Policlínico.Infrastructure.Data;

namespace Policlínico.Infrastructure.Services
{
    public class SolicitudMedicamentoService : ISolicitudMedicamentoService
    {
        private readonly PoliclínicoDbContext _context;

        public SolicitudMedicamentoService(PoliclínicoDbContext context)
        {
            _context = context;
        }

        public async Task<SolicitudReadDto> CreateAsync(SolicitudCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (!await _context.Departamentos.AnyAsync(d => d.IdDepartamento == dto.DepartamentoId))
                throw new InvalidOperationException("Departamento no existe.");

            var solicitud = new SolicitudMedicamento
            {
                DepartamentoId = dto.DepartamentoId,
                FechaSolicitud = dto.FechaSolicitud,
                Estado = "Inactivo"
            };

            foreach (var d in dto.Detalles)
            {
                if (!await _context.Medicamentos.AnyAsync(m => m.Id == d.MedicamentoId))
                    throw new InvalidOperationException($"Medicamento id={d.MedicamentoId} no existe.");

                solicitud.Detalles.Add(new SolicitudMedicamentoDetalle
                {
                    MedicamentoId = d.MedicamentoId,
                    Cantidad = d.Cantidad
                });
            }

            _context.SolicitudesMedicamentos.Add(solicitud);
            await _context.SaveChangesAsync();

            return await MapToReadDtoAsync(solicitud.IdSolicitud);
        }

        public async Task<IEnumerable<SolicitudReadDto>> GetAllAsync()
        {
            var list = await _context.SolicitudesMedicamentos
                .Include(s => s.Departamento)
                .Include(s => s.Detalles).ThenInclude(d => d.Medicamento)
                .ToListAsync();

            return list.Select(s => MapToReadDto(s)).ToList();
        }

        public async Task<SolicitudReadDto?> GetByIdAsync(int id)
        {
            var s = await _context.SolicitudesMedicamentos
                .Include(x => x.Departamento)
                .Include(x => x.Detalles).ThenInclude(d => d.Medicamento)
                .FirstOrDefaultAsync(x => x.IdSolicitud == id);

            return s == null ? null : MapToReadDto(s);
        }

        public async Task<SolicitudReadDto> ApproveAsync(int id, int jefeDepartamentoId)
        {
            var s = await _context.SolicitudesMedicamentos
                .Include(x => x.Detalles)
                .FirstOrDefaultAsync(x => x.IdSolicitud == id);

            if (s == null) throw new InvalidOperationException("Solicitud no existe.");

            var jefe = await _context.Trabajadores.FindAsync(jefeDepartamentoId);
            if (jefe == null) throw new InvalidOperationException("Jefe departamento no existe.");

            // actualizar estado y jefe
            s.Estado = "Activo";
            s.JefeDepartamentoId = jefeDepartamentoId;

            await _context.SaveChangesAsync();

            return await MapToReadDtoAsync(s.IdSolicitud);
        }

        // helpers
        private SolicitudReadDto MapToReadDto(SolicitudMedicamento s)
        {
            return new SolicitudReadDto
            {
                IdSolicitud = s.IdSolicitud,
                DepartamentoId = s.DepartamentoId,
                DepartamentoNombre = s.Departamento?.Nombre ?? string.Empty,
                FechaSolicitud = s.FechaSolicitud,
                Estado = s.Estado,
                JefeDepartamentoId = s.JefeDepartamentoId,
                Detalles = s.Detalles.Select(d => new SolicitudDetalleReadDto
                {
                    MedicamentoId = d.MedicamentoId,
                    MedicamentoNombre = d.Medicamento?.Nombre ?? string.Empty,
                    Cantidad = d.Cantidad
                }).ToList()
            };
        }

        private async Task<SolicitudReadDto> MapToReadDtoAsync(int id)
        {
            var s = await _context.SolicitudesMedicamentos
                .Include(x => x.Departamento)
                .Include(x => x.Detalles).ThenInclude(d => d.Medicamento)
                .FirstOrDefaultAsync(x => x.IdSolicitud == id);

            if (s == null) throw new InvalidOperationException("Solicitud no encontrada tras guardar.");

            return MapToReadDto(s);
        }
    }
}
