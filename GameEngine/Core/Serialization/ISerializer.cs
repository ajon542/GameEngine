
namespace GameEngine.Core.Serialization
{
    public interface ISerializer<T>
    {
        string Serialize(T gameObject);
        T Deserialize<T>(string data);
    }
}
