using AutoMapper;
using BillTrack.Core.Contracts.Invoice;
using BillTrack.Domain.Entities;

namespace BillTrack.Core.AutoMapperProfiles;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<InvoiceRequest, Invoice>()
            .ForMember(o => o.Id, opt => opt.Ignore());

        CreateMap<Invoice, InvoiceResponse>()
            .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.Employee));
        
        CreateMap<InvoiceUpdateRequest, Invoice>();
        CreateMap<Invoice, InvoiceResponse>();
    }
}