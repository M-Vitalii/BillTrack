using BillTrack.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BillTrack.Worker.Models;

public class AddressTo : IComponent
{
    private string Title { get; }
    private Employee Employee { get; }

    public AddressTo(string title, Employee employee)
    {
        Title = title;
        Employee = employee;
    }
        
    public void Compose(IContainer container)
    {
        container.ShowEntire().Column(column =>
        {
            column.Spacing(2);

            column.Item().Text(Title).SemiBold();
            column.Item().PaddingBottom(5).LineHorizontal(1); 
                
            column.Item().Text($"{Employee.Firstname} {Employee.Lastname}");
            column.Item().Text($"{Employee.Email}");
        });
    }
}