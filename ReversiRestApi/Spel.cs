using ReversiRestApi.Json_obj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ReversiRestApi.ISpel.Kleur;

namespace ReversiRestApi
{
    public class Spel : ISpel
    {
        public int ID { get; set; }
        public string Omschrijving { get; set; }
        public string Token { get; set; }
        public string Speler1Token { get; set; }
        public string Speler2Token { get; set; }
        public ISpel.Kleur[,] Bord { get; set; }
        public ISpel.Kleur AandeBeurt { get; set; }

        private const int _BORD_SIZE = 8;

        private int _prevY = 0;
        private int _prevX = 0;

        private int _diffY = 0;
        private int _diffX = 0;

        public List<CoordsJsonObj> CellsToFlip { get; set; }

        public Spel()
        {
            AandeBeurt = Zwart;
            Bord = new ISpel.Kleur[_BORD_SIZE, _BORD_SIZE]
            {
                {Geen, Geen, Geen, Geen, Geen, Geen, Geen, Geen },
                {Geen, Geen, Geen, Geen, Geen, Geen, Geen, Geen },
                {Geen, Geen, Geen, Geen, Geen, Geen, Geen, Geen },
                {Geen, Geen, Geen, Wit, Zwart, Geen, Geen, Geen },
                {Geen, Geen, Geen, Zwart, Wit, Geen, Geen, Geen },
                {Geen, Geen, Geen, Geen, Geen, Geen, Geen, Geen },
                {Geen, Geen, Geen, Geen, Geen, Geen, Geen, Geen },
                {Geen, Geen, Geen, Geen, Geen, Geen, Geen, Geen },
            }; // Y, X
        }

        public bool Afgelopen()
        {
            for (int i = 0; i < Bord.GetLength(0); i++)
            {
                for (int j = 0; j < Bord.GetLength(1); j++)
                {
                    if (Bord[i, j] == Geen)
                    {
                        if (ZetMogelijk(i, j))
                            return false;
                    }
                }
            }
            return true;
        }

        public bool DoeZet(int rijZet, int kolomZet)
        {

            if (ZetMogelijk(rijZet, kolomZet))
            {
                Bord[rijZet, kolomZet] = AandeBeurt;
               
                    FlipStonesBetween(rijZet, kolomZet);

               
                AandeBeurt = GetOpponentColor();
                return true;
            } else 
                return false; 
        }

        public ISpel.Kleur OverwegendeKleur()
        {
            int zwartCount = 0, witCount = 0;

            foreach (ISpel.Kleur color in Bord)
            {
                if (color == Wit) witCount++;
                if (color == Zwart) zwartCount++;
            }

            return (zwartCount == witCount) ? Geen : (zwartCount > witCount) ? Zwart : Wit;
        }

        public int[] GedraaideFiches()
        {
            var result = new int[3];

            foreach (var color in Bord)
                result[(int)color]++;

            return result;
        }

        public bool Pas()
        {
            AandeBeurt = GetOpponentColor();
            return true;
        }
        
        public bool ZetMogelijk(int rijZet, int kolomZet)
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
                                if (Bord[i, j] == Geen)
                                    continue;
                                else if (Bord[i, j] == AandeBeurt)
                                    continue;
                                else if (Bord[i, j] == GetOpponentColor())
                                    if (CheckMovePossible(rijZet, kolomZet, i, j))
                                        return Bord[rijZet, kolomZet] == Geen;
                            }
                        }
                    }
                }
            }

            return false;           
        }

        /// <summary>
        /// Flips stones between given x and y and _prevX and _prevY
        /// </summary>
        /// <param name="startY"></param>
        /// <param name="startX"></param>
        private void FlipStonesBetween(int startY, int startX)
        {
            /*int currY = startY;
            int currX = startX;

            int diffY = (startY - _prevY) < 0 ? (startY - _prevY) * -1 : (startY - _prevY);
            int diffX = (startX - _prevX) < 0 ? (startX - _prevX) * -1 : (startX - _prevX);

            for (int i = 0; i < ((diffX >= diffY) ? diffX : diffY); i++) {

                if (Bord[currY, currX] == GetOpponentColor())
                {
                    Bord[currY, currX] = AandeBeurt;
                    CellsToFlip = new List<CoordsJsonObj>();
                    CellsToFlip.Add(new CoordsJsonObj() { X = currX, Y = currY });
                }

                currY += _diffY;
                currX += _diffX;
            }*/
            CellsToFlip = new List<CoordsJsonObj>();
            
            for (int i = startY - 1; i <= startY + 1; i++)
            {
                for (int j = startX - 1; j <= startX + 1; j++)
                {
                    if (!CheckOutOfBounds(i) || !CheckOutOfBounds(j)) continue;

                    if (!CheckMovePossible(startY, startX, i, j))
                        continue;
                    int currY = startY;
                    int currX = startX;
                    int stepYDir = i - currY;
                    int stepXDir = j - currX;

                    currY += stepYDir;
                    currX += stepXDir;

                    ISpel.Kleur tempKLeur = GetOpponentColor();

                    while (Bord[currY, currX] != AandeBeurt)
                    {
                        Bord[currY, currX] = AandeBeurt;
                        CellsToFlip.Add(new CoordsJsonObj() { X = currX, Y = currY });

                        if (CheckOutOfBounds(currY + stepYDir) && CheckOutOfBounds(currX + stepXDir))
                        {
                            currY += stepYDir;
                            currX += stepXDir;
                        }
                    }
                }
            }
            CellsToFlip.Add(new CoordsJsonObj() { X = startX, Y = startY });
        }
        

        /// <summary>
        /// Check move direction is possible
        /// </summary>
        /// <param name="rijZet"></param>
        /// <param name="kolomZet"></param>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        private bool CheckMovePossible(int rijZet, int kolomZet, int y, int x)
        {
            _diffY = y - rijZet;
            _diffX = x - kolomZet;

            int currY = y;
            int currX = x;

            for (int i = 0; i < _BORD_SIZE; i++)
            {
                if (CheckOutOfBounds(currY) && CheckOutOfBounds(currX))
                {
                    if (Bord[currY, currX] == Geen)
                        return false;

                    if (Bord[currY, currX] == AandeBeurt)
                    {
                        _prevY = currY;
                        _prevX = currX;
                        return true;
                    }

                    currY += _diffY;
                    currX += _diffX;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns if bound is inside the bord
        /// </summary>
        /// <param name="bound"></param>
        /// <returns></returns>
        private bool CheckOutOfBounds(int bound) => (bound >= 0 && bound < _BORD_SIZE);

        /// <summary>
        /// Gets the opponents color;
        /// </summary>
        /// <returns></returns>
        private ISpel.Kleur GetOpponentColor() => AandeBeurt.Equals(Zwart) ? Wit : Zwart;  
    }
}
