using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleDeTarefas.Dominio;
using ControleDeTarefas.Telas;

namespace ControleDeTarefas.Controladores
{
    public class Controlador<T> where T : EntidadeBase
    {
        public List<T> Registros { get => VisualizarRegistros(); }

        public Controlador()
        {
        }

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

        private List<object> ObterParametros(IDataRecord linha)
        {
            List<object> parametros = new List<object>();
            for (int i = 0; i < linha.FieldCount; i++)
                parametros.Add(linha.GetValue(i));
            return parametros;
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

        public static String SelecionarTabela()
        {
            if (typeof(T) == typeof(Tarefa)) return "TBTarefas";
            if (typeof(T) == typeof(Contato)) return "TBContatos";
            return "";
        }

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

        
        private static void InicializarBanco(out SqlConnection conexaoComBanco, out SqlCommand comando)
        {
            string enderecoDBTarefas =
                @"Data Source=(LocalDb)\MSSqlLocalDB;Initial Catalog=DBTarefas;Integrated Security=True;Pooling=False";

            conexaoComBanco = new SqlConnection { ConnectionString = enderecoDBTarefas };
            conexaoComBanco.Open();
            comando = new SqlCommand { Connection = conexaoComBanco };
        }
    }
}
