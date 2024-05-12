using Reversi.API.Application.Common;
using System.Collections.Generic;
using Reversi.API.Application.Common.Mappings;
using Reversi.API.Application.Spellen.Commands.InProcessSpelMove.MoveModels;
using Reversi.API.Domain.Entities;
using Reversi.API.Domain.Enums;
using System;
using System.Linq;

namespace Reversi.API.Infrastructure.Services
{

    public class SpelMovementService : ISpelMovement
    {
        private HashSet<CoordsModel> _ficheList = new HashSet<CoordsModel>();

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
                    
                    if (ZetMogelijk(ref spel, i, j))
                        return false;
                }
            }

            return true;
        }

        public bool DoeZet(ref Spel spel, int rijZet, int kolomZet, out List<CoordsModel> flippedFiches)
        {
            flippedFiches = new List<CoordsModel>();
            _ficheList = new HashSet<CoordsModel>();

            var bord = spel.Bord.MapStringBordTo2DIntArr();

            if (!CheckOutOfBounds(rijZet) && !CheckOutOfBounds(kolomZet))
                return false;

            if (!ZetMogelijk(ref spel, rijZet, kolomZet))
                return false;
            spel.Bord = FlipStonesBetween(ref spel, _ficheList).MapIntArrToBase64String();

            bord[rijZet, kolomZet] = spel.AandeBeurt;
            spel.Bord = bord.MapIntArrToBase64String();
            flippedFiches = _ficheList.ToList();
           /* flippedFiches.Add(new CoordsModel { Y = rijZet, X = kolomZet });*/
            spel.AandeBeurt = (int)GetOpponentColor(spel);



            return true;
        }

        public bool ZetMogelijk(ref Spel spel, int rijZet, int kolomZet)
        {
            var bord = spel.Bord.MapStringBordTo2DIntArr();
            int possibilitiesCount = 0;

            if (!CheckOutOfBounds(rijZet) || !CheckOutOfBounds(kolomZet))
                return false;

            if (bord[rijZet, kolomZet] != (int)Kleur.Geen)
                return false;

            for (int i = rijZet - 1; i <= rijZet + 1; i++)
            {
                if (!CheckOutOfBounds(i))
                    continue;

                for (int j = kolomZet - 1; j <= kolomZet + 1; j++)
                {
                    if (!CheckOutOfBounds(j))
                        continue;

                    // Check if surrounding tiles are not the same color or no fiche.
                    if (bord[i, j] == (int)Kleur.Geen ||
                        bord[i, j] == (int)spel.AandeBeurt)
                        continue;

                    if (bord[i, j] == (int)GetOpponentColor(spel))
                    {
                        // Return true if the placed fiche has the kleur value of geen.
                        if (!CheckMovePossible(ref spel, rijZet, kolomZet, i, j))
                        {
                            continue;
                        }
                        possibilitiesCount++;
                        /* if (!CheckMovePossible(ref spel, rijZet, kolomZet, i, j))
                         {
                             continue;
                         }*/
                        /*if (i == -1 && j == -1 && _ficheList.Count != 0)
                            possibilitiesCount++;*/
                        /*if (CheckMovePossible(spel, rijZet, kolomZet, i, j))
                            return bord[rijZet, kolomZet] == (int)Kleur.Geen;*/
                    }
                }
            }

            spel.Bord = bord.MapIntArrToBase64String();

            return (possibilitiesCount > 0);
        }

        /*public bool ZetMogelijk(Spel spel, int rijZet, int kolomZet)
        {
            var bord = spel.Bord.MapStringBordTo2DIntArr();
            var bordToCheck = (int[,])spel.Bord.MapStringBordTo2DIntArr().Clone();

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
                                        return bordToCheck[rijZet, kolomZet] == (int)Kleur.Geen;
                                    }
                            }
                        }
                    }
                }
            }

            return false;
        }*/

        public bool CheckMovePossible(ref Spel spel, int rijZet, int kolomZet, int y, int x)
        {
            HashSet<CoordsModel> tempHashSet = new HashSet<CoordsModel>();
            var _diffY = y - rijZet;
            var _diffX = x - kolomZet;

            var currY = y;
            var currX = x;

            var bord = spel.Bord.MapStringBordTo2DIntArr();

            for (int i = 0; i < _BORD_SIZE; i++)
            {
                if (CheckOutOfBounds(currY) && CheckOutOfBounds(currX))
                {
                    if (bord[currY, currX] == (int)Kleur.Geen)
                    {
                        return false;
                    }

                    if (bord[currY, currX] == spel.AandeBeurt)
                    {
                        /*spel.Bord = bord.MapIntArrToBase64String();*/
                        tempHashSet.Add(new CoordsModel { Y = currY, X = currX });

                        foreach (var item in tempHashSet)
                        {
                            _ficheList.Add(item);
                        }
                        _ficheList.Add(new CoordsModel { Y = rijZet, X = kolomZet });
                        return true;
                    }

                    tempHashSet.Add(new CoordsModel { Y = currY, X = currX });
                    currY += _diffY;
                    currX += _diffX;
                }
            }
            return false;
        }
        /*public bool CheckMovePossible(Spel spel, int rijZet, int kolomZet, int y, int x)
        {
            var _diffY = y - rijZet;
            var _diffX = x - kolomZet;

            var currY = y;
            var currX = x;

            var bord = spel.Bord.MapStringBordTo2DIntArr();

            var _prevY = 0;
            var _prevX = 0;

            for (int i = 0; i < _BORD_SIZE; i++)
            {
                if (CheckOutOfBounds(currY) && CheckOutOfBounds(currX))
                {
                    if (bord[currY, currX] == (int)Kleur.Geen)
                        return false;

                    if (bord[currY, currX] == spel.AandeBeurt)
                    {
                        _prevY = currY;
                        _prevX = currX;
                        spel.Bord = bord.MapIntArrToBase64String();
                        return true;
                    }

                    currY += _diffY;
                    currX += _diffX;
                }
            }

            return false;
        }*/
        /* public bool DoeZet(ref Spel spel, int rijZet, int kolomZet, out List<CoordsModel> flippedResult)
         {
             flippedResult = null;
             var bord = spel.Bord.MapStringBordTo2DIntArr();


             if (rijZet >= bord.GetLength(0) || kolomZet >= bord.GetLength(1))
                 return false;

             if (ZetMogelijk(spel, rijZet, kolomZet))
             {
                 bord[rijZet, kolomZet] = spel.AandeBeurt;

                 spel.Bord = bord.MapIntArrToBase64String();
                 flippedResult = FlipStonesBetween(ref spel, rijZet, kolomZet);

                 spel.AandeBeurt = (int)GetOpponentColor(spel);
                 *//*spel.Bord = bord.MapIntArrToBase64String();*//*
                 return true;
             }
             else
             {
                 flippedResult = null;
                 spel.Bord = bord.MapIntArrToBase64String();
                 return false;
             }
         }*/

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

        public bool Opgeven(Spel spel)
        {
            var endTime = DateTime.Now;
            spel.FinishedAt = endTime;
            spel.WonBy = GetOpponentGuid(spel);
            spel.LostBy = GetSpelerGuid(spel);
            return false;
        }


        public int[,] FlipStonesBetween(ref Spel spel, HashSet<CoordsModel> ficheList)
        {
            var bord = spel.Bord.MapStringBordTo2DIntArr();

            foreach (var fiche in ficheList)
            {
                bord[fiche.Y, fiche.X] = spel.AandeBeurt;
            }

            return bord;
            // spel.Bord = bord.MapIntArrToBase64String();
        }
        /*public List<CoordsModel> FlipStonesBetween(ref Spel spel, int startY, int startX)
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

            bord[startX, startY] = spel.AandeBeurt;
            cellsToFlip.Add(new CoordsModel { X = startX, Y = startY });
            spel.Bord = bord.MapIntArrToBase64String();
            return cellsToFlip;
        }*/



        public bool CheckOutOfBounds(int bound) => (bound >= 0 && bound < _BORD_SIZE);


        public Kleur GetOpponentColor(Spel spel)
        {
            var result =  spel.AandeBeurt.Equals((int)Kleur.Zwart) ? Kleur.Wit : Kleur.Zwart;
            return result;
        } 

        public Guid? GetOpponentGuid(Spel spel)
        {
            var result = spel.AandeBeurt.Equals(1) ? spel.Speler2Token : spel.Speler1Token;
            return result;
        }

        public Guid? GetSpelerGuid(Spel spel)
        {
            var result = spel.AandeBeurt.Equals(1) ? spel.Speler1Token : spel.Speler2Token;
            return result;
        }

    }
}