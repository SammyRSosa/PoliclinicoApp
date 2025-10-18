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
    public class EntregaMedicamentoService : IEntregaMedicamentoService
    {
        private readonly PoliclínicoDbContext _context;

        public EntregaMedicamentoService(PoliclínicoDbContext context)
        {
            _context = context;
        }

        public async Task<EntregaMedicamentoReadDto> CreateAsync(EntregaMedicamentoCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var dep = await _context.Departamentos.FindAsync(dto.DepartamentoDestinoId);
            if (dep == null) throw new InvalidOperationException("Departamento destino no existe.");

            var entrega = new EntregaMedicamento
            {
                DepartamentoDestinoId = dto.DepartamentoDestinoId,
                FechaEntrega = dto.FechaEntrega,
                Estado = "Inactivo"
            };

            foreach (var d in dto.Detalles)
            {
                if (!await _context.Medicamentos.AnyAsync(m => m.Id == d.MedicamentoId))
                    throw new InvalidOperationException($"Medicamento id={d.MedicamentoId} no existe.");

                entrega.Detalles.Add(new EntregaMedicamentoDetalle
                {
                    MedicamentoId = d.MedicamentoId,
                    Cantidad = d.Cantidad
                });
            }

            _context.EntregasMedicamentos.Add(entrega);
            await _context.SaveChangesAsync();

            return await MapToReadDtoAsync(entrega.IdEntrega);
        }

        public async Task<IEnumerable<EntregaMedicamentoReadDto>> GetAllAsync()
        {
            var list = await _context.EntregasMedicamentos
                .Include(e => e.DepartamentoDestino)
                .Include(e => e.Detalles).ThenInclude(d => d.Medicamento)
                .ToListAsync();

            return list.Select(MapToReadDto).ToList();
        }

        public async Task<EntregaMedicamentoReadDto?> GetByIdAsync(int id)
        {
            var e = await _context.EntregasMedicamentos
                .Include(x => x.DepartamentoDestino)
                .Include(x => x.Detalles).ThenInclude(d => d.Medicamento)
                .FirstOrDefaultAsync(x => x.IdEntrega == id);

            return e == null ? null : MapToReadDto(e);
        }

        public async Task<EntregaMedicamentoReadDto> ApproveAsync(int id, int jefeAlmacenId)
        {
            var e = await _context.EntregasMedicamentos
                .Include(x => x.Detalles)
                .FirstOrDefaultAsync(x => x.IdEntrega == id);

            if (e == null) throw new InvalidOperationException("Entrega no existe.");

            var jefe = await _context.Trabajadores.FindAsync(jefeAlmacenId);
            if (jefe == null) throw new InvalidOperationException("Jefe de almacén no existe.");

            e.Estado = "Activo";
            e.JefeAlmacenId = jefeAlmacenId;

            await _context.SaveChangesAsync();
            return await MapToReadDtoAsync(e.IdEntrega);
        }

        private EntregaMedicamentoReadDto MapToReadDto(EntregaMedicamento e)
        {
            return new EntregaMedicamentoReadDto
            {
                IdEntrega = e.IdEntrega,
                DepartamentoDestinoId = e.DepartamentoDestinoId,
                DepartamentoDestinoNombre = e.DepartamentoDestino?.Nombre ?? string.Empty,
                FechaEntrega = e.FechaEntrega,
                Estado = e.Estado,
                JefeAlmacenId = e.JefeAlmacenId,
                Detalles = e.Detalles.Select(d => new EntregaMedicamentoDetalleReadDto
                {
                    MedicamentoId = d.MedicamentoId,
                    MedicamentoNombre = d.Medicamento?.Nombre ?? string.Empty,
                    Cantidad = d.Cantidad
                }).ToList()
            };
        }

        private async Task<EntregaMedicamentoReadDto> MapToReadDtoAsync(int id)
        {
            var e = await _context.EntregasMedicamentos
                .Include(x => x.DepartamentoDestino)
                .Include(x => x.Detalles).ThenInclude(d => d.Medicamento)
                .FirstAsync(x => x.IdEntrega == id);

            return MapToReadDto(e);
        }
    }
}
