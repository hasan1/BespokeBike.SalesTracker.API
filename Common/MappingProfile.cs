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
       }
   }
   