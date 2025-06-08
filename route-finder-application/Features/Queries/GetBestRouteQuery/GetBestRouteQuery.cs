using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.Finder.Application.Features.Queries.GetBestRouteQuery
{
    public record GetBestRouteQuery(string Origin, string Destination) : IRequest<GetBestRouteQueryResponse>;
}
