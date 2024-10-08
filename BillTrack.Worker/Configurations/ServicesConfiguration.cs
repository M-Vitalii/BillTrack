using BillTrack.Application.Factories;
using BillTrack.Application.Services;
using BillTrack.Core.Contracts.SqsMessages;
using BillTrack.Core.Factories;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Worker.Handlers;
using BillTrack.Worker.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BillTrack.Worker.Configurations;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddTransient<IEmployeeSalaryCalculator, EmployeeSalaryCalculator>();
        services.AddTransient<IPdfGenerator, InvoicePdfGenerator>();
        
        services.AddTransient<IS3FileService, S3FileService>();
        services.AddTransient<ISqsPublisher, SqsPublisher>();

        services.AddSingleton<IMessageHandlerFactory, MessageHandlerFactory>();
        services.AddTransient<IMessageHandler<CreatedInvoice>, CreatedInvoiceMessageHandler>();
        services.AddTransient<ISqsMessageDispatcher, SqsMessageDispatcher>();
        
        services.AddTransient<IMessageProcessor, SqsMessageProcessor>();
        
        return services;
    }
}