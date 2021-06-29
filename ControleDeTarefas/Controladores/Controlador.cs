using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ControleDeTarefas.Dominio;

namespace ControleDeTarefas.Controladores
{
    public class Controlador<T> where T : EntidadeBase
    {
        public List<T> Registros { get => VisualizarRegistros(); }
        private const string sqlSelecionarContatoPorId =
            @"SELECT 
                [ID],
                [NOME],
                [EMAIL],
                [TELEFONE],
                [EMPRESA],
                [CARGO]
             FROM
                [TBCONTATOS]
             WHERE 
                [ID] = @ID";

        public Controlador()
        {
        }

        #region CRUD Methods
        public string InserirNovoRegistro(T registro)
        {
            string resultadoValidacao = registro.Validar();
        
            InicializarBanco(out SqlConnection conexaoComBanco, out SqlCommand comandoInsercao);

            if (SelecionarTabela() == "TBTarefas")
            {
                Tarefa tarefa = registro as Tarefa;
                string sqlInsercao =
                        @"INSERT INTO TBTAREFAS 
        	                    (
                                    [TITULO], 
                                    [DATA_INICIAL],
                                    [DATA_CONCLUSAO],
                                    [PORCENTAGEM],
                                    [PRIORIDADE]
        	                    ) 
        	                    VALUES
        	                    (
                                    @TITULO, 
        		                    @DATA_INICIAL,
                                    @DATA_CONCLUSAO,
                                    @PORCENTAGEM,
                                    @PRIORIDADE
        	                    );";

                sqlInsercao +=
                    @"SELECT SCOPE_IDENTITY();";

                comandoInsercao.CommandText = sqlInsercao;

                comandoInsercao.Parameters.AddWithValue("TITULO", tarefa.Titulo);
                comandoInsercao.Parameters.AddWithValue("DATA_INICIAL", tarefa.DataInicial);
                comandoInsercao.Parameters.AddWithValue("DATA_CONCLUSAO", tarefa.DataConclusao);
                comandoInsercao.Parameters.AddWithValue("PORCENTAGEM", tarefa.Porcentagem);
                comandoInsercao.Parameters.AddWithValue("PRIORIDADE", tarefa.Prioridade);
            }
            else if (SelecionarTabela() == "TBContatos")
            {
                Contato contato = registro as Contato;
                string sqlInsercao =
                @"INSERT INTO TBCONTATOS  
        	                (	
        		                [NOME],
                                [EMAIL],
                                [TELEFONE],
                                [EMPRESA],
                                [CARGO]
                            )
        	                VALUES
        	                (
                                @NOME, 
        		                @EMAIL,
                                @TELEFONE,
                                @EMPRESA,
                                @CARGO
        	                );";

                sqlInsercao +=
                    @"SELECT SCOPE_IDENTITY();";

                comandoInsercao.CommandText = sqlInsercao;

                comandoInsercao.Parameters.AddWithValue("NOME", contato.Nome);
                comandoInsercao.Parameters.AddWithValue("EMAIL", contato.Email);
                comandoInsercao.Parameters.AddWithValue("TELEFONE", contato.Telefone);
                comandoInsercao.Parameters.AddWithValue("EMPRESA", contato.Empresa);
                comandoInsercao.Parameters.AddWithValue("CARGO", contato.Cargo);
            }
            else if (SelecionarTabela() == "TBCompromissos")
            {
                Compromisso compromisso = registro as Compromisso;
                string sqlInsercao =
                @"INSERT INTO TBCOMPROMISSOS  
        	                (	
        		                [ASSUNTO],
                                [LOCAL],
                                [DATAINICIO],
                                [DATAFIM],
                                [CONTATO]
                            )
        	                VALUES
        	                (
                                @ASSUNTO, 
        		                @LOCAL,
                                @DATAINICIO,
                                @DATAFIM,
                                @CONTATO
        	                );";

                sqlInsercao +=
                    @"SELECT SCOPE_IDENTITY();";

                comandoInsercao.CommandText = sqlInsercao;

                comandoInsercao.Parameters.AddWithValue("ASSUNTO", compromisso.Assunto);
                comandoInsercao.Parameters.AddWithValue("LOCAL", compromisso.Local);
                comandoInsercao.Parameters.AddWithValue("DATAINICIO", compromisso.DataInicio);
                comandoInsercao.Parameters.AddWithValue("DATAFIM", compromisso.DataFim);
                if (compromisso.Contato != null) comandoInsercao.Parameters.AddWithValue("CONTATO", compromisso.Contato.ID);
                else comandoInsercao.Parameters.AddWithValue("CONTATO", DBNull.Value);
            }

            object id = comandoInsercao.ExecuteScalar();

            registro.ID = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public List<T> VisualizarRegistros()
        {
            InicializarBanco(out SqlConnection conexaoComBanco, out SqlCommand comandoSelecao);

            string query = "";
            if (SelecionarTabela() == "TBTarefas") query = $"SELECT * FROM TBTAREFAS ORDER BY [PRIORIDADE] DESC";
            else if (SelecionarTabela() == "TBContatos") query = $"SELECT * FROM TBCONTATOS ORDER BY [CARGO] ASC";
            else if (SelecionarTabela() == "TBCompromissos") 
                query = $@"SELECT TBCOMPROMISSOS.[ID], TBCOMPROMISSOS.[ASSUNTO], TBCOMPROMISSOS.[LOCAL], 
                        TBCOMPROMISSOS.[DATAINICIO], TBCOMPROMISSOS.[DATAFIM], TBCONTATOS.[NOME] FROM TBCOMPROMISSOS 
                        LEFT JOIN TBCONTATOS ON TBCOMPROMISSOS.[CONTATO] = TBCONTATOS.[ID]
                        ORDER BY [DATAINICIO] ASC";

            comandoSelecao.CommandText = query;
            SqlDataReader leitorRegistros = comandoSelecao.ExecuteReader();

            List<T> registros = new List<T>();

            while (leitorRegistros.Read())
            {
                List<object> parametros = ObterParametros(leitorRegistros);
                var id = parametros.First();
                parametros.Remove(id);

                T registro = (T)Activator.CreateInstance(typeof(T), parametros.ToArray());
                registro.ID = Convert.ToInt32(id);
                registros.Add(registro);
            }

            conexaoComBanco.Close();
            return registros;
        }

        public List<T> VisualizarCompromissosPassados()
        {
            InicializarBanco(out SqlConnection conexaoComBanco, out SqlCommand comandoSelecao);

            string query = $@"SELECT TBCOMPROMISSOS.[ID], TBCOMPROMISSOS.[ASSUNTO], TBCOMPROMISSOS.[LOCAL], 
                        TBCOMPROMISSOS.[DATAINICIO], TBCOMPROMISSOS.[DATAFIM], TBCONTATOS.[NOME] FROM TBCOMPROMISSOS 
                        LEFT JOIN TBCONTATOS ON TBCOMPROMISSOS.[CONTATO] = TBCONTATOS.[ID]
                        WHERE TBCOMPROMISSOS.[DATAFIM] < GETDATE() 
                        ORDER BY [DATAINICIO] ASC";

            comandoSelecao.CommandText = query;
            SqlDataReader leitorRegistros = comandoSelecao.ExecuteReader();

            List<T> registros = new List<T>();

            while (leitorRegistros.Read())
            {
                List<object> parametros = ObterParametros(leitorRegistros);
                var id = parametros.First();
                parametros.Remove(id);

                T registro = (T)Activator.CreateInstance(typeof(T), parametros.ToArray());
                registro.ID = Convert.ToInt32(id);
                registros.Add(registro);
            }

            conexaoComBanco.Close();
            return registros;
        }

        public List<T> VisualizarCompromissosFuturos()
        {
            InicializarBanco(out SqlConnection conexaoComBanco, out SqlCommand comandoSelecao);

            string query = $@"SELECT TBCOMPROMISSOS.[ID], TBCOMPROMISSOS.[ASSUNTO], TBCOMPROMISSOS.[LOCAL], 
                        TBCOMPROMISSOS.[DATAINICIO], TBCOMPROMISSOS.[DATAFIM], TBCONTATOS.[NOME] FROM TBCOMPROMISSOS 
                        LEFT JOIN TBCONTATOS ON TBCOMPROMISSOS.[CONTATO] = TBCONTATOS.[ID]
                        WHERE TBCOMPROMISSOS.[DATAFIM] > GETDATE() 
                        ORDER BY [DATAINICIO] ASC";

            comandoSelecao.CommandText = query;
            SqlDataReader leitorRegistros = comandoSelecao.ExecuteReader();

            List<T> registros = new List<T>();

            while (leitorRegistros.Read())
            {
                List<object> parametros = ObterParametros(leitorRegistros);
                var id = parametros.First();
                parametros.Remove(id);

                T registro = (T)Activator.CreateInstance(typeof(T), parametros.ToArray());
                registro.ID = Convert.ToInt32(id);
                registros.Add(registro);
            }

            conexaoComBanco.Close();
            return registros;
        }

        public List<T> VisualizarCompromissosFuturosDoDia()
        {
            InicializarBanco(out SqlConnection conexaoComBanco, out SqlCommand comandoSelecao);

            string query = $@"SELECT TBCOMPROMISSOS.[ID], TBCOMPROMISSOS.[ASSUNTO], TBCOMPROMISSOS.[LOCAL], 
                        TBCOMPROMISSOS.[DATAINICIO], TBCOMPROMISSOS.[DATAFIM], TBCONTATOS.[NOME] FROM TBCOMPROMISSOS 
                        LEFT JOIN TBCONTATOS ON TBCOMPROMISSOS.[CONTATO] = TBCONTATOS.[ID]
                        WHERE DAY(TBCOMPROMISSOS.[DATAFIM]) = DAY(GETDATE()) AND TBCOMPROMISSOS.[DATAFIM] > GETDATE() 
                        ORDER BY [DATAINICIO] ASC";

            comandoSelecao.CommandText = query;
            SqlDataReader leitorRegistros = comandoSelecao.ExecuteReader();

            List<T> registros = new List<T>();

            while (leitorRegistros.Read())
            {
                List<object> parametros = ObterParametros(leitorRegistros);
                var id = parametros.First();
                parametros.Remove(id);

                T registro = (T)Activator.CreateInstance(typeof(T), parametros.ToArray());
                registro.ID = Convert.ToInt32(id);
                registros.Add(registro);
            }

            conexaoComBanco.Close();
            return registros;
        }

        public List<T> VisualizarCompromissosFuturosDoMes()
        {
            InicializarBanco(out SqlConnection conexaoComBanco, out SqlCommand comandoSelecao);

            string query = $@"SELECT TBCOMPROMISSOS.[ID], TBCOMPROMISSOS.[ASSUNTO], TBCOMPROMISSOS.[LOCAL], 
                        TBCOMPROMISSOS.[DATAINICIO], TBCOMPROMISSOS.[DATAFIM], TBCONTATOS.[NOME] FROM TBCOMPROMISSOS 
                        LEFT JOIN TBCONTATOS ON TBCOMPROMISSOS.[CONTATO] = TBCONTATOS.[ID]
                        WHERE MONTH(TBCOMPROMISSOS.[DATAFIM]) = MONTH(GETDATE()) AND TBCOMPROMISSOS.[DATAFIM] > GETDATE()  
                        ORDER BY [DATAINICIO] ASC";

            comandoSelecao.CommandText = query;
            SqlDataReader leitorRegistros = comandoSelecao.ExecuteReader();

            List<T> registros = new List<T>();

            while (leitorRegistros.Read())
            {
                List<object> parametros = ObterParametros(leitorRegistros);
                var id = parametros.First();
                parametros.Remove(id);

                T registro = (T)Activator.CreateInstance(typeof(T), parametros.ToArray());
                registro.ID = Convert.ToInt32(id);
                registros.Add(registro);
            }

            conexaoComBanco.Close();
            return registros;
        }

        public void EditarRegistro(int id, T registro)
        {
            InicializarBanco(out SqlConnection conexaoComBanco, out SqlCommand comandoAtualizacao);

            if (SelecionarTabela() == "TBTarefas")
            {
                Tarefa tarefa = registro as Tarefa;
                string sqlAtualizacao =
                @"UPDATE TBTAREFAS 
        	                SET	
        		                [TITULO] = @TITULO, 
                                [DATA_CONCLUSAO] = @DATA_CONCLUSAO,
                                [PORCENTAGEM] = @PORCENTAGEM,
                                [PRIORIDADE] = @PRIORIDADE
        	                WHERE 
        		                [ID] = @ID";

                comandoAtualizacao.CommandText = sqlAtualizacao;

                comandoAtualizacao.Parameters.AddWithValue("ID", id);
                comandoAtualizacao.Parameters.AddWithValue("TITULO", tarefa.Titulo);
                comandoAtualizacao.Parameters.AddWithValue("DATA_CONCLUSAO", tarefa.DataConclusao);
                comandoAtualizacao.Parameters.AddWithValue("PORCENTAGEM", tarefa.Porcentagem);
                comandoAtualizacao.Parameters.AddWithValue("PRIORIDADE", tarefa.Prioridade);
            }
            else if (SelecionarTabela() == "TBContatos")
            {
                Contato contato = registro as Contato;
                string sqlAtualizacao =
                @"UPDATE TBCONTATOS 
        	                SET	
        		                [NOME] = @NOME, 
                                [EMAIL] = @EMAIL,
                                [TELEFONE] = @TELEFONE,
                                [EMPRESA] = @EMPRESA,
                                [CARGO] = @CARGO
        	                WHERE 
        		                [ID] = @ID";

                comandoAtualizacao.CommandText = sqlAtualizacao;

                comandoAtualizacao.Parameters.AddWithValue("ID", id);
                comandoAtualizacao.Parameters.AddWithValue("NOME", contato.Nome);
                comandoAtualizacao.Parameters.AddWithValue("EMAIL", contato.Email);
                comandoAtualizacao.Parameters.AddWithValue("TELEFONE", contato.Telefone);
                comandoAtualizacao.Parameters.AddWithValue("EMPRESA", contato.Empresa);
                comandoAtualizacao.Parameters.AddWithValue("CARGO", contato.Cargo);
            }
            else if (SelecionarTabela() == "TBCompromissos")
            {
                Compromisso compromisso = registro as Compromisso;
                string sqlAtualizacao =
                @"UPDATE TBCOMPROMISSOS 
        	                SET	
        		                [ASSUNTO] = @ASSUNTO, 
                                [LOCAL] = @LOCAL,
                                [DATAINICIO] = @DATAINICIO,
                                [DATAFIM] = @DATAFIM,
                                [CONTATO] = @CONTATO
        	                WHERE 
        		                [ID] = @ID";

                comandoAtualizacao.CommandText = sqlAtualizacao;

                comandoAtualizacao.Parameters.AddWithValue("ID", id);
                comandoAtualizacao.Parameters.AddWithValue("ASSUNTO", compromisso.Assunto);
                comandoAtualizacao.Parameters.AddWithValue("LOCAL", compromisso.Local);
                comandoAtualizacao.Parameters.AddWithValue("DATAINICIO", compromisso.DataInicio);
                comandoAtualizacao.Parameters.AddWithValue("DATAFIM", compromisso.DataFim);
                comandoAtualizacao.Parameters.AddWithValue("CONTATO", compromisso.Contato.ID);
            }

            comandoAtualizacao.ExecuteNonQuery();

            conexaoComBanco.Close();
        }

        public void ExcluirRegistro(int id)
        {
            InicializarBanco(out SqlConnection conexaoComBanco, out SqlCommand comandoExclusao);

            string sqlExclusao = "DELETE FROM " + SelecionarTabela() + " WHERE [ID] = @ID";

            comandoExclusao.CommandText = sqlExclusao;

            comandoExclusao.Parameters.AddWithValue("ID", id);

            comandoExclusao.ExecuteNonQuery();

            conexaoComBanco.Close();
        }
        #endregion


        #region Reset methods
        public static void ResetarTabelaTarefas()
        {
            InicializarBanco(out SqlConnection conexaoComBanco, out SqlCommand comandoReset);

            string sqlReset = "TRUNCATE TABLE TBTarefas";
            comandoReset.CommandText = sqlReset;
            comandoReset.ExecuteNonQuery();

            conexaoComBanco.Close();
        }

        public static void ResetarTabelaContatos()
        {
            InicializarBanco(out SqlConnection conexaoComBanco, out SqlCommand comandoReset);

            string sqlReset = "TRUNCATE TABLE TBContatos";
            comandoReset.CommandText = sqlReset;
            comandoReset.ExecuteNonQuery();

            conexaoComBanco.Close();
        }

        public static void ResetarTabelaCompromissos()
        {
            InicializarBanco(out SqlConnection conexaoComBanco, out SqlCommand comandoReset);

            string sqlReset = "TRUNCATE TABLE TBCompromissos";
            comandoReset.CommandText = sqlReset;
            comandoReset.ExecuteNonQuery();

            conexaoComBanco.Close();
        }
        #endregion

        public Contato SelecionarContatoPorId(int id)
        {
            InicializarBanco(out SqlConnection conexaoComBanco, out SqlCommand comandoSelecao);

            string query = sqlSelecionarContatoPorId;
            comandoSelecao.CommandText = query;
            comandoSelecao.Parameters.AddWithValue("ID", id);
            SqlDataReader leitorRegistros = comandoSelecao.ExecuteReader();

            List<Contato> contatos = new List<Contato>();

            while (leitorRegistros.Read())
            {
                List<object> parametros = ObterParametros(leitorRegistros);
                var idContato = parametros.First();
                parametros.Remove(idContato);

                Contato contato = new Contato(parametros[0].ToString(), parametros[1].ToString(), parametros[2].ToString(), parametros[3].ToString(), parametros[4].ToString())
                {
                    ID = Convert.ToInt32(id)
                };
                contatos.Add(contato);
            }

            conexaoComBanco.Close();
            return contatos[0];
        }

        #region Private methods

        private static void InicializarBanco(out SqlConnection conexaoComBanco, out SqlCommand comando)
        {
            string enderecoDBTarefas =
                @"Data Source=(LocalDb)\MSSqlLocalDB;Initial Catalog=DBTarefas;Integrated Security=True;Pooling=False";

            conexaoComBanco = new SqlConnection { ConnectionString = enderecoDBTarefas };
            conexaoComBanco.Open();
            comando = new SqlCommand { Connection = conexaoComBanco };
        }

        private List<object> ObterParametros(IDataRecord linha)
        {
            List<object> parametros = new List<object>();
            for (int i = 0; i < linha.FieldCount; i++)
                parametros.Add(linha.GetValue(i));
            return parametros;
        }
        
        private static String SelecionarTabela()
        {
            if (typeof(T) == typeof(Tarefa)) return "TBTarefas";
            if (typeof(T) == typeof(Contato)) return "TBContatos";
            if (typeof(T) == typeof(Compromisso)) return "TBCompromissos";
            return "";
        }
        #endregion
    }
}


