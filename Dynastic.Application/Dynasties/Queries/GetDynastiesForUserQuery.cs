using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Application.Dynasties.Queries
{
    public class GetDynastiesForUserQuery : IRequest<List<Dynasty>>
    {
        public string? UserId { get; set; }
    }

    public class GetDynastiesForUserQueryHandler : IRequestHandler<GetDynastiesForUserQuery, List<Dynasty>>
    {
        private readonly IApplicationDbContext _context;
        public GetDynastiesForUserQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Dynasty>> Handle(GetDynastiesForUserQuery request, CancellationToken cancellationToken)
        {
            return await _context.UserDynasties
                .Where(d => d.UserId.Equals(request.UserId))
                .Include(d => d.Dynasty)
                .Select(d => d.Dynasty)
                .ToListAsync(cancellationToken);
        }
    }
}
