using System.Collections.Generic;
using MediatR;
using SmartFridgeApp.Core.Domain.Entities;

namespace SmartFridgeApp.Core.Application.Features
{
    public class GetCategoriesQuery : IRequest<IEnumerable<Category>>
    {
        public GetCategoriesQuery()
        {
            
        }
    }
}
