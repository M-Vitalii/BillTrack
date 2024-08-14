using AutoMapper;
using BillTrack.Core.Contracts.Invoice;
using BillTrack.Domain.Entities;

namespace BillTrack.Core.AutoMapperProfiles;

public class InvoiceProfile : Profile
{
    protected InvoiceProfile()
    {
        CreateMap<InvoiceRequest, Invoice>()
            .ForMember(o => o.Id, opt => opt.Ignore());

        CreateMap<Invoice, InvoiceResponse>();
    }
}