
using FluentValidation;
using Grab.A.Seat.Shared.Models;

namespace Grab.A.Seat.BookingAPI.Validator
{
    public class BookingValidator : AbstractValidator<Booking>
    {
        public BookingValidator()
        {
            RuleFor(booking => booking.BookingStartDateTime).MustAsync(CheckDate).WithMessage("Date can not be in past");
            RuleFor(booking => booking.Customer.Name).NotNull().WithMessage("Customer Name can not be null");
            RuleFor(booking => booking.Customer.ContactNumber).NotNull().WithMessage("Customer Contact can not be null");

        }

        private async Task<bool> CheckDate(DateTime date, CancellationToken token)
        {
            if (date < DateTime.Now) return false;
            return true;
        }
    }
}
