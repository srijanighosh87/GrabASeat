using Grab.A.Seat.Shared.Dtos;
using Grab.A.Seat.Shared.ManagerConfig;
using Grab.A.Seat.Shared.Commands;

namespace Grab.A.Seat.Shared.Manager
{
    public interface BaseManager<Tin> where Tin : ICommand
    {
        Task<ResponseDto> ProcessAsync(Tin command);
    }

}