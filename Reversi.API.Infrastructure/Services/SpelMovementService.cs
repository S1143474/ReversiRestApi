using Reversi.API.Application.Common;
using System.Collections.Generic;
using Reversi.API.Application.Common.Mappings;
using Reversi.API.Application.Spellen.Commands.InProcessSpelMove.MoveModels;
using Reversi.API.Domain.Entities;
using Reversi.API.Domain.Enums;

namespace Reversi.API.Infrastructure.Services
{
    public class SpelMovementService : ISpelMovement
    {
        private const int _BORD_SIZE = 8;

        public bool Afgelopen(Spel spel)
        {
            var bord = spel.Bord.MapStringBordTo2DIntArr();
            for (int i = 0; i < bord.GetLength(0); i++)
            {
                for (int j = 0; j < bord.GetLength(1); j++)
                {
                    if (bord[i, j] != (int)Kleur.Geen) 
                        continue;
                    
                    if (ZetMogelijk(spel, i, j))
                        return false;
                }
            }

            return true;
        }

        public bool DoeZet(Spel spel, int rijZet, int kolomZet, out List<CoordsModel> flippedResult)
        {
            var bord = spel.Bord.MapStringBordTo2DIntArr();

            if (ZetMogelijk(spel, rijZet, kolomZet))
            {
                bord[rijZet, kolomZet] = spel.AandeBeurt;

                flippedResult = FlipStonesBetween(spel, rijZet, kolomZet);

                spel.AandeBeurt = (int)GetOpponentColor(spel);
                spel.Bord = bord.MapIntArrToBase64String();
                return true;
            }
            else
            {
                flippedResult = null;
                spel.Bord = bord.MapIntArrToBase64String();
                return false;
            }
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
            spel.AandeBeurt = (int)GetOpponentColor(spel);
            return true;
        }

        public bool ZetMogelijk(Spel spel, int rijZet, int kolomZet)
        {
            var bord = spel.Bord.MapStringBordTo2DIntArr();
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
                                if (bord[i, j] == (int)Kleur.Geen)
                                    continue;
                                else if (bord[i, j] == spel.AandeBeurt)
                                    continue;
                                else if (bord[i, j] == (int)GetOpponentColor(spel))
                                    if (CheckMovePossible(spel, rijZet, kolomZet, i, j))
                                    {
                                        return bord[rijZet, kolomZet] == (int)Kleur.Geen;
                                    }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public List<CoordsModel> FlipStonesBetween(Spel spel, int startY, int startX)
        {
            var cellsToFlip = new List<CoordsModel>();

            var bord = spel.Bord.MapStringBordTo2DIntArr();

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

                    while (bord[currY, currX] != spel.AandeBeurt)
                    {
                        bord[currY, currX] = spel.AandeBeurt;
                        cellsToFlip.Add(new CoordsModel { X = currX, Y = currY });

                        if (CheckOutOfBounds(currY + stepYDir) && CheckOutOfBounds(currX + stepXDir))
                        {
                            currY += stepYDir;
                            currX += stepXDir;
                        }
                    }
                }
            }
            cellsToFlip.Add(new CoordsModel { X = startX, Y = startY });
            spel.Bord = bord.MapIntArrToBase64String();
            return cellsToFlip;
        }

        public bool CheckMovePossible(Spel spel, int rijZet, int kolomZet, int y, int x)
        {
            var _diffY = y - rijZet;
            var _diffX = x - kolomZet;

            var currY = y;
            var currX = x;

            var bord = spel.Bord.MapStringBordTo2DIntArr();

/*            var _prevY = 0;
            var _prevX = 0;
*/
            for (int i = 0; i < _BORD_SIZE; i++)
            {
                if (CheckOutOfBounds(currY) && CheckOutOfBounds(currX))
                {
                    if (bord[currY, currX] == (int)Kleur.Geen)
                        return false;

                    if (bord[currY, currX] == spel.AandeBeurt)
                    {
                        /*                        _prevY = currY;
                                                _prevX = currX;*/
                        spel.Bord = bord.MapIntArrToBase64String();
                        return true;
                    }

                    currY += _diffY;
                    currX += _diffX;
                }
            }

            return false;
        }

        public bool CheckOutOfBounds(int bound) => (bound >= 0 && bound < _BORD_SIZE);


        public Kleur GetOpponentColor(Spel spel)
        {
            var result =  spel.AandeBeurt.Equals((int)Kleur.Zwart) ? Kleur.Wit : Kleur.Zwart;
            return result;
        } 

    }
}