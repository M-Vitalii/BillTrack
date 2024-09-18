using BillTrack.Core.Models.Worker;
using BillTrack.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BillTrack.Worker.Models;

public class AddressTo : IComponent
{
    private string Title { get; }
    private EmployeeWorkSummary EmployeeWorkSummary { get; }

    public AddressTo(string title, EmployeeWorkSummary employeeWorkSummary)
    {
        Title = title;
        EmployeeWorkSummary = employeeWorkSummary;
    }
        
    public void Compose(IContainer container)
    {
        container.ShowEntire().Column(column =>
        {
            column.Spacing(2);

            column.Item().Text(Title).SemiBold();
            column.Item().PaddingBottom(5).LineHorizontal(1); 
                
            column.Item().Text($"{EmployeeWorkSummary.FullName}");
            column.Item().Text($"{EmployeeWorkSummary.Email}");
        });
    }
}