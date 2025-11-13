using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Pensafy_Final_Integrated
{
    public static class Database
    {
        private const string ConnectionString = "Data Source=PENSAFY.db;Version=3;";

        private static SQLiteConnection GetConnection()
        {
            var conn = new SQLiteConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        // Usuario
        public static bool UsuarioExiste(string email)
        {
            using (var conn = GetConnection())
            using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM Usuario_ WHERE Email = @e", conn))
            {
                cmd.Parameters.AddWithValue("@e", email);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public static bool CadastrarUsuario(string nome, string email, string senha)
        {
            using (var conn = GetConnection())
            using (var cmd = new SQLiteCommand(
                "INSERT INTO Usuario_(Nome_Completo, Email, Senha_, Pontuacao_Total) VALUES (@n,@e,@s,0)",
                conn))
            {
                cmd.Parameters.AddWithValue("@n", nome);
                cmd.Parameters.AddWithValue("@e", email);
                cmd.Parameters.AddWithValue("@s", senha);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static int Login(string email, string senha)
        {
            using (var conn = GetConnection())
            using (var cmd = new SQLiteCommand("SELECT Id_Usuario FROM Usuario_ WHERE Email=@e AND Senha_=@s", conn))
            {
                cmd.Parameters.AddWithValue("@e", email);
                cmd.Parameters.AddWithValue("@s", senha);
                var res = cmd.ExecuteScalar();
                if (res == null) return -1;
                return Convert.ToInt32(res);
            }
        }

        public static int ObterPontuacaoTotal(int idUsuario)
        {
            using (var conn = GetConnection())
            using (var cmd = new SQLiteCommand("SELECT Pontuacao_Total FROM Usuario_ WHERE Id_Usuario=@u", conn))
            {
                cmd.Parameters.AddWithValue("@u", idUsuario);
                var res = cmd.ExecuteScalar();
                if (res==null) return 0;
                return Convert.ToInt32(res);
            }
        }

        public static void AtualizarPontuacao(int idUsuario, int pontos)
        {
            using (var conn = GetConnection())
            using (var cmd = new SQLiteCommand("UPDATE Usuario_ SET Pontuacao_Total = Pontuacao_Total + @p WHERE Id_Usuario=@u", conn))
            {
                cmd.Parameters.AddWithValue("@p", pontos);
                cmd.Parameters.AddWithValue("@u", idUsuario);
                cmd.ExecuteNonQuery();
            }
        }

        // Palavras
        public static List<(int Id, string Texto, string Dificuldade)> ObterPalavras()
        {
            var lista = new List<(int, string, string)>();
            using (var conn = GetConnection())
            using (var cmd = new SQLiteCommand("SELECT Id_Palavra, Texto_Palavra, Dificuldade FROM Palavra", conn))
            using (var rd = cmd.ExecuteReader())
            {
                while (rd.Read())
                {
                    lista.Add((rd.GetInt32(0), rd.GetString(1), rd.GetString(2)));
                }
            }
            return lista;
        }

        public static (int Id, string Texto, string Dificuldade) ObterPalavraPorDificuldade(string dificuldade)
        {
            using (var conn = GetConnection())
            using (var cmd = new SQLiteCommand("SELECT Id_Palavra, Texto_Palavra, Dificuldade FROM Palavra WHERE Dificuldade = @d ORDER BY RANDOM() LIMIT 1", conn))
            {
                cmd.Parameters.AddWithValue("@d", dificuldade);
                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        return (rd.GetInt32(0), rd.GetString(1), rd.GetString(2));
                    }
                }
            }
            return (0, "", "");
        }

        // Partida
        public static int CriarPartida(int idUsuario, int idPalavra, string palavraSecreta)
        {
            using (var conn = GetConnection())
            using (var cmd = new SQLiteCommand("INSERT INTO Partida (Id_Usuario, Palavra_Secreta, Tentativas_Usadas, Acertou, Pontuacao_Ganha, Data_Partida, fk_Palavra_Id_Palavra) VALUES (@u,@ps,0,0,0,DATE('now'),@pid); SELECT last_insert_rowid();", conn))
            {
                cmd.Parameters.AddWithValue("@u", idUsuario);
                cmd.Parameters.AddWithValue("@ps", palavraSecreta);
                cmd.Parameters.AddWithValue("@pid", idPalavra);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // Tentativa
        public static void RegistrarTentativa(int idPartida, string texto, string resultado, int posicao)
        {
            using (var conn = GetConnection())
            using (var cmd = new SQLiteCommand("INSERT INTO Tentativa (Id_Partida, Tentativa_Texto, Resultado, Posicao) VALUES (@p,@t,@r,@pos)", conn))
            {
                cmd.Parameters.AddWithValue("@p", idPartida);
                cmd.Parameters.AddWithValue("@t", texto);
                cmd.Parameters.AddWithValue("@r", resultado);
                cmd.Parameters.AddWithValue("@pos", posicao);
                cmd.ExecuteNonQuery();
            }
        }

        // Ranking
        public static List<(string Nome, int Pontos)> ObterRanking()
        {
            var lista = new List<(string, int)>();
            using (var conn = GetConnection())
            using (var cmd = new SQLiteCommand("SELECT Nome_Completo, Pontuacao_Total FROM Usuario_ ORDER BY Pontuacao_Total DESC", conn))
            using (var rd = cmd.ExecuteReader())
            {
                while (rd.Read())
                {
                    lista.Add((rd.GetString(0), rd.GetInt32(1)));
                }
            }
            return lista;
        }

        // Cupom
        public static void GerarCupom(int idUsuario, string codigo, string tipo)
        {
            using (var conn = GetConnection())
            using (var cmd = new SQLiteCommand("INSERT INTO Cupom (Id_Usuario, Data_Geracao, Codigo_Cupom, Status_Cupom, Tipo_Cupom) VALUES (@u,DATE('now'),@c,'ativo',@t)", conn))
            {
                cmd.Parameters.AddWithValue("@u", idUsuario);
                cmd.Parameters.AddWithValue("@c", codigo);
                cmd.Parameters.AddWithValue("@t", tipo);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
