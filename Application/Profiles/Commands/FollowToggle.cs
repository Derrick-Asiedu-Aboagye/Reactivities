using System;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Persistence;

namespace Application.Profiles.Commands;

public class FollowToggle
{
    public class Command : IRequest<Results<Unit>>
    {
        public required string TargetUserId { get; set; }
    }

    public class Handler(AppDbContext context, IUserAccessor userAccessor) : IRequestHandler<Command, Results<Unit>>
    {
        public async Task<Results<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var observer = await userAccessor.GetUserAsync();
            var target = await context.Users.FindAsync([request.TargetUserId], cancellationToken);

            if (target == null) return Results<Unit>.Failure("Target user not found", 400);

            var following = await context.UserFollowings
                .FindAsync([observer.Id, target.Id], cancellationToken);

            if (following == null)
            {
                context.UserFollowings.Add(new UserFollowing
                {
                    ObserverId = observer.Id,
                    TargetId = target.Id
                });
            }
            else context.UserFollowings.Remove(following);

            return await context.SaveChangesAsync(cancellationToken) > 0
                ? Results<Unit>.Success(Unit.Value)
                : Results<Unit>.Failure("Problem updating following", 400);
        }
    }
}