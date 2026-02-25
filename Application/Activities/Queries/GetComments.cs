using System;
using Application.Activities.DTOs;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;

namespace Application.Activities.Queries;

public class GetComments
{
    public class Query : IRequest<Results<List<CommentDto>>>
    {
        public required string ActivityId { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper)
        : IRequestHandler<Query, Results<List<CommentDto>>>
    {
        public async Task<Results<List<CommentDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var comments = await context.Comments
                .Where(x => x.ActivityId == request.ActivityId)
                .OrderByDescending(x => x.CreatedAt)
                .ProjectTo<CommentDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Results<List<CommentDto>>.Success(comments);
        }
    }
}
