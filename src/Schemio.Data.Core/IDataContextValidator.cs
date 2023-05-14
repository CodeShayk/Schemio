namespace Schemio.Data.Core
{
    public interface IDataContextValidator
    {
        public void Validate(IDataContext context);
    }
}