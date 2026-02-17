using Application.Activities.DTOs;
using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Commands;

public class CreateActivity
{
    public class Command : IRequest<Results<string>>
    {
        public required CreateActivityDto ActivityDto { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Command, Results<string>>
    {
        public async Task<Results<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = mapper.Map<Activity>(request.ActivityDto);
            context.Activities.Add(activity);

            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Results<string>.Failure("Failed to create the activity", 400);
            return Results<string>.Success(activity.Id);
        }
    }
}
