using BaseApplication.Exceptions;
using Informing.Application.Interfaces;
using MediatR;

namespace Informing.Application.GroupContacts.Commands.DeleteGroupContact
{

    public record DeleteGroupContactCommand(long id) : IRequest<Unit>;

    public class DeleteGroupContactCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteGroupContactCommand, Unit>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Unit> Handle(DeleteGroupContactCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.GroupContacts
                .FindAsync([request.id], cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.GroupContact), request.id);
            }

            _context.GroupContacts.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}