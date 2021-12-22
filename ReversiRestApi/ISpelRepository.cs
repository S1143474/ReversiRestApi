using ReversiRestApi.Json_obj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi
{
    public interface ISpelRepository
    {
        void AddSpel(Spel spel);

        public List<Spel> GetSpellen();

        bool UpdateSpel(Spel spel);

        bool JoinSpel(JoinGameObj joinGameObj);

        Spel GetSpel(string spelToken);
    }
}
