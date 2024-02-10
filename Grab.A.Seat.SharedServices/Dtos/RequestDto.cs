using System.Runtime.Serialization;

namespace Grab.A.Seat.Shared.Dtos
{
    public class RequestDto
    {
        public string Url { get; set; }
        public ApiTypeEnum ApiType { get; set; }
        public object Data { get; set; }
    }

    public enum ApiTypeEnum
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}