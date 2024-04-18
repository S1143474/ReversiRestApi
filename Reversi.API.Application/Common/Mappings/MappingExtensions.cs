using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
/*using Reversi.API.Application.Spellen.Queries.LoadReversi.Contract;
*/using Reversi.API.Domain.Entities;
using Reversi.API.Domain.Enums;

namespace Reversi.API.Application.Common.Mappings
{
    public static class MappingExtensions
    {

        public static string MapIntArrToBase64String(this int[,] intBord)
        {
            var bordInBytes = ToByteArray(intBord);
            var base64Bord = Convert.ToBase64String(bordInBytes);

            return base64Bord;
        }

       /* public static Spel MapToEntity(this DBSpel dbSpel)
        {
            return new Spel
            {
                Id = dbSpel.GUID,
                Omschrijving = dbSpel.Description,
                Token = dbSpel.Token,
                Speler1Token = dbSpel.Speler1Token,
                Speler2Token = dbSpel.Speler2Token,
                Bord = FromIntToKleurArray(dbSpel.Bord.MapStringBordTo2DIntArr()),
                AandeBeurt = (Kleur)dbSpel.Beurt,
                CreatedAt = dbSpel.CreatedAt,
                StartedAt = dbSpel.StartedAt,
                FinishedAt = dbSpel.EndedAt,
            };
        }

        public static SpelDTO MapToDto(this DBSpel dbSpel)
        {
            return new SpelDTO()
            {
                Id = dbSpel.GUID,
                Omschrijving = dbSpel.Description,
                Token = dbSpel.Token,
                Speler1Token = dbSpel.Speler1Token,
                Speler2Token = dbSpel.Speler2Token,
                Bord = dbSpel.Bord.MapStringBordTo2DIntList(),
                AandeBeurt = dbSpel.Beurt,
                CreatedAt = dbSpel.CreatedAt,
                StartedAt = dbSpel.StartedAt,
                EndedAt = dbSpel.EndedAt
            };
        }*/


        /// <summary>
        /// Converts a int[,] bord to a ISpel.Kleur[,] bord
        /// </summary>
        /// <param name="prevBord"></param>
        /// <returns></returns>
        public static Kleur[,] FromIntToKleurArray(int[,] prevBord)
        {
            Kleur[,] result = new Kleur[prevBord.GetLength(0), prevBord.GetLength(1)];

            for (int i = 0; i < prevBord.GetLength(0); i++)
            for (int j = 0; j < prevBord.GetLength(1); j++)
                result[i, j] = ConvertIntToKleur(prevBord[i, j]);

            return result;
        }

        /// <summary>
        /// Converts a ISpel.Kleur[,] to an int[,]
        /// </summary>
        /// <param name="prevBord"></param>
        /// <returns></returns>
        public static int[,] FromKleurToIntArray(Kleur[,] prevBord)
        {
            int[,] result = new int[prevBord.GetLength(0), prevBord.GetLength(1)];

            for (int i = 0; i < prevBord.GetLength(0); i++)
            for (int j = 0; j < prevBord.GetLength(1); j++)
                result[i, j] = (int)prevBord[i, j];

            return result;
        }

        private static Kleur ConvertIntToKleur(int i)
        {
            if (i == 1)
                return Kleur.Wit;
            if (i == 2)
                return Kleur.Zwart;
            return Kleur.Geen;
        }

        /// <summary>
        /// Converts an int[,] to a Byte array
        /// </summary>
        /// <param name="intArray"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(int[,] intArray)
        {
            byte[] nmbsBytes = new byte[intArray.GetLength(0) * intArray.GetLength(1) * 4];
            int b = 0;
            for (int i = 0; i < intArray.GetLength(0); i++)
            {
                for (int j = 0; j < intArray.GetLength(1); j++)
                {
                    byte[] array = BitConverter.GetBytes(intArray[i, j]);
                    for (int m = 0; m < array.Length; m++)
                    {
                        nmbsBytes[b++] = array[m];
                    }
                }
            }
            return nmbsBytes;
        }

        public static List<List<int>> MapStringBordTo2DIntList(this string bord)
        {
            var bordBytes = Convert.FromBase64String(bord);
            var bordIntArr = ToIntArray(bordBytes);

            return bordIntArr.IntBordToList();
        }

        public static int[,] MapStringBordTo2DIntArr(this string bord)
        {
            var bordBytes = Convert.FromBase64String(bord);
            var bordIntArr = ToIntArray(bordBytes);

            return bordIntArr;
        }

        private static List<List<int>> IntBordToList(this int[,] bord)
        {
            var result = new List<List<int>>();

            for (int i = 0; i < bord.GetLength(0); i++)
            {
                var tempList = new List<int>();

                for (int j = 0; j < bord.GetLength(1); j++)
                {
                    tempList.Add(bord[i, j]);
                }

                result.Add(tempList);
            }

            return result;
        }

        /// <summary>
        /// Converts a byte[] to an int[,]
        /// </summary>
        /// <param name="nmbsBytes"></param>
        /// <returns></returns>
        private static int[,] ToIntArray(byte[] nmbsBytes)
        {
            int[,] nmbs = new int[8, 8];
            int k = 0;
            for (int i = 0; i < nmbs.GetLength(0); i++)
            {
                for (int j = 0; j < nmbs.GetLength(1); j++)
                {
                    nmbs[i, j] = BitConverter.ToInt32(nmbsBytes, k);
                    k += 4;
                }
            }
            return nmbs;
        }
    }
}
