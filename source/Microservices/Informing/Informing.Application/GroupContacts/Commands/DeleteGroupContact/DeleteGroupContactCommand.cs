using BaseApplication.Exceptions;
using Informing.Application.Interfaces;
using MediatR;

namespace Informing.Application.GroupContacts.Commands.DeleteGroupContact
{

    public record DeleteGroupContactCommand(long id) : IRequest<Unit>;

    public class DeleteGroupContactCommandHandler : IRequestHandler<DeleteGroupContactCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public DeleteGroupContactCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task<Unit> Handle(DeleteGroupContactCommand request, CancellationToken cancellationToken)
        //{
        //    var entity = await _context.groupContacts
        //        .FindAsync(new object[] { request.id }, cancellationToken);

        //    if (entity == null)
        //    {
        //        throw new NotFoundException(nameof(Domain.Entities.GroupContact), request.id);
        //    }

        //    _context.groupContacts.Remove(entity);

        //    await _context.SaveChangesAsync(cancellationToken);

        //    return Unit.Value;
        //}
        public async Task<Unit> Handle(DeleteGroupContactCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.groupContacts
                .FindAsync(new object[] { request.id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.GroupContact), request.id);
            }

            _context.groupContacts.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}