using AutoMapper;
using BAL.DTO;
using DAL.Models;

namespace BAL.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDTO>();
            CreateMap<EmployeeDTO, Employee>();

            CreateMap<EmployeeInfo, EmployeeInfoDTO>();
            CreateMap<EmployeeInfoDTO, EmployeeInfo>();

            CreateMap<Position, PositionDTO>();
            CreateMap<PositionDTO, Position>();

            CreateMap<Department, DepartmentDTO>();
            CreateMap<DepartmentDTO, Department>();

            CreateMap<Company, CompanyDTO>();
        }
    }
}
