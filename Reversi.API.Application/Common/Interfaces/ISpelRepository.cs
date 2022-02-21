using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Reversi.API.Domain.Entities;

namespace Reversi.API.Application.Common.Interfaces
{
    public interface ISpelRepository
    {
        Task<SpelList> LoadSpellenAsync(CancellationToken token);

        /*      Task<bool> InsertSpelAsync(Spel spel, CancellationToken token);

              Task<Spel> LoadSpelAsync(string spelToken, CancellationToken token);

              Task<Spel> LoadNotEndedSpelFromSpeler1TokenAsync(string speler1Token, CancellationToken token);

              Task<Spel> LoadNotEndedSpelFromSpeler2TokenAsync(string speler2Token, CancellationToken token);

              Task<string> LoadSpelTokenViaSpelerTokenAsync(string spelerToken, CancellationToken token);

              Task<bool> UpdateSpelBordAndBeurtAsync(Spel spel, CancellationToken token);

              Task<bool> UpdateSpelJoinAsync(JoinGameObj joingameObj, CancellationToken token);

              Task<bool> UpdateSpelIsFinishedAsync(string spelToken, CancellationToken token);
         */
    }
}
