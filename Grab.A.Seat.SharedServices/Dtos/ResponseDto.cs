

using Microsoft.Extensions.Logging;

namespace Grab.A.Seat.Shared.Dtos
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object? Result { get; set; }
    }

    public static class WrapResponse
    {
        public static ResponseDto WrapOk(ResponseDto responseDto, string Message, object Result = null)
        {
            responseDto.Message = Message;
            responseDto.IsSuccess = true;
            responseDto.Result = Result;

            return responseDto;
        }

        public static ResponseDto WrapError<T>(ResponseDto responseDto, string Message, object Result = null, ILogger<T> logger = null)
        {
            logger.LogError(Message);
            responseDto.Message = Message;
            responseDto.IsSuccess = false;
            responseDto.Result = Result;

            return responseDto;
        }
    }
}