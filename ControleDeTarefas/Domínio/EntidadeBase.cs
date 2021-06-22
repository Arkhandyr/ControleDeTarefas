namespace ControleDeTarefas.Dominio
{
    public abstract class EntidadeBase
    {
        public int ID { get; set; }
        public abstract string Validar();
    }
}