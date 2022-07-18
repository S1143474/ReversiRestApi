using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Reversi.API.Application;
using Reversi.API.Application.Common.Helpers;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Application.Common.RequestParameters;
using Reversi.API.Domain.Entities;
using Reversi.API.Infrastructure.Persistence;

namespace Reversi.API.Infrastructure.Repository
{
    public class SpelRepository : BaseRepository<Spel>, ISpelRepository
    {
        private ISortHelper<Spel> _sortHelper;

        public SpelRepository(RepositoryContext repositoryContext, ISortHelper<Spel> sortHelper) 
            : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public PagedList<Spel> GetAllSpellen(QueryStringParameters parameters)
        {
            var spellen = FindAll()
                .OrderBy(spel => spel.StartedAt);

            var sortedSpellen = _sortHelper.ApplySort(spellen, parameters.OrderBy);

            return PagedList<Spel>
                .ToPagedList(sortedSpellen, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Spel> GetSpelByIdAsync(Guid id)
        {
            return await FindByCondition(spel => 
                    spel.Token.Equals(id))
                .FirstOrDefaultAsync();
        }

        public Task<ItemList<DBSpel>> LoadSpellenAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<ItemList<DBSpel>> LoadSpelAsync(string spelToken, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertSpelAsync(string spelToken, string speler1Token, string omschrijving, string bord, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<ItemList<DBSpel>> LoadSpelTokenViaSpelerTokenAsync(string spelerToken, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateSpelJoinAsync(string spelToken, string speler2Token, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<ItemList<DBSpel>> LoadSpelFromSpelerOrSpelTokenAsync(string spelerToken, string spelToken, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateSpelBoardAndTurn(string spelToken, int beurt, string base64Board, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateSpelSetFinishAsync(string spelToken, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
