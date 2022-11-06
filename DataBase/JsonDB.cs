using Newtonsoft.Json;
using Authorisation2.Entities;
using Authorisation2.Model;
using Microsoft.Extensions.Options;

namespace Authorisation2.DataBase;

public class JsonDB
{
    private readonly string JsonFileName;
    public JsonDB (IOptions<Settings> options)
    {
        JsonFileName = options.Value.JsonUser;
    }

    public void WriteToJson (List <User> userList)
    {
        var JsonData = JsonConvert.SerializeObject(userList);
        File.WriteAllText(JsonFileName, JsonData);
    }

    public List<User>? ReadFromJson ()
    {
        if (!File.Exists (JsonFileName)) return null;

        var jsonString = File.ReadAllText (JsonFileName);
        var jsonData   = JsonConvert.DeserializeObject<List<User>> (jsonString);
        return jsonData;
    }
}
