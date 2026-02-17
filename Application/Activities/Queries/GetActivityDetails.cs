using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Queries;

public class GetActivityDetails
{
    public class Query : IRequest<Results<Activity>>
    {
        public required string Id { get; set; }
    }

    public class Handler(AppDbContext appDbContext) : IRequestHandler<Query, Results<Activity>>
    {
        public async Task<Results<Activity>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await appDbContext.Activities.FindAsync([request.Id], cancellationToken);
            
            if (activity == null) return Results<Activity>.Failure("Activity NOT FOUND!", 404);
            
            return Results<Activity>.Success(activity);
        }
    }
}
