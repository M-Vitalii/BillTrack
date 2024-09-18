using BillTrack.Core.Models.Worker;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BillTrack.Worker.Models;

public class InvoiceDocument : IDocument
{
    public InvoiceModel Model { get; }

    public InvoiceDocument(InvoiceModel model)
    {
        Model = model;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);

                page.Footer().AlignCenter().Text(text =>
                {
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
            });
    }

    void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column
                    .Item().Text($"Invoice #{Model.InvoiceId}")
                    .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                column.Item().Text(text =>
                {
                    text.Span("Issue date: ").SemiBold();
                    text.Span($"{Model.IssueDate:d}");
                });
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(20);

            column.Item().Row(row =>
            {
                row.RelativeItem().Component(new AddressFrom("From", Model.EmployeeWorkSummary));
                row.ConstantItem(50);
                row.RelativeItem().Component(new AddressTo("For", Model.EmployeeWorkSummary));
            });

            column.Item().Element(ComposeTable);

            var totalPrice = Model.EmployeeWorkSummary.CalculatedSalary;
            column.Item().PaddingRight(5).AlignRight().Text($"Grand total: {totalPrice:C}").SemiBold();
        });
    }

    void ComposeTable(IContainer container)
    {
        var headerStyle = TextStyle.Default.SemiBold();

        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(25);
                columns.RelativeColumn(3);
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Text("#");
                header.Cell().Text("Product").Style(headerStyle);
                header.Cell().AlignRight().Text("hours").Style(headerStyle);
                header.Cell().AlignRight().Text("Hourly rate").Style(headerStyle);
                header.Cell().AlignRight().Text("Amount").Style(headerStyle);

                header.Cell().ColumnSpan(5).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
            });

            table.Cell().Element(CellStyle).Text($"1");
            table.Cell().Element(CellStyle).Text(Model.EmployeeWorkSummary.ProjectName);
            table.Cell().Element(CellStyle).AlignRight().Text($"{Model.EmployeeWorkSummary.TotalHoursWorked}");
            table.Cell().Element(CellStyle).AlignRight().Text($"{Model.EmployeeWorkSummary.HourlyRate:C}");
            table.Cell().Element(CellStyle).AlignRight().Text($"{Model.EmployeeWorkSummary.CalculatedSalary:C}");

            static IContainer CellStyle(IContainer container) =>
                container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
        });
    }
}