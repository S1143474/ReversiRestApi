using ReversiRestApi.Json_obj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReversiRestApi
{
    public class SpelRepository : ISpelRepository
    {
        // Lijst met tijdelijke spellen
        public List<Spel> Spellen { get; set; }

        public SpelRepository()
        {
            Spel spel1 = new Spel();
            Spel spel2 = new Spel();
            Spel spel3 = new Spel();

            spel1.Token = "abcdef";
            spel1.Speler1Token = "abcdef";
            spel1.Omschrijving = "Potje snel reversi, dus niet lang nadenken";

            spel2.Speler1Token = "ghijkl";
            spel2.Speler2Token = "mnoqpr";
            spel2.Omschrijving = "Ik zoek een gevorderde tegenspeler!";

            spel3.Speler1Token = "stuvwx";
            spel3.Omschrijving = "Na dit spel wil ik er nog een paar spelen tegen zelfde tegenstander";

            Spellen = new List<Spel> { spel1, spel2, spel3 };
        }

        /// <summary>
        /// Adds spel to Spellen List
        /// </summary>
        /// <param name="spel"></param>
        public async Task AddSpel(CancellationToken token, Spel spel) => Spellen.Add(spel);

        /// <summary>
        /// Retrieves a Spel via a specific spelToken
        /// </summary>
        /// <param name="spelToken"></param>
        /// <returns></returns>
        public async Task<Spel> GetSpel(CancellationToken token, string spelToken)
        {
            return Spellen.Where(spel => spel.Token != null && spel.Token.Equals(spelToken)).Select(spel => spel).FirstOrDefault();
        }

        public Task<Spel> GetSpelFromSpeler1(CancellationToken token, string speler1Token)
        {
            throw new NotImplementedException();
        }

        public Task<Spel> GetSpelFromSpeler2(CancellationToken token, string speler2Token)
        {
            throw new NotImplementedException();
        }

        public Task<Spel> SelectSpelTokenViaSpelerToken(CancellationToken token, string spelerToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns list of all Spel objects in Spellen
        /// </summary>
        /// <returns></returns>
        public async Task<List<Spel>> GetSpellenAsync(CancellationToken token)
        {
            return Spellen;
        }

        public Task<bool> UpdateSpel(CancellationToken token, Spel spel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> JoinSpel(CancellationToken token, JoinGameObj joinGameObj)
        {
            throw new NotImplementedException();
        }
    }
}
