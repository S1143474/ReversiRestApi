using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi.API.Domain.Entities
{
    public class FicheList
    {
        public IList<Fiche> Fiches { get; private set; } = new List<Fiche>();
    }
}
