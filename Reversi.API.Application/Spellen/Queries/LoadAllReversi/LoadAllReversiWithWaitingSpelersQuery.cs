using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Domain.Entities;

namespace Reversi.API.Application.Spellen.Queries.LoadAllReversi
{
    public class LoadAllReversiWithWaitingSpelersQuery : IRequest<SpelList>
    {

    }

    public class LoadAllReversiWithWaitingSpelersQueryHandle : IRequestHandler<LoadAllReversiWithWaitingSpelersQuery, SpelList>
    {
        private readonly ISpelRepository _spelRepository;
        public LoadAllReversiWithWaitingSpelersQueryHandle(ISpelRepository repository)
        {
            _spelRepository = repository;
        }

        public async Task<SpelList> Handle(LoadAllReversiWithWaitingSpelersQuery request, CancellationToken cancellationToken)
        {
            var result = await _spelRepository.LoadSpellenAsync(cancellationToken);
            return result;
        }
    }
}
