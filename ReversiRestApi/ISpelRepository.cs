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
        Task AddSpel(CancellationToken token, Spel spel);

        public Task<List<Spel>> GetSpellenAsync(CancellationToken token);

        Task<bool> UpdateSpel(CancellationToken token, Spel spel);

        Task<bool> JoinSpel(CancellationToken token, JoinGameObj joinGameObj);

        Task<Spel> GetSpel(CancellationToken token, string spelToken);

        Task<Spel> GetSpelFromSpeler1(CancellationToken token, string speler1Token);
        Task<Spel> GetSpelFromSpeler2(CancellationToken token, string speler2Token);
    }
}
