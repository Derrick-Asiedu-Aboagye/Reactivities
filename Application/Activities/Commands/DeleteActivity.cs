using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Commands;

public class DeleteActivity
{
    public class Command : IRequest<Results<Unit>>
    {
        public required string Id { get; set; }
    }

    public class Handler(AppDbContext context) : IRequestHandler<Command, Results<Unit>>
    {
        public async Task<Results<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activityToDelete = await context.Activities
                .FindAsync([request.Id], cancellationToken);
            
            if (activityToDelete == null) return Results<Unit>.Failure("Activity not found", 404);

            context.Remove(activityToDelete);
            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Results<Unit>.Failure("Failed to delete the activity", 400);

            return Results<Unit>.Success(Unit.Value);
        }
    }
}
