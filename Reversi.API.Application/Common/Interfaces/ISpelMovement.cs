using System;
using System.Collections.Generic;
using System.Text;
using Reversi.API.Application.Common.Interfaces;
/*using Reversi.API.Application.Spellen.DTO;
*/using Reversi.API.Domain.Entities;
using Reversi.API.Domain.Enums;

namespace Reversi.API.Application.Common
{
    public interface ISpelMovement
    {
        bool Afgelopen(Spel spel);

       /* bool DoeZet(Spel spel, int rijZet, int kolomZet, out List<CoordsDTO> flippedResult);*/

        Kleur OverwegendeKleur(Kleur[,] bord);

        int[] GedraaideFiches(Kleur[,] bord);

        bool Pas(Spel spel);

        bool ZetMogelijk(Spel spel, int rijZet, int kolomZet);

        // Methods below used to be private.

       /* List<CoordsDTO> FlipStonesBetween(Spel spel, int startY, int startX); // TODO: Something with CellsToFlip List*/

        bool CheckMovePossible(Spel spel, int rijZet, int kolomZet, int y, int x);

        bool CheckOutOfBounds(int bound);

        Kleur GetOpponentColor(Spel spel);
    }
}
