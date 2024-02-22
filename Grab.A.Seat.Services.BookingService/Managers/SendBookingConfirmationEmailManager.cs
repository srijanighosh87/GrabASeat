

using FluentValidation;
using Grab.A.seat.BookingService;
using Grab.A.Seat.BookingAPI.Bookings.Commands;
using Grab.A.Seat.BookingAPI.Bookings.Dto;
using Grab.A.Seat.Domains.Data;
using Grab.A.Seat.Shared.Dtos;
using Grab.A.Seat.Shared.Manager;
using Grab.A.Seat.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Grab.A.Seat.BookingAPI.Bookings.Managers
{
    public class SendBookingConfirmationEmailManager : BaseManager<SendEmailCommand>
    {
        private readonly ILogger<SendBookingConfirmationEmailManager> _logger;
        private readonly ResponseDto _responseDto;
        private readonly IConfiguration _configuration;


        public SendBookingConfirmationEmailManager(
            ILogger<SendBookingConfirmationEmailManager> logger,
            IConfiguration configuration
            )
        {
            _logger = logger;
            _responseDto = new ResponseDto();
            _configuration = configuration;
        }
        public async Task<ResponseDto> ProcessAsync(SendEmailCommand command)
        {
            try
            {
                var sendGridApiKey = _configuration.GetSection("SendGrid:ApiKey").Value;
                var client = new SendGridClient(sendGridApiKey);
                var from = new EmailAddress("n4nova@gmail.com", "Grab-A-Seat Team");
                var to = new EmailAddress(command.Email, command.Name);
                var subject = $"Booking Confirmed - Ref No. {command.RefNo}";
                var plainTextContent = "This is a test email sent from SendGrid.";
                var htmlContent = $@"
                    <html>
                    <p>
                    Dear {command.Name},
                    </p>
                    <p>
                    Your booking is confirmed now for date: {command.Date}
                    </p>
                    <p>
                    Best regards,
                    </p>
                    <p>
                    Grab-A-Seat Team.
                    </p>
                    </html>
                ";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);

                return new ResponseDto
                {
                    IsSuccess = response.IsSuccessStatusCode,
                    Message = response.StatusCode.ToString(),
                    Result = null
                };
            }
            catch (Exception e)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = e.Message,
                    Result = null
                };
            }
        }

    }
}
