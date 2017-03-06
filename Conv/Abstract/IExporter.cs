namespace Conv.Abstract
{
    public interface IExporter<in T>
    {
        void Export(T data);
    }
}