//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Threading.Tasks;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using SmartFridgeApp.API.Fridges;
//using SmartFridgeApp.Infrastructure;
//using SmartFridgeApp.Infrastructure.InternalCommands;

//namespace SmartFridgeApp.API.InternalCommands
//{
//    public class CommandsDispatcher : ICommandsDispatcher
//    {
//        private readonly IMediator _mediator;
//        private readonly SmartFridgeAppContext _context;

//        public CommandsDispatcher(
//            IMediator mediator,
//            SmartFridgeAppContext context)
//        {
//            this._mediator = mediator;
//            this._context = context;
//        }

//        public async Task DispatchCommandAsync(Guid id)
//        {
//            var command = await this._context.InternalCommands.SingleOrDefaultAsync(x => x.Id == id);

//            Type type = Assembly.GetAssembly(typeof(MarkFridgeAsWelcomedCommand)).GetType(command.Type);
//            var request = JsonConvert.DeserializeObject(command.Data, type);

//            command.ProcessedDate = DateTime.UtcNow;

//            await this._mediator.Send((IRequest)request);
//        }
//    }
//}
