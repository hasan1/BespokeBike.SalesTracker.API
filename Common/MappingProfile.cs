using AutoMapper;
using BespokeBike.SalesTracker.API.Model;
using BespokeBike.SalesTracker.API.ModelDto;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerGetDto>();
        CreateMap<CustomerCreateDto, Customer>();
        CreateMap<CustomerUpdateDto, Customer>();



        CreateMap<Product, ProductGetDto>();
        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>();


        CreateMap<Employee, EmployeeGetDto>();
        CreateMap<EmployeeCreateDto, Employee>();
        CreateMap<EmployeeUpdateDto, Employee>();


        CreateMap<Sale, SaleGetDto>();
        CreateMap<SaleCreateDto, Sale>();
        CreateMap<SaleUpdateDto, Sale>();
    }
}
