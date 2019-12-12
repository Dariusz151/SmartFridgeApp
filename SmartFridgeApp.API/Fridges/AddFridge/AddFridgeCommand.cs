using MediatR;

namespace SmartFridgeApp.API.Fridges.AddFridge
{
    public class AddFridgeCommand : IRequest<FridgeDto>
    {
        public string Name { get; }
        public string Address { get; }
        public string Desc { get; }
        
        public AddFridgeCommand(string name, string address, string desc)
        {
            Name = name;
            Address = address;
            Desc = desc;
        }
    }
}
