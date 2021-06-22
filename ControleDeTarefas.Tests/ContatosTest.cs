using ControleDeTarefas.Controladores;
using ControleDeTarefas.Dominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ControleDeContatos.Tests
{
    [TestClass]
    public class ContatosTest
    {
        private readonly Controlador<Contato> controladorContato = new Controlador<Contato>();

        [TestMethod]
        public void DeveInserirContato()
        {
            Contato c1 = new Contato("Victor", "arkhandyr@gmail.com", "49991368617", "NDD", "Estagiário");

            string resultado = controladorContato.InserirNovoRegistro(c1);

            Assert.AreEqual("REGISTRO_VALIDO", resultado);
        }

        [TestMethod]
        public void DeveExcluirContato()
        {
            Contato c1 = new Contato("Victor", "arkhandyr@gmail.com", "49991368617", "NDD", "Estagiário");

            controladorContato.InserirNovoRegistro(c1);

            controladorContato.ExcluirRegistro(c1.ID);

            List<Contato> contatos = controladorContato.VisualizarRegistros();

            Assert.AreEqual(2, (contatos.Count) % 4); //testes adicionam 5 e removem 1
        }

        [TestMethod]
        public void DeveBuscarTodasContatos()
        {
            Contato c1 = new Contato("Victor Henrique", "arkhandyr@gmail.com", "+5549991368617", "NDD", "Estagiario");
            Contato c2 = new Contato("Joao Xavier", "joaoxavier@gmail.com", "+5549981372952", "NDD", "Estagiario");
            Contato c3 = new Contato("Vinicius Jordao", "vinijordao@gmail.com", "+5549998132718", "NDD", "Estagiario");

            controladorContato.InserirNovoRegistro(c1);
            controladorContato.InserirNovoRegistro(c2);
            controladorContato.InserirNovoRegistro(c3);

            List<Contato> Contatos = controladorContato.VisualizarRegistros();

            Assert.IsTrue(Contatos.Count >= 3);
        }
    }
}
