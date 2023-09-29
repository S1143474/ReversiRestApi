using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Reversi.API.Application.Common.Helpers;
using Reversi.API.Application.Common.RequestParameters;
using Reversi.API.Domain.Entities;

namespace Reversi.API.Application.Common.Interfaces
{
    public interface ISpelRepository : IBaseRepository<Spel>
    {
        bool DeleteSpel(Guid spelToken);
        PagedList<Spel> GetAllSpellen(QueryStringParameters parameters);
        PagedList<Spel> GetAllSpellenInQueue(QueryStringParameters parameters);
        PagedList<Spel> GetAllSpellenInProcess(QueryStringParameters parameters);
        PagedList<Spel> GetAllSpellenFinished(QueryStringParameters parameters);

        Task<Spel> GetSpelByIdAsync(Guid id);
        Task<Spel> GetSpelInQueueByIdAsync(Guid id);
        Task<Spel> GetSpelInProcessByIdAsync(Guid id);
        Task<Spel> GetSpelFinishedByIdAsync(Guid id);

        Task<Spel> GetSpelInQueueBySpelerTokenAsync(Guid spelerToken);
        Task<Spel> GetSpelInProcessBySpelerTokenAsync(Guid spelerToken);
        Task<Spel> GetSpelUnFinishedBySpelerTokenAsync(Guid spelerToken);
        PagedList<Spel> GetSpellenFinishedBySpelerTokenAsync(Guid spelerToken, QueryStringParameters parameters);

        Task<Spel> GetSpelInProcessFromSpelerOrSpelTokenAsync(Guid spelerToken, Guid token);

        Task<ItemList<DBSpel>> LoadSpellenAsync(CancellationToken token);
        Task<ItemList<DBSpel>> LoadSpelAsync(string spelToken, CancellationToken token);
        Task<int> InsertSpelAsync(string spelToken, string speler1Token, string omschrijving, string bord, CancellationToken token);
        Task<ItemList<DBSpel>> LoadSpelTokenViaSpelerTokenAsync(string spelerToken, CancellationToken token);
        Task<int> UpdateSpelJoinAsync(string spelToken, string speler2Token, CancellationToken token);
        Task<ItemList<DBSpel>> LoadSpelFromSpelerOrSpelTokenAsync(string spelerToken, string spelToken, CancellationToken cancellationToken);
        Task<int> UpdateSpelBoardAndTurn(string spelToken, int beurt, string base64Board,
            CancellationToken cancellationToken);

        Task<int> UpdateSpelSetFinishAsync(string spelToken, CancellationToken cancellationToken);










        /*      
              Task<Spel> LoadSpelAsync(string spelToken, CancellationToken token);

              Task<Spel> LoadNotEndedSpelFromSpeler1TokenAsync(string speler1Token, CancellationToken token);

              Task<Spel> LoadNotEndedSpelFromSpeler2TokenAsync(string speler2Token, CancellationToken token);

             

              Task<bool> UpdateSpelBordAndBeurtAsync(Spel spel, CancellationToken token);

              Task<bool> UpdateSpelJoinAsync(JoinGameObj joingameObj, CancellationToken token);

              Task<bool> UpdateSpelIsFinishedAsync(string spelToken, CancellationToken token);
         */
    }
}
