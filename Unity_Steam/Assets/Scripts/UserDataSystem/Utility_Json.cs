using Newtonsoft.Json;  //https://github.com/jilleJr/Newtonsoft.Json-for-Unity.git#upm

public class Utility_Json
{
    static public string ObjectToJson(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    static public T JsonToOject<T>(string jsonData)
    {
        return JsonConvert.DeserializeObject<T>(jsonData);
    }
}