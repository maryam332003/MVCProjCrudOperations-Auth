using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;

namespace Demo.PL.Helper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
            //    .ForMember(des => des.ImageName, opt => opt.MapFrom(src => src.Image));
            //CreateMap<Employee, EmployeeViewModel>();
            ;
            CreateMap<DepartmentViewModel, Department>().ReverseMap();
            CreateMap<SignUpViewModel,ApplicationUser>().ReverseMap();  
        }
    }
}
