
namespace GameEngine.Core.Serialization
{
    public interface ISerializer
    {
        string Serialize(GameObject gameObject);
        GameObject Deserialize(string data);
    }
}
