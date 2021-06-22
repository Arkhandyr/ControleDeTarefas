using System;

namespace ControleDeTarefas.Telas
{
    public abstract class TelaBase
    {
        private string Titulo { get; set; }

        public TelaBase(string tit)
        {
            Titulo = tit;
        }

        public abstract void Menu();

        protected void MontarCabecalhoTabela(string configuracaoColunasTabela, params object[] colunas)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(configuracaoColunasTabela, colunas);
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.ResetColor();
        }

        protected void ApresentarMensagem(string mensagem, TipoMensagem tipoMensagem)
        {
            switch (tipoMensagem)
            {
                case TipoMensagem.Sucesso: Console.ForegroundColor = ConsoleColor.Green; break;
                case TipoMensagem.Atencao: Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                case TipoMensagem.Erro: Console.ForegroundColor = ConsoleColor.Red; break;
                default: break;
            }
            Console.WriteLine("\n" + mensagem);
            Console.ResetColor();
            Console.ReadKey();
        }

        protected void ConfigurarTela(string subtitulo)
        {
            Console.Clear();
            Console.WriteLine(Titulo);
            Console.WriteLine("\n" + subtitulo + "\n");
        }
    }
}