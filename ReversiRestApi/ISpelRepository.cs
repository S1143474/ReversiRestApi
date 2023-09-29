using ReversiRestApi.Json_obj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReversiRestApi
{
    public interface ISpelRepository
    {
        // Old methods
        Task AddSpel(CancellationToken token, Spel spel);

        public Task<List<Spel>> GetSpellenAsync(CancellationToken token);

        Task<bool> UpdateSpel(CancellationToken token, Spel spel);

        Task<bool> JoinSpel(CancellationToken token, JoinGameObj joinGameObj);

        Task<Spel> GetSpel(CancellationToken token, string spelToken);

        Task<Spel> GetSpelFromSpeler1(CancellationToken token, string speler1Token);
        Task<Spel> GetSpelFromSpeler2(CancellationToken token, string speler2Token);

        Task<Spel> SelectSpelTokenViaSpelerToken(CancellationToken token, string spelerToken);

        Task<bool> FinishSpel(CancellationToken token, string spelToken);

        // New methods
        /*Task<bool> InsertSpelAsync(Spel spel, CancellationToken token);

        Task<Spel> LoadSpelAsync(string spelToken, CancellationToken token);

        Task<Spel> LoadNotEndedSpelFromSpeler1TokenAsync(string speler1Token, CancellationToken token);

        Task<Spel> LoadNotEndedSpelFromSpeler2TokenAsync(string speler2Token, CancellationToken token);

        Task<IEnumerable<Spel>> LoadSpellenAsync(CancellationToken token);

        Task<string> LoadSpelTokenViaSpelerTokenAsync(string spelerToken, CancellationToken token);

        Task<bool> UpdateSpelBordAndBeurtAsync(Spel spel, CancellationToken token);

        Task<bool> UpdateSpelJoinAsync(JoinGameObj joingameObj, CancellationToken token);

        Task<bool> UpdateSpelIsFinishedAsync(string spelToken, CancellationToken token);
        */

    }
}
