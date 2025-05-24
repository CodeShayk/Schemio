namespace Schemio.Core
{
    public interface IEntityContextValidator
    {
        void Validate(IEntityRequest context);
    }
}