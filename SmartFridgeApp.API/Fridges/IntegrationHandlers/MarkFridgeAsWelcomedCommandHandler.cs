using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Domain.Models.Fridges;

namespace SmartFridgeApp.API.Fridges.IntegrationHandlers
{
    public class MarkFridgeAsWelcomedCommandHandler : IRequestHandler<MarkFridgeAsWelcomedCommand, Unit>
    {
        private readonly IFridgeRepository _fridgeRepository;

        public MarkFridgeAsWelcomedCommandHandler(IFridgeRepository fridgeRepository)
        {
            _fridgeRepository = fridgeRepository;
        }

        public async Task<Unit> Handle(MarkFridgeAsWelcomedCommand command, CancellationToken cancellationToken)
        {
            var fridge = await this._fridgeRepository.GetByIdAsync(command.FridgeId);

            fridge.MarkAsWelcomed();
            
            return Unit.Value;
        }
    }
}
