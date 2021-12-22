using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ReversiRestApi.ISpel.Kleur;

namespace ReversiRestApi.Json_obj
{
    public class SpelJsonObj
    {
        public int ID { get; set; }
        public string Omschrijving { get; set; }
        public string Token { get; set; }
        public string Speler1Token { get; set; }
        public string Speler2Token { get; set; }
        public List<List<int>> Bord { get; set; }
        public int AandeBeurt { get; set; }

        public SpelJsonObj(Spel spel)
        {
            ID = spel.ID;
            Omschrijving = spel.Omschrijving;
            Token = spel.Token;
            Speler1Token = spel.Speler1Token;
            Speler2Token = spel.Speler2Token;
            Bord = ConvertBordKleurToBordIntArray(spel.Bord);
            AandeBeurt = (int)spel.AandeBeurt;
        }

        /// <summary>
        /// Converts Bord[,] array to an string
        /// </summary>
        /// <param name="bord"></param>
        /// <returns></returns>
        public static List<List<int>> ConvertBordKleurToBordIntArray(ISpel.Kleur[,] prevBord)
        {
            List<List<int>> result = new List<List<int>>();

            for (int i = 0; i < prevBord.GetLength(0); i++)
            {
                result.Add(new List<int>());
                for (int j = 0; j < prevBord.GetLength(1); j++)
                {
                    result[i].Add((int)prevBord[i, j]);
                }
            }

            return result;
        }
    }
}
