using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Infrastructure
{
    public class DomainEventsDispatcherCommandHandlerDecorator<T> : IRequestHandler<T, Unit> where T : IRequest
    {
        private readonly IRequestHandler<T, Unit> _decorated;
        private readonly IUnitOfWork _unitOfWork;

        public DomainEventsDispatcherCommandHandlerDecorator(
            IRequestHandler<T, Unit> decorated,
            IUnitOfWork unitOfWork)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(T command, CancellationToken cancellationToken)
        {
            Console.WriteLine("In decorator: before handle.");
            await this._decorated.Handle(command, cancellationToken);

            await this._unitOfWork.CommitAsync(cancellationToken);

            Console.WriteLine("In decorator: after unitOfWork.Commit.");



            return Unit.Value;
        }
    }
}
