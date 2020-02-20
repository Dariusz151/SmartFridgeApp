using MediatR;

namespace SmartFridgeApp.API.Fridges.DeleteFridge
{
    public class DeleteFridgeCommand : IRequest
    {
        public int FridgeId { get; }

        public DeleteFridgeCommand(int fridgeId)
        {
            FridgeId = fridgeId;
        }
    }
}
