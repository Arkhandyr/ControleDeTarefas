using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeTarefas.Dominio
{
    public class Tarefa : EntidadeBase
    {
        #region Atributos
        public String Titulo { get; set; }
        public DateTime DataInicial { get; private set; }
        public DateTime DataConclusao { get; set; }
        public String Porcentagem { get; set; }
        public Prioridade Prioridade { get; set; }
        #endregion

        public Tarefa(string titulo, Prioridade prioridade)
        {
            this.Titulo = titulo;
            DataInicial = DateTime.Now;
            DataConclusao = DateTime.MaxValue;
            Porcentagem = "0%";
            this.Prioridade = prioridade;
        }

        public Tarefa(string titulo, DateTime dataInicial, DateTime dataConclusao, String porcentagem, int prioridade)
        {
            Titulo = titulo;
            DataInicial = dataInicial;
            DataConclusao = dataConclusao;
            Porcentagem = porcentagem;
            Prioridade = (Prioridade)prioridade;
        }

        public Tarefa()
        {
        }

        public override string Validar()
        {
            return "REGISTRO_VALIDO";
        }

        public override String ToString()
        {
            return $"ID: {ID} | Titulo: {Titulo} | Prioridade: {Prioridade} | Porcentagem: {Porcentagem} | Data Inicial: {DataInicial.ToShortDateString()} " +
            $"{(DataConclusao != DateTime.MaxValue ? $" | Conclusão: {DataConclusao.ToShortDateString()}" : "")}\n";
        }
    }
}