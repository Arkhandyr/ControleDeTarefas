using ControleDeTarefas.Controladores;
using ControleDeTarefas.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ControleDeTarefas.Telas
{
    public class TelaContatos : TelaCadastro<Contato>
    {

        public TelaContatos(Controlador<Contato> controlador) : base(controlador, "Cadastro de Contatos")
        {
        }

        public override Contato RegistroValido()
        {
            String nome, email, telefone, empresa, cargo;
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Digite o nome do contato\n");
                nome = Console.ReadLine();
                if (nome.Length > 0)
                    break;
                Console.WriteLine("Título não pode ser vazio\n");
            }
            while (true)
            {
                Console.WriteLine("\nDigite o E-Mail do contato:\n");
                email = Console.ReadLine();
                if (ValidarEmail(email))
                    break;
                Console.WriteLine("E-Mail inválido\n");
            }
            while (true)
            {
                Console.WriteLine("\nDigite o telefone do contato:\n");
                telefone = Console.ReadLine();
                if (ValidarTelefone(telefone))
                    break;
                Console.WriteLine("Telefone inválido\n");
            }
            while (true)
            {
                Console.WriteLine("\nDigite a empresa do contato:\n");
                empresa = Console.ReadLine();
                if (empresa.Length > 0)
                    break;
                Console.WriteLine("Campo 'empresa' não pode ser vazio\n");
            }
            while (true)
            {
                Console.WriteLine("\nDigite o cargo do contato:\n");
                cargo = Console.ReadLine();
                if (cargo.Length > 0)
                    break;
                Console.WriteLine("Campo 'cargo' não pode ser vazio\n");
            }

            return new Contato(nome, email, telefone, empresa, cargo);
        }

        private bool ValidarEmail(string email)
        {
            return Regex.IsMatch
            (
               email,
               @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",
               RegexOptions.IgnoreCase
            );
        }

        private bool ValidarTelefone(string telefone)
        {
            return Regex.IsMatch
            (
               telefone,
               @"(\(?\d{2}\)?\s)?(\d{4,5}\d{4})",
               RegexOptions.IgnoreCase
            );
        }
    }
}
