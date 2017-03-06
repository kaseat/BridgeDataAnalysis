namespace Conv.Abstract
{
    public interface IImporter<out T>
    {
        T Import();
    }
}