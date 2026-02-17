using MediatR;
using Domain;
using Persistence;
using AutoMapper;
using Application.Core;
using Application.Activities.DTOs;

namespace Application.Activities.Commands;

public class EditActivity
{
    public class Command : IRequest<Results<Unit>>
    {
        public required EditActivityDto ActivityDto { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Command, Results<Unit>>
    {
        public async Task<Results<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activityToEdit = await context.Activities
                .FindAsync([request.ActivityDto.Id], cancellationToken);
            
            if (activityToEdit == null) return Results<Unit>.Failure("Activity not found", 404);

            mapper.Map(request.ActivityDto, activityToEdit);
            
            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Results<Unit>.Failure("Failed to update the activity", 400);

            return Results<Unit>.Success(Unit.Value);
        }
    }
}
