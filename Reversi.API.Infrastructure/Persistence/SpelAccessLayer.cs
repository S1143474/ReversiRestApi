using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Reversi.API.Application;
using Reversi.API.Application.Common.Helpers;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Application.Common.RequestParameters;
using Reversi.API.Domain.Entities;
using Reversi.API.Infrastructure.Persistence.SQLCommands;

namespace Reversi.API.Infrastructure.Persistence
{
    public class SpelAccessLayer : ISpelRepository
    {
        public PagedList<Spel> GetAllSpellen(QueryStringParameters parameters)
        {
            throw new NotImplementedException();
        }

        public PagedList<Spel> GetAllSpellenInQueue(QueryStringParameters parameters)
        {
            throw new NotImplementedException();
        }

        public PagedList<Spel> GetAllSpellenInProcess(QueryStringParameters parameters)
        {
            throw new NotImplementedException();
        }

        public PagedList<Spel> GetAllSpellenFinished(QueryStringParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<Spel> GetSpelByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Spel> GetSpelInQueueByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Spel> GetSpelInProcessByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Spel> GetSpelFinishedByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Spel> GetSpelInQueueBySpelerTokenAsync(Guid spelerToken)
        {
            throw new NotImplementedException();
        }

        public Task<Spel> GetSpelInProcessBySpelerTokenAsync(Guid spelerToken)
        {
            throw new NotImplementedException();
        }

        public Task<Spel> GetSpelUnFinishedBySpelerTokenAsync(Guid spelerToken)
        {
            throw new NotImplementedException();
        }

        public PagedList<Spel> GetSpellenFinishedBySpelerTokenAsync(Guid spelerToken, QueryStringParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<Spel> GetSpelInProcessFromSpelerOrSpelTokenAsync(Guid spelerToken, Guid token)
        {
            throw new NotImplementedException();
        }

        public async Task<ItemList<DBSpel>> LoadSpellenAsync(CancellationToken token)
            => await new SQLSelectCommand()
                .Select()
                .From("Spel")
                .Where("EndedAt", "IS", null)
                .Build()
                .Execute<ItemList<DBSpel>, DBSpel>();

        public async Task<ItemList<DBSpel>> LoadSpelAsync(string spelToken, CancellationToken token)
            => await new SQLSelectCommand()
                .Select()
                .From("Spel")
                .Where("Token", "=", spelToken)
                .Build()
                .Execute<ItemList<DBSpel>, DBSpel>();

        public async Task<int> InsertSpelAsync(string spelToken, string speler1Token, string omschrijving, string bord, CancellationToken token)
            => await new SQLInsertCommand()
                .Insert("Token", "Speler1Token", "Description", "Bord")
                .Into("Spel")
                .Values(spelToken, speler1Token, omschrijving, bord)
                .Build()
                .Execute();

        public async Task<ItemList<DBSpel>> LoadSpelTokenViaSpelerTokenAsync(string spelerToken, CancellationToken token)
            => await new SQLSelectCommand()
                .Select()
                .From("Spel")
                .Where("Speler1Token", "=", spelerToken)
                .And("EndedAt", "IS", null)
                .Or("Speler2Token", "=", spelerToken)
                .And("EndedAt", "IS", null)
                .Build()
                .Execute<ItemList<DBSpel>, DBSpel>();

        public async Task<int> UpdateSpelJoinAsync(string spelToken, string speler2Token,
            CancellationToken cancellationToken)
            => await new SQLUpdateCommand()
                .Update("Spel")
                .Set(new string[] { "Speler2Token", "StartedAt" },
                    new object[] { speler2Token, DateTime.Now.ToUniversalTime() })
                .Where("Token", "=", spelToken)
                .Build()
                .Execute();

        public async Task<ItemList<DBSpel>> LoadSpelFromSpelerOrSpelTokenAsync(string spelerToken, string spelToken,
            CancellationToken cancellationToken)
            => await new SQLSelectCommand()
                .Select()
                .From("Spel")
                .Where("Speler1Token", "=", spelerToken)
                .And("EndedAt", "IS", null)
                .Or("Speler2Token", "=", spelerToken)
                .And("EndedAt", "IS", null)
                .Or("Token", "=", spelToken)
                .And("EndedAt", "IS", null)
                .Build()
                .Execute<ItemList<DBSpel>, DBSpel>();

        public async Task<int> UpdateSpelBoardAndTurn(string spelToken, int beurt, string base64Board,
            CancellationToken cancellationToken)
            => await new SQLUpdateCommand()
                .Update("Spel")
                .Set(
                    new [] { "Bord", "Beurt" },
                    new object[] { base64Board, beurt })
                .Where("Token", "=", spelToken)
                .Build()
                .Execute();

        public async Task<int> UpdateSpelSetFinishAsync(string spelToken, CancellationToken cancellationToken)
            => await new SQLUpdateCommand()
                .Update("Spel")
                .Set(
                    new[] { "EndedAt" },
                    new object[] { DateTime.Now.ToUniversalTime() })
                .Where("Token", "=", spelToken)
                .Build()
                .Execute();

        public IQueryable<Spel> FindAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Spel> FindByCondition(Expression<Func<Spel, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public void Create(Spel entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Spel entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Spel entity)
        {
            throw new NotImplementedException();
        }

        public bool DeleteSpel(Guid spelToken)
        {
            throw new NotImplementedException();
        }
    }
}
