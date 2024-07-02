namespace Arcatech.Managers.Save
{
    public interface ISerializer
    {
        public string Serialize<T>(T arg);
        public T Deserialize<T>(string data);
        public string Extension { get; }
    }


}

