using AutoMapper;
using SS.Template.Domain.Entities;

namespace SS.Template.Application.Customers
{
    public sealed class CustomersMapping : Profile
    {
        public CustomersMapping()
        {
            CreateMap<Customer, CustomerModel>()
                .ReverseMap()
                .ForMember(x => x.Id, e => e.Ignore())
                .ForMember(x => x.Status, e => e.Ignore())
                .ForMember(x => x.DateCreated, e => e.Ignore())
                .ForMember(x => x.DateUpdated, e => e.Ignore());
        }
    }
}
