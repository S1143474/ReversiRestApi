using Reversi.API.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Domain.Entities;
using Reversi.API.Domain.Enums;
using Reversi.API.Infrastructure.Maps;

namespace Reversi.API.Infrastructure.Services
{
    public class SpelMovementService : ISpelMovement
    {
        private const int _BORD_SIZE = 8;

        public bool Afgelopen(Spel spel)
        {
            for (int i = 0; i < spel.Bord.GetLength(0); i++)
            {
                for (int j = 0; j < spel.Bord.GetLength(1); j++)
                {
                    if (spel.Bord[i, j] == Kleur.Geen)
                    {
                        if (ZetMogelijk(spel, i, j))
                            return false;
                    }
                }
            }

            return true;
        }
        public bool DoeZet(Spel spel, int rijZet, int kolomZet)
        {
            if (ZetMogelijk(spel, rijZet, kolomZet))
            {
                spel.Bord[rijZet, kolomZet] = spel.AandeBeurt;

                FlipStonesBetween(spel, rijZet, kolomZet);


                spel.AandeBeurt = GetOpponentColor(spel);
                return true;
            }
            else
                return false;
        }

        public Kleur OverwegendeKleur(Kleur[,] bord)
        {
            int zwartCount = 0, witCount = 0;

            foreach (Kleur color in bord)
            {
                if (color == Kleur.Wit) witCount++;
                if (color == Kleur.Zwart) zwartCount++;
            }

            return (zwartCount == witCount) ? Kleur.Geen : (zwartCount > witCount) ? Kleur.Zwart : Kleur.Wit;
        }

        public int[] GedraaideFiches(Kleur[,] bord)
        {
            var result = new int[3];

            foreach (var color in bord)
                result[(int)color]++;

            return result;
        }

        public bool Pas(Spel spel)
        {
            spel.AandeBeurt = GetOpponentColor(spel);
            return true;
        }

        public bool ZetMogelijk(Spel spel, int rijZet, int kolomZet)
        {
            // Check if Row(Y) - rijzet And Col(X) kolomzet are in between the boundaries of the bord.
            if (CheckOutOfBounds(rijZet) && CheckOutOfBounds(kolomZet))
            {
                for (int i = rijZet - 1; i <= rijZet + 1; i++)
                {
                    if (CheckOutOfBounds(i))
                    {
                        for (int j = kolomZet - 1; j <= kolomZet + 1; j++)
                        {
                            if (CheckOutOfBounds(j))
                            {
                                if (spel.Bord[i, j] == Kleur.Geen)
                                    continue;
                                else if (spel.Bord[i, j] == spel.AandeBeurt)
                                    continue;
                                else if (spel.Bord[i, j] == GetOpponentColor(spel))
                                    if (CheckMovePossible(spel, rijZet, kolomZet, i, j))
                                        return spel.Bord[rijZet, kolomZet] == Kleur.Geen;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public IEnumerable<IFiche> FlipStonesBetween(Spel spel, int startY, int startX)
        {
            var cellsToFlip = new List<FicheSeriesMap>();

            for (int i = startY - 1; i <= startY + 1; i++)
            {
                for (int j = startX - 1; j <= startX + 1; j++)
                {
                    if (!CheckOutOfBounds(i) || !CheckOutOfBounds(j)) continue;

                    if (!CheckMovePossible(spel, startY, startX, i, j))
                        continue;
                    int currY = startY;
                    int currX = startX;
                    int stepYDir = i - currY;
                    int stepXDir = j - currX;

                    currY += stepYDir;
                    currX += stepXDir;

                    Kleur tempKLeur = GetOpponentColor(spel);

                    while (spel.Bord[currY, currX] != spel.AandeBeurt)
                    {
                        spel.Bord[currY, currX] = spel.AandeBeurt;
                        cellsToFlip.Add(new FicheSeriesMap() { X = currX, Y = currY });

                        if (CheckOutOfBounds(currY + stepYDir) && CheckOutOfBounds(currX + stepXDir))
                        {
                            currY += stepYDir;
                            currX += stepXDir;
                        }
                    }
                }
            }
            cellsToFlip.Add(new FicheSeriesMap() { X = startX, Y = startY });
            return cellsToFlip;
        }

        public bool CheckMovePossible(Spel spel, int rijZet, int kolomZet, int y, int x)
        {
            var _diffY = y - rijZet;
            var _diffX = x - kolomZet;

            var currY = y;
            var currX = x;

/*            var _prevY = 0;
            var _prevX = 0;
*/
            for (int i = 0; i < _BORD_SIZE; i++)
            {
                if (CheckOutOfBounds(currY) && CheckOutOfBounds(currX))
                {
                    if (spel.Bord[currY, currX] == Kleur.Geen)
                        return false;

                    if (spel.Bord[currY, currX] == spel.AandeBeurt)
                    {
/*                        _prevY = currY;
                        _prevX = currX;*/
                        return true;
                    }

                    currY += _diffY;
                    currX += _diffX;
                }
            }

            return false;
        }

        public bool CheckOutOfBounds(int bound) => (bound >= 0 && bound < _BORD_SIZE);


        public Kleur GetOpponentColor(Spel spel) => spel.AandeBeurt.Equals(Kleur.Zwart) ? Kleur.Wit : Kleur.Zwart;

    }
}
