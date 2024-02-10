

using Grab.A.Seat.Shared.Dtos;
using Grab.A.Seat.Shared.Commands;

namespace Grab.A.Seat.Shared.ManagerConfig
{
    public interface BaseGetAllManager<Tin> where Tin : ICommand
    {
        Task<ResponseDto> ProcessAsync(Tin command);
    }

}