using ControleDeTarefas.Controladores;
using ControleDeTarefas.Dominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ControleDeTarefas.Tests
{
    [TestClass]
    public class TarefasTest
    {
        private readonly Controlador<Tarefa> controladorTarefa = new Controlador<Tarefa>();

        [TestMethod]
        public void DeveInserirTarefa()
        {
            Tarefa t1 = new Tarefa("Atividade 01", (Prioridade)3);

            string resultado = controladorTarefa.InserirNovoRegistro(t1);
            
            Assert.AreEqual("REGISTRO_VALIDO", resultado);
        }

        [TestMethod]
        public void DeveExcluirTarefa()
        {
            Tarefa t1 = new Tarefa("Atividade 01", (Prioridade)3);

            controladorTarefa.InserirNovoRegistro(t1);

            controladorTarefa.ExcluirRegistro(t1.ID);

            List<Tarefa> tarefas = controladorTarefa.VisualizarRegistros();

            Assert.AreEqual(1, (tarefas.Count % 4)); //testes adicionam 5 e removem 1
        }

        [TestMethod]
        public void DeveBuscarTodasTarefas()
        {
            Tarefa t1 = new Tarefa("Atividade 01", (Prioridade)3);
            Tarefa t2 = new Tarefa("Atividade 02", (Prioridade)2);
            Tarefa t3 = new Tarefa("Atividade 03", (Prioridade)1);

            controladorTarefa.InserirNovoRegistro(t1);
            controladorTarefa.InserirNovoRegistro(t2);
            controladorTarefa.InserirNovoRegistro(t3);

            List<Tarefa> tarefas = controladorTarefa.VisualizarRegistros();

            Assert.IsTrue(tarefas.Count >= 3);
        }
    }
}
