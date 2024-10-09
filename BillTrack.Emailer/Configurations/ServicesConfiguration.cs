using BillTrack.Application.Factories;
using BillTrack.Application.Services;
using BillTrack.Core.Contracts.SqsMessages;
using BillTrack.Core.Factories;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Emailer.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace BillTrack.Emailer.Configurations;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddTransient<IS3FileService, S3FileService>();

        services.AddSingleton<IMessageHandlerFactory, MessageHandlerFactory>();
        services.AddTransient<IMessageHandler<GeneratedInvoice>, GeneratedInvoiceMessageHandler>();
        services.AddTransient<ISqsMessageDispatcher, SqsMessageDispatcher>();
        
        services.AddTransient<IMessageProcessor, SqsMessageProcessor>();
        
        return services;
    }
}