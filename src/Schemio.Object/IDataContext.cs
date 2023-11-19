namespace Schemio.Object
{
    public interface IDataContext
    {
        public string[] Paths { get; set; }
        decimal CurrentVersion { get; }
    }
}