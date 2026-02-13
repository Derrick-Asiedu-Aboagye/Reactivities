using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Queries;

public class GetActivityDetails
{
    public class Query : IRequest<Activity>
    {
        public required string Id { get; set; }
    }

    public class Handler(AppDbContext appDbContext) : IRequestHandler<Query, Activity>
    {
        public async Task<Activity> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await appDbContext.Activities.FindAsync([request.Id], cancellationToken);
            
            if (activity == null) throw new Exception("Activity NOT FOUND!");
            
            return activity;
        }
    }
}
