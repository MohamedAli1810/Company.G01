using AutoMapper;
using Company.G01.DAL.Models;
using Company.G01.PL.Dtos;

namespace Company.G01.PL.Mapping
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<Department, CreateDepartmentDto>();
        }
    }
}
