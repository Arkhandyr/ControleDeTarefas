using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleDeTarefas.Dominio;
using ControleDeTarefas.Controladores;

namespace ControleDeTarefas.Telas
{
    public class TelaPrincipal : TelaBase
    {
        private readonly Controlador<Tarefa> controladorTarefa = new Controlador<Tarefa>();
        private readonly Controlador<Contato> controladorContato = new Controlador<Contato>();
        private readonly Controlador<Compromisso> controladorCompromisso = new Controlador<Compromisso>();

        public TelaPrincipal() : base("Tela Principal")
        {
            //PopularTarefas();
            //PopularContatos();
            PopularCompromissos();

            while (true)
            {
                TelaBase tb = ObterTela();
                if (tb == null)
                {
                    ApresentarMensagem("Opção inválida", TipoMensagem.Erro);
                    continue;
                }
                tb.Menu();
            }
        }
        public TelaBase ObterTela()
        {
            Menu();
            switch (Console.ReadLine())
            {
                case "1": return new TelaTarefas(controladorTarefa);
                case "2": return new TelaContatos(controladorContato);
                case "3": return new TelaCompromissos(controladorCompromisso);
                default: return null;
            }
        }
        public override void Menu()
        {
            ConfigurarTela("Escolha uma opção: ");
            Console.WriteLine("1. Cadastro de Tarefas");
            Console.WriteLine("2. Cadastro de Contatos");
            Console.WriteLine("3. Cadastro de Compromissos\n");
            Console.Write("Opção: ");
        }

        #region Private methods
        private void PopularTarefas()
        {
            Controlador<Tarefa>.ResetarTabelaTarefas();
            Tarefa t1 = new Tarefa("Atividade 01", (Prioridade)1);
            Tarefa t2 = new Tarefa("Atividade 02", (Prioridade)2);
            Tarefa t3 = new Tarefa("Atividade 03", (Prioridade)3);
            Tarefa t4 = new Tarefa("Atividade 04", (Prioridade)1);
            Tarefa t5 = new Tarefa("Atividade 05", (Prioridade)2);
            Tarefa t6 = new Tarefa("Atividade 06", (Prioridade)3);
            Tarefa t7 = new Tarefa("Atividade 07", (Prioridade)1);
            Tarefa t8 = new Tarefa("Atividade 08", (Prioridade)2);
            Tarefa t9 = new Tarefa("Atividade 09", (Prioridade)3);
            Tarefa t10 = new Tarefa("Atividade 10", (Prioridade)1);

            controladorTarefa.InserirNovoRegistro(t1);
            controladorTarefa.InserirNovoRegistro(t2);
            controladorTarefa.InserirNovoRegistro(t3);
            controladorTarefa.InserirNovoRegistro(t4);
            controladorTarefa.InserirNovoRegistro(t5);
            controladorTarefa.InserirNovoRegistro(t6);
            controladorTarefa.InserirNovoRegistro(t7);
            controladorTarefa.InserirNovoRegistro(t8);
            controladorTarefa.InserirNovoRegistro(t9);
            controladorTarefa.InserirNovoRegistro(t10);
        }
        private void PopularContatos()
        {
            Controlador<Contato>.ResetarTabelaContatos();
            Contato c1 = new Contato("Victor Henrique", "arkhandyr@gmail.com", "+5549991368617", "NDD", "Estagiario");
            Contato c2 = new Contato("Joao Xavier", "joaoxavier@gmail.com", "+5549981372952", "NDD", "Estagiario");
            Contato c3 = new Contato("Vinicius Jordao", "vinijordao@gmail.com", "+5549998132718", "NDD", "Estagiario");
            Contato c4 = new Contato("Alexandre Rech", "alexandrerech@gmail.com", "+5549993271852", "NDD", "Professor");
            Contato c5 = new Contato("Valmir Tortelli", "tortelli@gmail.com", "+5549998132816", "NDD", "CEO");
            Contato c6 = new Contato("Jackson Cenci", "jacksondopandeiro@gmail.com", "+5549991382841", "NDD", "Diretor");
            Contato c7 = new Contato("Hugo", "hugo@gmail.com", "+5549991391752", "NDD", "Diretor");

            controladorContato.InserirNovoRegistro(c1);
            controladorContato.InserirNovoRegistro(c2);
            controladorContato.InserirNovoRegistro(c3);
            controladorContato.InserirNovoRegistro(c4);
            controladorContato.InserirNovoRegistro(c5);
            controladorContato.InserirNovoRegistro(c6);
            controladorContato.InserirNovoRegistro(c7);
        }
        private void PopularCompromissos()
        {
            Controlador<Compromisso>.ResetarTabelaCompromissos();
            Compromisso c1 = new Compromisso("Aula", "NDD", new DateTime(2021, 6, 29, 10, 00, 00), new DateTime(2022, 6, 29, 12, 00, 00), controladorCompromisso.SelecionarContatoPorId(4));
            Compromisso c2 = new Compromisso("Palestra", "Meet", new DateTime(2021, 6, 15, 8, 00, 00), new DateTime(2021, 6, 15, 12, 00, 00), controladorCompromisso.SelecionarContatoPorId(6));
            Compromisso c3 = new Compromisso("Atividade", "Casa", new DateTime(2021, 10, 1, 10, 00, 00), new DateTime(2021, 10, 1, 12, 00, 00), controladorCompromisso.SelecionarContatoPorId(2));
            Compromisso c4 = new Compromisso("Visita", "NDD", new DateTime(2021, 2, 10, 8, 00, 00), new DateTime(2021, 2, 10, 12, 00, 00), controladorCompromisso.SelecionarContatoPorId(5));
            Compromisso c5 = new Compromisso("Aula", "NDD", new DateTime(2021, 2, 23, 10, 00, 00), new DateTime(2021, 2, 23, 12, 00, 00), controladorCompromisso.SelecionarContatoPorId(4));
            Compromisso c6 = new Compromisso("Faculdade", "NDD", new DateTime(2020, 2, 23, 10, 00, 00), new DateTime(2020, 2, 23, 12, 00, 00), controladorCompromisso.SelecionarContatoPorId(1));

            controladorCompromisso.InserirNovoRegistro(c1);
            controladorCompromisso.InserirNovoRegistro(c2);
            controladorCompromisso.InserirNovoRegistro(c3);
            controladorCompromisso.InserirNovoRegistro(c4);
            controladorCompromisso.InserirNovoRegistro(c5);
            controladorCompromisso.InserirNovoRegistro(c6);
        }
        #endregion
    }
}
