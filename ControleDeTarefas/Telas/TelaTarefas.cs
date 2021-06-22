using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleDeTarefas.Dominio;
using ControleDeTarefas.Controladores;

namespace ControleDeTarefas.Telas
{
    public class TelaTarefas : TelaCadastro<Tarefa>
    {

        public TelaTarefas(Controlador<Tarefa> controlador) : base(controlador, "Cadastro de Tarefas")
        {
        }

        public override Tarefa RegistroValido()
        {
            String titulo;
            Prioridade prioridade;
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Digite o título da Tarefa\n");
                titulo = Console.ReadLine();
                if (titulo.Length > 0)
                    break;
                Console.WriteLine("Título não pode ser vazio");
            }
            while (true)
            {
                Console.WriteLine("\nDigite a prioridade da Tarefa (1 para baixa, 2 para  e 3 para alta):\n");
                prioridade = (Prioridade)Convert.ToInt32(Console.ReadLine());
                if ((int)prioridade == 1 || (int)prioridade == 2 || (int)prioridade == 3)
                    break;
                Console.WriteLine("Prioridade precisa ser numérica entre 1 e 3");
            }

            return new Tarefa(titulo, prioridade);
        }
    }
}
