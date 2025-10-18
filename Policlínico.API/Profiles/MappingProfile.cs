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
                .ForMember(dest => dest.IdConsulta, opt => opt.MapFrom(src => src.IdConsulta))
                .ForMember(dest => dest.TipoConsulta, opt => opt.MapFrom(src => src.Tipo))
                .ForMember(dest => dest.FechaConsulta, opt => opt.MapFrom(src => src.FechaConsulta))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
                .ForMember(dest => dest.Diagnostico, opt => opt.MapFrom(src => src.Diagnostico))
                .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
                .ForMember(dest => dest.DoctorPrincipalId, opt => opt.MapFrom(src => src.MedicoPrincipalId))
                .ForMember(dest => dest.DoctorPrincipalNombre, opt => opt.MapFrom(src => src.MedicoPrincipal != null ? src.MedicoPrincipal.Nombre : null))
                .ForMember(dest => dest.DepartamentoAtiendeId, opt => opt.MapFrom(src => src.DepartamentoId))
                // .ForMember(dest => dest.PuestoMedicoId, opt => opt.MapFrom(src => src.Departamento != null ? src.Departamento.Nombre : null))
                .ForMember(dest => dest.DoctoresParticipantes, opt => opt.MapFrom(src =>
                    src.Doctores != null ? src.Doctores.Select(d => new TrabajadorMiniDto
                    {
                        IdTrabajador = d.IdTrabajador,
                        Nombre = d.Nombre,
                        Cargo = d.Cargo
                    }).ToList() : new List<TrabajadorMiniDto>()
                ));




            // 🏥 Departamento → DTO completo (lectura)
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

            // 🏥 Departamento → DTO resumido (para respuestas simples)
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

            // 👨‍⚕️ Trabajador → DTO lectura
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

            // 🔗 Asignación → DTO
            CreateMap<Asignacion, AsignacionDto>()
                .ForMember(dest => dest.DepartamentoNombre,
                    opt => opt.MapFrom(src => src.Departamento != null ? src.Departamento.Nombre : string.Empty));

            // DTO auxiliar: Trabajador simplificado
            CreateMap<Trabajador, TrabajadorMiniDto>();


            // 👇 al final de tu constructor en MappingProfile
            CreateMap<Departamento, DepartamentoSimpleDto>()
                .ForMember(dest => dest.JefeNombre,
                    opt => opt.MapFrom(src => src.Jefe != null ? src.Jefe.Nombre : null))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado));

         

            // // ConsultaCreateDTO → Consulta
            // CreateMap<ConsultaCreateDTO, Consulta>()
            //     .ForMember(dest => dest.Estado, opt => opt.Ignore()) // estado se calcula en servicio
            //     .ForMember(dest => dest.ConsultaTrabajadores, opt => opt.Ignore());

            // // ConsultaUpdateDTO → Consulta (mapear solo algunos campos)
            // CreateMap<ConsultaUpdateDTO, Consulta>()
            //     .ForMember(dest => dest.Estado, opt => opt.Ignore())
            //     .ForMember(dest => dest.ConsultaTrabajadores, opt => opt.Ignore());

            // // Consulta → ConsultaSimpleDTO
            // CreateMap<Consulta, ConsultaSimpleDTO>();




        }
    }
}
