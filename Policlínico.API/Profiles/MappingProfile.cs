using AutoMapper;
using Policlínico.Domain.Entities;
using Policlínico.Application.DTOs;
using Policlínico.API.DTOs;
using System.Linq;

namespace Policlínico.API.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {


            CreateMap<Consulta, ConsultaReadDto>()
                 .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Departamento != null ? src.Departamento.Nombre : null))
                 .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo))
                 .ForMember(dest => dest.Diagnostico, opt => opt.MapFrom(src => src.Diagnostico))
                 .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado));

            // Consulta de Emergencia
            CreateMap<ConsultaEmergencia, ConsultaReadDto>()
                .IncludeBase<Consulta, ConsultaReadDto>()
                .ForMember(dest => dest.Paciente, opt => opt.MapFrom(src => src.Paciente != null ? src.Paciente.Nombre : null))
                .ForMember(dest => dest.MedicoPrincipal, opt => opt.MapFrom(src => src.MedicoPrincipal != null ? src.MedicoPrincipal.Nombre : null))
                .ForMember(dest => dest.MedicoAtendio, opt => opt.MapFrom(src => src.MedicoAtendio != null ? src.MedicoAtendio.Nombre : null));

            // Consulta Programada
            CreateMap<ConsultaProgramada, ConsultaReadDto>()
                .IncludeBase<Consulta, ConsultaReadDto>()
                .ForMember(dest => dest.Remision, opt => opt.MapFrom(src => src.Remision != null ? $"Remisión #{src.Remision.IdRemision}" : null))
                .ForMember(dest => dest.MedicoPrincipal, opt => opt.MapFrom(src => src.MedicoPrincipal != null ? src.MedicoPrincipal.Nombre : null))
                .ForMember(dest => dest.MedicoAtendio, opt => opt.MapFrom(src => src.MedicoAtendio != null ? src.MedicoAtendio.Nombre : null));

            // =====================================================
            // 📌 REMISIONES
            // =====================================================
            CreateMap<Remision, RemisionReadDto>()
                .ForMember(dest => dest.DepartamentoAtiendeId, opt => opt.MapFrom(src => src.Departamento != null ? src.Departamento.Nombre : null))
                .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.Paciente != null ? src.Paciente.Nombre : null));

            CreateMap<RemisionCreateDto, Remision>();
            CreateMap<RemisionUpdateDto, Remision>();

            // =====================================================
            // 📌 TRABAJADORES
            // =====================================================
            CreateMap<Trabajador, TrabajadorReadDto>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));

            CreateMap<TrabajadorCreateDto, Trabajador>();
            CreateMap<TrabajadorUpdateDto, Trabajador>();

            // =====================================================
            // 📌 DEPARTAMENTOS
            // =====================================================
            CreateMap<Departamento, DepartamentoReadDto>();
            CreateMap<DepartamentoCreateDto, Departamento>();
            CreateMap<DepartamentoUpdateDto, Departamento>();




            // Departamento → DTO completo (lectura)
            CreateMap<Departamento, DepartamentoReadDto>()
                .ForMember(dest => dest.JefeNombre,
                    opt => opt.MapFrom(src => src.Jefe != null ? src.Jefe.Nombre : null))
                .ForMember(dest => dest.Trabajadores,
                    opt => opt.MapFrom(src =>
                        src.Asignaciones != null
                            ? src.Asignaciones
                                .Where(a => a.FechaFin == null && a.Trabajador != null)
                                .Select(a => new TrabajadorMiniDto
                                {
                                    IdTrabajador = a.Trabajador.IdTrabajador,
                                    Nombre = a.Trabajador.Nombre,
                                    Cargo = a.Trabajador.Cargo
                                }).ToList()
                            : new List<TrabajadorMiniDto>()
                    ));

            // Departamento → DTO resumido (para respuestas simples)
            CreateMap<Departamento, DepartamentoDto>()
                .ForMember(dest => dest.JefeNombre,
                    opt => opt.MapFrom(src => src.Jefe != null ? src.Jefe.Nombre : null))
                .ForMember(dest => dest.Trabajadores,
                    opt => opt.MapFrom(src =>
                        src.Asignaciones != null
                            ? src.Asignaciones
                                .Where(a => a.FechaFin == null && a.Trabajador != null)
                                .Select(a => new TrabajadorMiniDto
                                {
                                    IdTrabajador = a.Trabajador.IdTrabajador,
                                    Nombre = a.Trabajador.Nombre,
                                    Cargo = a.Trabajador.Cargo
                                }).ToList()
                            : new List<TrabajadorMiniDto>()
                    ));

            // DTO → Departamento (creación)
            CreateMap<DepartamentoCreateDto, Departamento>()
                .ForMember(dest => dest.Estado,
                    opt => opt.MapFrom(src => src.JefeId.HasValue ? "Activo" : "Inactivo"))
                .ForMember(dest => dest.JefeId,
                    opt => opt.MapFrom(src => src.JefeId));

            // DTO → Departamento (actualización)
            CreateMap<DepartamentoUpdateDto, Departamento>()
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado));

            // Trabajador → DTO lectura
            CreateMap<Trabajador, TrabajadorReadDto>()
                .ForMember(dest => dest.Asignaciones,
                    opt => opt.MapFrom(src =>
                        src.Asignaciones != null
                            ? src.Asignaciones.Select(a => new AsignacionDto
                            {
                                DepartamentoId = a.DepartamentoId,
                                DepartamentoNombre = a.Departamento != null ? a.Departamento.Nombre : string.Empty,
                                FechaInicio = a.FechaInicio,
                                FechaFin = a.FechaFin
                            }).ToList()
                            : new List<AsignacionDto>()));

            // DTO → Trabajador (creación)
            CreateMap<TrabajadorCreateDto, Trabajador>()
                .ForMember(dest => dest.Asignaciones, opt => opt.Ignore());

            // DTO → Trabajador (actualización)
            CreateMap<TrabajadorUpdateDto, Trabajador>()
                .ForMember(dest => dest.Asignaciones, opt => opt.Ignore());

            // Asignación → DTO
            CreateMap<Asignacion, AsignacionDto>()
                .ForMember(dest => dest.DepartamentoNombre,
                    opt => opt.MapFrom(src => src.Departamento != null ? src.Departamento.Nombre : string.Empty));

            // DTO auxiliar: Trabajador simplificado
            CreateMap<Trabajador, TrabajadorMiniDto>();


            // al final de tu constructor en MappingProfile
            CreateMap<Departamento, DepartamentoSimpleDto>()
                .ForMember(dest => dest.JefeNombre,
                    opt => opt.MapFrom(src => src.Jefe != null ? src.Jefe.Nombre : null))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado));

        }
    }
}
