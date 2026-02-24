using System;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.Profiles.Commands;

public class DeletePhoto
{
    public class Command : IRequest<Results<Unit>>
    {
        public required string PhotoId { get; set; }
    }

    public class Handler(AppDbContext context, IUserAccessor userAccessor,
        IPhotoService photoService) : IRequestHandler<Command, Results<Unit>>
    {
        public async Task<Results<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await userAccessor.GetUserWithPhotosAsync();

            var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);

            if (photo == null) return Results<Unit>.Failure("Cannot find photo", 400);

            if (photo.Url == user.ImageUrl) return Results<Unit>.Failure("Cannot delete main photo", 400);

            await photoService.DeletePhoto(photo.PublicId);

            user.Photos.Remove(photo);

            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            return result
                ? Results<Unit>.Success(Unit.Value)
                : Results<Unit>.Failure("Problem deleting photo", 400);
        }
    }
}
