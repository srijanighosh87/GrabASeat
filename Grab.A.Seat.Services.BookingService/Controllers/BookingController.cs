
using Grab.A.Seat.BookingAPI.Bookings.Commands;
using Grab.A.Seat.BookingAPI.Bookings.Managers;
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
        private readonly BaseGetAllManager<SearchBookingCommand> _searchAllBookingManager;
        private readonly BaseGetAllManager<GetPaginatedBookingCommand> _getPaginatedBookingsManager;
        private readonly BaseManager<GetBookingByIdCommand> _getBookingByIdManager;
        private readonly BaseManager<UpdateBookingCommand> _updateBookingManager;
        private readonly BaseManager<DeleteBookingCommand> _deleteBookingManager;



        public BookingController(ILogger<BookingController> logger,
            BaseManager<AddBookingCommand> addBookingManager,
            BaseGetAllManager<GetAllBookingCommand> getAllBookingsManager,
            BaseGetAllManager<GetPaginatedBookingCommand> getPaginatedBookingsManager,
            BaseGetAllManager<SearchBookingCommand> searchAllBookingManager,
            BaseManager<GetBookingByIdCommand> getBookingByIdManager,
            BaseManager<UpdateBookingCommand> updateBookingManager,
            BaseManager<DeleteBookingCommand> deleteBookingManager
            )
        {
            _logger = logger;
            _addManager = addBookingManager;
            _getAllBookingsManager = getAllBookingsManager;
            _searchAllBookingManager = searchAllBookingManager;
            _getPaginatedBookingsManager = getPaginatedBookingsManager;
            _getBookingByIdManager = getBookingByIdManager;
            _updateBookingManager = updateBookingManager;
            _deleteBookingManager = deleteBookingManager;
        }

        /// <summary>
        /// get Booking By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ResponseDto</returns>
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ResponseDto))]
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
        public async Task<IActionResult> GetAllBookings(bool fetchPastBookings)
        {
            var response = await _getAllBookingsManager.ProcessAsync(new GetAllBookingCommand
            {
                fetchPastBookings = fetchPastBookings
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
        /// Get All Bookings
        /// </summary>
        /// <returns>ResponseDto</returns>
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ResponseDto))]
        [HttpGet("SearchBookings")]
        public async Task<IActionResult> SearchBookings(string nameOrRef = null, DateTime date = new DateTime())
        {
            var response = await _searchAllBookingManager.ProcessAsync(new SearchBookingCommand
            {
                date = date,
                nameOrRef = nameOrRef
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
        /// Get All Bookings paginated
        /// </summary>
        /// <returns>ResponseDto</returns>
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ResponseDto))]
        [HttpGet("GetPaginatedBookings")]
        public async Task<IActionResult> GetAllBookingsPaginated(bool fetchPastBookings, int numberOfItemsPerPage, int pageNumber)
        {
            var response = await _getPaginatedBookingsManager.ProcessAsync(new GetPaginatedBookingCommand
            {
                fetchPastBookings = fetchPastBookings,
                numberOfItemsPerPage = numberOfItemsPerPage,
                pageNumber = pageNumber
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
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ResponseDto))]
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] AddBookingCommand addCustomer)
        {
            var response = await _addManager.ProcessAsync(addCustomer);
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
        /// Update an existing Booking
        /// </summary>
        /// <param name="updateCustomer"></param>
        /// <returns>ResponseDto</returns>
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ResponseDto))]
        [HttpPut]
        public async Task<IActionResult> UpdateBooking([FromBody] UpdateBookingCommand updateCustomer)
        {
            var response = await _updateBookingManager.ProcessAsync(updateCustomer);
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
