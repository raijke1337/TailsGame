using UnityEngine;

namespace Arcatech.Managers.Save
{
    public class JsonSerializer : ISerializer
    {
        public JsonSerializer()
        {
            Extension = "json";
        }

        public string Extension { get; }

        public T Deserialize<T>(string data)
        {
            return JsonUtility.FromJson<T>(data);
        }

        public string Serialize<T>(T arg)
        {
            return JsonUtility.ToJson(arg, true);
        }
    }


}

