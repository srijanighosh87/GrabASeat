
using Grab.A.Seat.BookingAPI.Bookings.Commands;
using Grab.A.Seat.Shared.Dtos;
using Grab.A.Seat.Shared.Manager;
using Grab.A.Seat.Shared.ManagerConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Grab.A.Seat.BookingAPI.Bookings
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly BaseManager<AddBookingCommand> _addManager;
        private readonly BaseGetAllManager<GetAllBookingCommand> _getAllBookingsManager;
        private readonly BaseManager<GetBookingByIdCommand> _getBookingByIdManager;
        private readonly BaseManager<UpdateBookingCommand> _updateBookingManager;
        private readonly BaseManager<DeleteBookingCommand> _deleteBookingManager;



        public BookingController(ILogger<BookingController> logger,
            BaseManager<AddBookingCommand> addBookingManager,
            BaseGetAllManager<GetAllBookingCommand> getAllBookingsManager,
            BaseManager<GetBookingByIdCommand> getBookingByIdManager,
            BaseManager<UpdateBookingCommand> updateBookingManager,
            BaseManager<DeleteBookingCommand> deleteBookingManager
            )
        {
            _logger = logger;
            _addManager = addBookingManager;
            _getAllBookingsManager = getAllBookingsManager;
            _getBookingByIdManager = getBookingByIdManager;
            _updateBookingManager = updateBookingManager;
            _deleteBookingManager = deleteBookingManager;
        }

        /// <summary>
        /// get Booking By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ResponseDto</returns>
        [HttpGet("{id}")]
        public async Task<ResponseDto> GetBooking(Guid id)
        {
            return await _getBookingByIdManager.ProcessAsync(new GetBookingByIdCommand { id = id });
        }

        /// <summary>
        /// Get All Bookings
        /// </summary>
        /// <returns>ResponseDto</returns>
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ResponseDto))]
        [HttpGet("GetAllBookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            var response = await _getAllBookingsManager.ProcessAsync(new GetAllBookingCommand
            {
                isTrackingEnabled = true
            });
            if (response.IsSuccess)
                return Ok(response);
            else
            {
                return Conflict(new ResponseDto
                {
                    IsSuccess = false,
                    Message = response.Message,
                    Result = response.Result
                });
            }
        }

        /// <summary>
        /// Create a new Booking
        /// </summary>
        /// <param name="addCustomer"></param>
        /// <returns>ResponseDto</returns>
        [HttpPost]
        public async Task<ResponseDto> CreateBooking([FromBody] AddBookingCommand addCustomer)
        {
            return await _addManager.ProcessAsync(addCustomer);
        }

        /// <summary>
        /// Update an existing Booking
        /// </summary>
        /// <param name="updateCustomer"></param>
        /// <returns>ResponseDto</returns>
        [HttpPut]
        public async Task<ResponseDto> UpdateBooking([FromBody] UpdateBookingCommand updateCustomer)
        {
            var x = await _updateBookingManager.ProcessAsync(updateCustomer);
            return x;
        }

        /// <summary>
        /// Delete a  booking
        /// </summary>
        /// <param name="id"></param>
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ResponseDto))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            var response = await _deleteBookingManager.ProcessAsync(new DeleteBookingCommand { Id = id });
            if (response.IsSuccess)
                return Ok(response);
            else
            {
                return Conflict(new ResponseDto
                {
                    IsSuccess = false,
                    Message = response.Message,
                    Result = response.Result
                });
            }
        }
    }
}
