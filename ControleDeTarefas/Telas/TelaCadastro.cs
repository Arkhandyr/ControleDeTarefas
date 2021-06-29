using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleDeTarefas.Controladores;
using ControleDeTarefas.Dominio;

namespace ControleDeTarefas.Telas
{
    public abstract class TelaCadastro<T> : TelaBase, ICadastravel where T : EntidadeBase
    {
        private readonly Controlador<T> controlador;
        public TelaCadastro(Controlador<T> controlador, string tit) : base(tit)
        {
            this.controlador = controlador;
        }

        public abstract T RegistroValido();

        public override void Menu()
        {
            Console.Clear();
            ConfigurarTela("Escolha uma opção: ");
            Console.WriteLine("1. INSERIR um registro");
            Console.WriteLine("2. VISUALIZAR registros");
            Console.WriteLine("3. EDITAR um registro");
            Console.WriteLine("4. EXCLUIR um registro");

            Console.WriteLine("Digite S para Voltar\n");
            Console.Write("Opção: ");

            switch (Console.ReadLine())
            {
                case "1": InserirNovoRegistro(); break;
                case "2": { if (VisualizarRegistros()) Console.ReadKey(); break; }
                case "3": EditarRegistro(); break;
                case "4": ExcluirRegistro(); break;
            }
        }

        public void InserirNovoRegistro()
        {
            ConfigurarTela("Inserindo um novo registro...");

            T registro = RegistroValido();

            string resultadoValidacao = controlador.InserirNovoRegistro(registro);

            if (resultadoValidacao == "REGISTRO_VALIDO")
                ApresentarMensagem("Registro inserido com sucesso!", TipoMensagem.Sucesso);
            else
            {
                ApresentarMensagem(resultadoValidacao, TipoMensagem.Erro);
                InserirNovoRegistro();
            }
        }

        public bool VisualizarRegistros()
        {
            List<T> registros = controlador.VisualizarRegistros();
            if (registros[0].GetType().Equals(typeof(Compromisso)))
            {
                String tipoVisualizacao = "";
                Console.Clear();
                ConfigurarTela("Escolha uma opção: ");
                Console.WriteLine("1. Visualizar todos os compromissos");
                Console.WriteLine("2. Visualizar compromissos passsados");
                Console.WriteLine("3. Visualizar compromissos futuros");
                Console.WriteLine("4. Visualizar compromissos do dia");
                Console.WriteLine("5. Visualizar compromissos do mes");

                Console.WriteLine("Digite S para Voltar\n");
                Console.Write("Opção: ");

                switch (Console.ReadLine())
                {
                    case "1": { tipoVisualizacao = "Todos"; break; }
                    case "2": { tipoVisualizacao = "Futuro"; break; }
                    case "3": { tipoVisualizacao = "Passado"; break; }
                    case "4": { tipoVisualizacao = "Dia"; break; }
                    case "5": { tipoVisualizacao = "Mes"; break; }

                }
                ConfigurarTela("Visualizando registros...");
                if (tipoVisualizacao == "Todos") Extensions.MostrarLista(controlador.VisualizarRegistros());
                if (tipoVisualizacao == "Futuro") Extensions.MostrarLista(controlador.VisualizarCompromissosFuturos());
                if (tipoVisualizacao == "Passado") Extensions.MostrarLista(controlador.VisualizarCompromissosPassados());
                if (tipoVisualizacao == "Dia") Extensions.MostrarLista(controlador.VisualizarCompromissosFuturosDoDia());
                if (tipoVisualizacao == "Mes") Extensions.MostrarLista(controlador.VisualizarCompromissosFuturosDoMes());
                return true;
            }
            else
            {
                ConfigurarTela("Visualizando registros...");
                Extensions.MostrarLista(controlador.VisualizarRegistros());
                return true;
            }
            
        }

        public void EditarRegistro()
        {
            ConfigurarTela("Editando um registro...");

            string opcao = ObterOpcao(controlador.Registros);
            if (opcao == "S") return;

            int id = Convert.ToInt32(opcao);
            controlador.EditarRegistro(id, RegistroValido());

            ApresentarMensagem("Operação realizada com sucesso!", TipoMensagem.Sucesso);
        }

        public void ExcluirRegistro()
        {
            ConfigurarTela("Excluindo um registro...");

            string opcao = ObterOpcao(controlador.Registros);
            if (opcao == "S") return;

            int id = Convert.ToInt32(opcao);
            controlador.ExcluirRegistro(id);

            ApresentarMensagem("Operação realizada com sucesso!", TipoMensagem.Sucesso);
        }

        protected bool OpcaoValida(string opcao, List<T> lista)
        {
            return int.TryParse(opcao, out int id) && lista.Exists(x => x.ID == id);
        }

        protected String ObterOpcao(List<T> lista)
        {
            Console.Clear();

            if (!lista.MostrarLista())
                return "S";

            Console.Write("\nDigite o ID do registro que deseja editar: ");
            String opcao = Console.ReadLine().ToUpperInvariant();

            if (opcao == "S") return opcao;

            if (!OpcaoValida(opcao, lista))
            {
                ApresentarMensagem("Esta opção não existe: " + opcao, TipoMensagem.Erro);
                return ObterOpcao(lista);
            }
            return opcao;
        }
    }

    public static class Extensions
    {
        public static bool MostrarLista(this IList lista)
        {
            if (lista.Count == 0)
            {
                Console.WriteLine("Nenhum item aqui!");
                return false;
            }
            else
            {
                foreach (var item in lista)
                    Console.WriteLine(item.ToString());
                return true;
            }
        }
    }
}
