using Application.Core;
using Application.Profiles.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles.Queries;

public class GetProfile
{
    public class Query : IRequest<Results<UserProfile>>
    {
        public required string UserId { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper) 
        : IRequestHandler<Query, Results<UserProfile>>
    {
        public async Task<Results<UserProfile>> Handle(Query request, CancellationToken cancellationToken)
        {
            var profile = await context.Users
                .ProjectTo<UserProfile>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            return profile == null
                ? Results<UserProfile>.Failure("Profile not found", 404)
                : Results<UserProfile>.Success(profile);
        }
    }
}
