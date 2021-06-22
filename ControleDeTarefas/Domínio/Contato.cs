using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeTarefas.Dominio
{
    public class Contato : EntidadeBase
    {
        #region Atributos
        public String Nome { get; set; }
        public String Email { get; set; }
        public String Telefone { get; set; }
        public String Empresa { get; set; }
        public String Cargo { get; set; }
        #endregion

        public Contato(String nome, String email, String telefone, String empresa, String cargo)
        {
            this.Nome = nome;
            this.Email = email;
            this.Telefone = telefone;
            this.Empresa = empresa;
            this.Cargo = cargo;
        }

        public Contato()
        {
        }

        public override string Validar()
        {
            return "REGISTRO_VALIDO";
        }

        public override String ToString()
        {
            return $"ID: {ID} | Nome: {Nome} | Email: {Email} | Telefone: {Telefone} | Empresa: {Empresa} | Cargo: {Cargo}\n";
        }
    }
}
