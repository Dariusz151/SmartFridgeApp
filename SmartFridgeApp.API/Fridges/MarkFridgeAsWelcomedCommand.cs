using MediatR;

namespace SmartFridgeApp.API.Fridges
{
    public class MarkFridgeAsWelcomedCommand : IRequest
    {
        public MarkFridgeAsWelcomedCommand(int fridgeId)
        {
            FridgeId = fridgeId;
        }

        public int FridgeId { get; }
    }
}
