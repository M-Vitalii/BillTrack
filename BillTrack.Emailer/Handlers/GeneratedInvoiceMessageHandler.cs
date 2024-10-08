using BillTrack.Core.Contracts.SqsMessages;
using BillTrack.Core.Exceptions;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Models;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.Extensions.Options;

namespace BillTrack.Emailer.Handlers;

public class GeneratedInvoiceMessageHandler : IMessageHandler<GeneratedInvoice>
{
    private readonly IS3FileService _s3FileService;
    private readonly AwsSettings _awsSettings;
    private readonly EmailSettings _emailSettings;

    public GeneratedInvoiceMessageHandler(
        IS3FileService s3FileService,
        IOptions<AwsSettings> awsSettingsOptions,
        IOptions<EmailSettings> emailSettingsOptions)
    {
        _s3FileService = s3FileService;
        _awsSettings = awsSettingsOptions.Value;
        _emailSettings = emailSettingsOptions.Value;
    }

    public async Task HandleMessageAsync(GeneratedInvoice invoice)
    {
        await using var invoiceFileStream = await _s3FileService.GetObjectAsync(_awsSettings.InvoiceBucketName, invoice.FileName)
            ?? throw new FileNotFoundException($"Invoice file with key '{invoice.FileName}' could not be retrieved from S3.");

        var emailBody = ConstructEmailBody(invoice);

        var emailResponse = await Email
            .From(_emailSettings.SenderEmail)
            .To(invoice.EmailTo)
            .Subject($"Invoice for {DateTime.Now.ToShortDateString()}")
            .Body(emailBody)
            .Attach(new Attachment
            {
                Data = invoiceFileStream,
                ContentType = "application/pdf",
                Filename = invoice.FileName
            })
            .SendAsync();

        if (!emailResponse.Successful)
        {
            throw new EmailSendFailedException($"Failed to send the email with the invoice. Reason(s): {string.Join(", ", emailResponse.ErrorMessages)}");
        }
    }

    private static string ConstructEmailBody(GeneratedInvoice invoice) => 
        $@"
        Dear {invoice.EmployeeFullName},

        We hope this email finds you well. Please find your invoice for the services provided below:

        Invoice Number: {invoice.FileName.Split(".")[0]}
        Invoice Date: {DateTime.Now.ToShortDateString()}

        Attached is your PDF invoice. Kindly review it at your convenience.

        Best regards,
        The Bill Track Team";
    }
