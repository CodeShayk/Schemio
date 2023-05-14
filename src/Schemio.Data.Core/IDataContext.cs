namespace Schemio.Data.Core
{
    public interface IDataContext
    {
        public string[] Paths { get; set; }
        string CurrentVersion { get; }
    }
}