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
            ConfigurarTela("Visualizando registros...");
            Extensions.MostrarLista(controlador.VisualizarRegistros());
            return true;
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
