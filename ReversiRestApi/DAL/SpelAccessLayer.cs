using ReversiRestApi.Json_obj;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReversiRestApi.DAL
{
    public class SpelAccessLayer : ISpelRepository
    {
        private const string _CONNECTION_STRING = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ReversiDbRestApi;Integrated Security=True;";


        /// <summary>
        /// Adds a new spel to the database
        /// </summary>
        /// <param name="spel"></param>
        public async Task AddSpel(CancellationToken token, Spel spel)
        {
            using (SqlConnection conn = new SqlConnection(_CONNECTION_STRING))
            {

                string query = "INSERT INTO Spel (Description, Token, Speler1Token, Speler2Token, Bord) VALUES(@Description, @Token, @Speler1Token, @Speler2Token, @Bord)";
                SqlCommand command = new SqlCommand(query, conn);

                SqlParameter parameter = new SqlParameter(@"Speler2Token", HandleNull(spel.Speler2Token)) { SqlDbType = System.Data.SqlDbType.VarChar, Scale = 45, IsNullable = true};

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(@"Description", spel.Omschrijving) { SqlDbType = System.Data.SqlDbType.Text},
                    new SqlParameter(@"Token", spel.Token) { SqlDbType = System.Data.SqlDbType.VarChar, Scale = 45},
                    new SqlParameter(@"Speler1Token", HandleNull(spel.Speler1Token)) { SqlDbType = System.Data.SqlDbType.VarChar, Scale = 45},
                    parameter,
                    new SqlParameter(@"Bord", Convert.ToBase64String(ToByteArray(FromKleurToIntArray(spel.Bord)).ToArray())) { SqlDbType = System.Data.SqlDbType.VarChar}
                };

                command.Parameters.AddRange(parameters);

                await conn.OpenAsync(token);

                await command.ExecuteNonQueryAsync(token);
                conn.Close();
            }
        }

        /// <summary>
        /// Retrieves a spel via a spelToken
        /// </summary>
        /// <param name="spelToken"></param>
        /// <returns></returns>
        public async Task<Spel> GetSpel(CancellationToken token, string spelToken)
        {
            Spel result = null;

            using (SqlConnection conn = new SqlConnection(_CONNECTION_STRING))
            {
                string query = "SELECT * FROM Spel WHERE Token = @Token";
                SqlCommand command = new SqlCommand(query, conn);
                await conn.OpenAsync(token);

                command.Parameters.AddWithValue("@Token", spelToken);

                SqlDataReader reader = await command.ExecuteReaderAsync(token);
                
                while(reader.Read())
                {
                    result = new Spel()
                    {
                        ID = Convert.ToInt32(reader["GUID"]),
                        Omschrijving = reader["Description"].ToString(),
                        Token = reader["Token"].ToString(),
                        Speler1Token = reader["Speler1Token"].ToString(),
                        Speler2Token = HandleDbNull(reader["Speler2Token"]),
                        Bord = FromIntToKleurArray(ToIntArray(Convert.FromBase64String(reader["Bord"].ToString()))),
                        AandeBeurt = ConvertIntToKleur(Convert.ToInt32(reader["Beurt"]))
                    };
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="speler1Token"></param>
        /// <returns></returns>
        public async Task<Spel> GetSpelFromSpeler1(CancellationToken token, string speler1Token)
        {
            Spel result = null;

            using (SqlConnection conn = new SqlConnection(_CONNECTION_STRING))
            {
                string query = "SELECT * FROM Spel WHERE Speler1Token = @speler1Token AND EndedAt IS NULL";
                SqlCommand command = new SqlCommand(query, conn);
                await conn.OpenAsync(token);

                command.Parameters.AddWithValue("@speler1Token", speler1Token);

                SqlDataReader reader = await command.ExecuteReaderAsync(token);

                while (reader.Read())
                {
                    result = new Spel()
                        {
                            ID = Convert.ToInt32(reader["GUID"]),
                            Omschrijving = reader["Description"].ToString(),
                            Token = reader["Token"].ToString(),
                            Speler1Token = reader["Speler1Token"].ToString(),
                            Speler2Token = HandleDbNull(reader["Speler2Token"]),
                            Bord = FromIntToKleurArray(ToIntArray(Convert.FromBase64String(reader["Bord"].ToString()))),
                            AandeBeurt = ConvertIntToKleur(Convert.ToInt32(reader["Beurt"]))
                        };
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="speler2Token"></param>
        /// <returns></returns>
        public async Task<Spel> GetSpelFromSpeler2(CancellationToken token, string speler2Token)
        {
            Spel result = null;

            using (SqlConnection conn = new SqlConnection(_CONNECTION_STRING))
            {
                string query = "SELECT * FROM Spel WHERE Speler2Token = @speler2Token AND EndedAt IS NULL";
                SqlCommand command = new SqlCommand(query, conn);
                await conn.OpenAsync(token);

                command.Parameters.AddWithValue("@speler2Token", speler2Token);

                SqlDataReader reader = await command.ExecuteReaderAsync(token);

                while (reader.Read())
                {
                    result = new Spel()
                    {
                        ID = Convert.ToInt32(reader["GUID"]),
                        Omschrijving = reader["Description"].ToString(),
                        Token = reader["Token"].ToString(),
                        Speler1Token = reader["Speler1Token"].ToString(),
                        Speler2Token = HandleDbNull(reader["Speler2Token"]),
                        Bord = FromIntToKleurArray(ToIntArray(Convert.FromBase64String(reader["Bord"].ToString()))),
                        AandeBeurt = ConvertIntToKleur(Convert.ToInt32(reader["Beurt"]))
                    };
                }
            }

            return result;
        }

        /// <summary>
        /// Updates a turn or pass etc to the database.
        /// </summary>
        /// <param name="spelToken"></param>
        public async Task<bool> UpdateSpel(CancellationToken token, Spel spel)
        {
            using (SqlConnection conn = new SqlConnection(_CONNECTION_STRING))
            {
                string query = "UPDATE Spel SET Bord = @Bord, Beurt = @Beurt WHERE Token = @Token";
                try
                {
                    SqlCommand command = new SqlCommand(query, conn);
                    await conn.OpenAsync(token);

                    command.Parameters.AddWithValue("@Token", spel.Token);
                    command.Parameters.AddWithValue("@Beurt", spel.AandeBeurt);
                    command.Parameters.AddWithValue("@Bord", Convert.ToBase64String(ToByteArray(FromKleurToIntArray(spel.Bord)).ToArray()));

                    await command.ExecuteNonQueryAsync(token);
                    return true;
                } catch (Exception e) { return false; }
            }
        }

        /// <summary>
        /// Retrieves all the spellen with a speler2token of NULL
        /// </summary>
        /// <returns></returns>
        public async Task<List<Spel>> GetSpellenAsync(CancellationToken token)
        {
            List<Spel> result = new List<Spel>();

            using (SqlConnection conn = new SqlConnection(_CONNECTION_STRING))
            {
                /*string query = "SELECT * FROM Spel WHERE Speler2Token IS NULL";*/
                string query = "SELECT * FROM Spel";
                SqlCommand command = new SqlCommand(query, conn);
                await conn.OpenAsync(token);

                SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken:token);

                while (reader.Read())
                {
                    Spel spel = new Spel()
                    {
                        ID = Convert.ToInt32(reader["GUID"]),
                        Omschrijving = reader["Description"].ToString(),
                        Token = reader["Token"].ToString(),
                        Speler1Token = reader["Speler1Token"].ToString(),
                        Speler2Token = HandleDbNull(reader["Speler2Token"]),
                        Bord = FromIntToKleurArray(ToIntArray(Convert.FromBase64String(reader["Bord"].ToString()))),
                        AandeBeurt = ConvertIntToKleur(Convert.ToInt32(reader["Beurt"]))
                    };

                    result.Add(spel);
                }
            }

            return result;
        }

        /// <summary>
        /// Method for joining a game as second player.
        ///   </summary>
        /// <param name="joinGameObj"></param>
        /// <returns></returns>
        public async Task<bool> JoinSpel(CancellationToken token, JoinGameObj joinGameObj)
        {
            if (joinGameObj != null && !string.IsNullOrWhiteSpace(joinGameObj.SpelToken) && !string.IsNullOrWhiteSpace(joinGameObj.Speler2Token))
            {
                using (SqlConnection conn = new SqlConnection(_CONNECTION_STRING))
                {
                    string query = "UPDATE Spel SET Speler2Token = @Speler2Token, StartedAt = @StartedAt WHERE Token = @SpelToken";
                    
                    try
                    {
                        SqlCommand command = new SqlCommand(query, conn);
                        await conn.OpenAsync(token);

                        command.Parameters.AddWithValue("@Speler2Token", joinGameObj.Speler2Token);
                        command.Parameters.AddWithValue("@StartedAt", DateTime.Now.ToUniversalTime());
                        command.Parameters.AddWithValue("@SpelToken", joinGameObj.SpelToken);

                        await command.ExecuteNonQueryAsync(token);
                        return true;
                    }
                    catch (Exception e) { return false; }
                }
            }
            return false;
        }

        /// <summary>
        /// Converts object from null to DBNull.Value
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private object HandleNull(object obj)
        {
            return (obj != null) ? obj : DBNull.Value;
        }

        /// <summary>
        /// Converts object from DBNull.Value to null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string HandleDbNull(object obj)
        {
            return (obj != DBNull.Value) ? obj.ToString() : null;
        }

        /// <summary>
        /// Converts a int[,] bord to a ISpel.Kleur[,] bord
        /// </summary>
        /// <param name="prevBord"></param>
        /// <returns></returns>
        private ISpel.Kleur[,] FromIntToKleurArray(int[,] prevBord)
        {
            ISpel.Kleur[,] result = new ISpel.Kleur[prevBord.GetLength(0), prevBord.GetLength(1)];

            for (int i = 0; i < prevBord.GetLength(0); i++)
                for (int j = 0; j < prevBord.GetLength(1); j++)
                    result[i, j] = ConvertIntToKleur(prevBord[i, j]);

            return result;
        }

        /// <summary>
        /// Converts an int[,] to a Byte array
        /// </summary>
        /// <param name="intArray"></param>
        /// <returns></returns>
        private byte[] ToByteArray(int[,] intArray)
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

        /// <summary>
        /// Converts a ISpel.Kleur[,] to an int[,]
        /// </summary>
        /// <param name="prevBord"></param>
        /// <returns></returns>
        private int[,] FromKleurToIntArray(ISpel.Kleur[,] prevBord)
        {
            int[,] result = new int[prevBord.GetLength(0), prevBord.GetLength(1)];

            for (int i = 0; i < prevBord.GetLength(0); i++)
                for (int j = 0; j < prevBord.GetLength(1); j++)
                    result[i, j] = (int)prevBord[i, j];

            return result;
        }

        /// <summary>
        /// Converts a byte[] to an int[,]
        /// </summary>
        /// <param name="nmbsBytes"></param>
        /// <returns></returns>
        private int[,] ToIntArray(byte[] nmbsBytes)
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

        /// <summary>
        /// Converts an int to a ISpel.Kleur
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private ISpel.Kleur ConvertIntToKleur(int i)
        {
            if (i == 1)
                return ISpel.Kleur.Wit;
            else if (i == 2)
                return ISpel.Kleur.Zwart;
            else
                return ISpel.Kleur.Geen;
        }
    }
}
