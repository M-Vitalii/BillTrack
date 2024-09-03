using BillTrack.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BillTrack.Worker.Models;

public class AddressFrom : IComponent
{
    private string Title { get; }
    private Employee Employee { get; }

    public AddressFrom(string title, Employee address)
    {
        Title = title;
        Employee = address;
    }
        
    public void Compose(IContainer container)
    {
        container.ShowEntire().Column(column =>
        {
            column.Spacing(2);

            column.Item().Text(Title).SemiBold();
            column.Item().PaddingBottom(5).LineHorizontal(1); 
                
            column.Item().Text(Employee.Department.Name);
        });
    }   
}