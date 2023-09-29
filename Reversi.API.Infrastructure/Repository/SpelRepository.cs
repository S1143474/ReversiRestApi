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

        public bool DeleteSpel(Guid spelToken)
        {
            var spel = FindByCondition(s => s.Token.Equals(spelToken)).FirstOrDefault();

            if (spel == null)
                return false;


            Delete(spel);
            RepositoryContext.SaveChanges();

            spel = FindByCondition(s => s.Token.Equals(spelToken)).FirstOrDefault();
            return spel == null;
        }

        public PagedList<Spel> GetAllSpellen(QueryStringParameters parameters)
        {
            var spellen = FindAll()
                .OrderBy(spel => spel.StartedAt);

            var sortedSpellen = _sortHelper.ApplySort(spellen, parameters.OrderBy);

            return PagedList<Spel>
                .ToPagedList(sortedSpellen, parameters.PageNumber, parameters.PageSize);
        }

        public PagedList<Spel> GetAllSpellenInQueue(QueryStringParameters parameters)
        {
            var spellenInQueue = FindByCondition(
                spel => spel.Speler2Token == null && spel.StartedAt == null);

            var sortedSpellenInQueue = _sortHelper.ApplySort(spellenInQueue, parameters.OrderBy);
            
            return PagedList<Spel>
                .ToPagedList(sortedSpellenInQueue, parameters.PageNumber, parameters.PageSize);
        }

        public PagedList<Spel> GetAllSpellenInProcess(QueryStringParameters parameters)
        {
            var spellenInProcess = FindByCondition(
                spel => spel.StartedAt != null &&
                        spel.FinishedAt == null);

            var sortedSpellenInProcess = _sortHelper.ApplySort(spellenInProcess, parameters.OrderBy);

            return PagedList<Spel>
                .ToPagedList(sortedSpellenInProcess, parameters.PageNumber, parameters.PageSize);
        }

        public PagedList<Spel> GetAllSpellenFinished(QueryStringParameters parameters)
        {
            var spellenFinished = FindByCondition(spel =>
                spel.FinishedAt != null);

            var sortedSpellenFinished = _sortHelper.ApplySort(spellenFinished, parameters.OrderBy);

            return PagedList<Spel>
                .ToPagedList(sortedSpellenFinished, parameters.PageNumber, parameters.PageSize);
        }

        // ---- Get Spel Entities Based On Token ----
        public async Task<Spel> GetSpelByIdAsync(Guid id)
        {
            return await FindByCondition(spel => 
                    spel.Token.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<Spel> GetSpelInQueueByIdAsync(Guid id)
        {
            return await FindByCondition(spel =>
                spel.Speler2Token == null &&
                spel.StartedAt == null &&
                spel.Token.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<Spel> GetSpelInProcessByIdAsync(Guid id)
        {
            return await FindByCondition(spel => 
                    spel.StartedAt != null &&
                    spel.FinishedAt == null &&
                    spel.Token.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<Spel> GetSpelFinishedByIdAsync(Guid id)
        {
            return await FindByCondition(spel =>
                spel.Token.Equals(id) &&
                spel.FinishedAt != null).FirstOrDefaultAsync();
        }

        // ---- Get Spel Entities Based On SpelerToken ----
        public async Task<Spel> GetSpelInQueueBySpelerTokenAsync(Guid spelerToken)
        {
            return await FindByCondition(spel => 
                spel.StartedAt == null &&
                spel.FinishedAt == null &&
                (spel.Speler1Token.Equals(spelerToken))).FirstOrDefaultAsync();
        }

        public async Task<Spel> GetSpelInProcessBySpelerTokenAsync(Guid spelerToken)
        {
            return await FindByCondition(spel =>
                    spel.StartedAt != null &&
                    spel.FinishedAt == null &&
                    (spel.Speler1Token.Equals(spelerToken) || 
                     spel.Speler2Token.Equals(spelerToken)))
                .FirstOrDefaultAsync();
        }

        public PagedList<Spel> GetSpellenFinishedBySpelerTokenAsync(Guid spelerToken, QueryStringParameters parameters)
        {
            var spellenFinished = FindByCondition(spel =>
                spel.FinishedAt != null &&
                (spel.Speler1Token.Equals(spelerToken) ||
                 spel.Speler2Token.Equals(spelerToken)));

            var sortedSpellenFinished = _sortHelper.ApplySort(spellenFinished, parameters.OrderBy);

            return PagedList<Spel>
                .ToPagedList(sortedSpellenFinished, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Spel> GetSpelUnFinishedBySpelerTokenAsync(Guid spelerToken)
        {
            return await FindByCondition(spel => 
                spel.FinishedAt == null &&
                (spel.Speler1Token.Equals(spelerToken) ||
                 spel.Speler2Token.Equals(spelerToken))).FirstOrDefaultAsync();
        }

        public async Task<Spel> GetSpelInProcessFromSpelerOrSpelTokenAsync(Guid spelerToken, Guid token)
        {
            return await FindByCondition(spel =>
                    spel.StartedAt != null &&
                    spel.FinishedAt == null &&
                    (spel.Speler1Token.Equals(spelerToken) ||
                     spel.Speler2Token.Equals(spelerToken) ||
                     spel.Token.Equals(token)))
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
