using Dynastic.Domain.Entities;
using Dynastic.Domain.Interface;
using Dynastic.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Application.Dynasties.Queries
{
    public class GetDynastyByIdQuery : IRequest<Dynasty>
    {
        public Guid Id { get; set; }
    }

    public class GetDynastyByIdQueryHandler : IRequestHandler<GetDynastyByIdQuery, Dynasty>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetDynastyByIdQueryHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Dynasty> Handle(GetDynastyByIdQuery request, CancellationToken cancellationToken)
        {
            var ud = await _context.UserDynasties
                .Where(u => u.UserId.Equals(_currentUserService.UserId) && u.DynastyId.Equals(request.Id))
                .Include(ud => ud.Dynasty)
                .ThenInclude(d => d.Members)
                .FirstOrDefaultAsync(cancellationToken);
            return ud?.Dynasty;
        }
    }
}
