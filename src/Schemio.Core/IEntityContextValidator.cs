namespace Schemio.Core
{
    public interface IEntityContextValidator
    {
        public void Validate(IEntityRequest context);
    }
}