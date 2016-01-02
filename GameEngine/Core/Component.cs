using Newtonsoft.Json;

namespace GameEngine.Core
{
    public class Component
    {
        /// <summary>
        /// The game object containing this component.
        /// </summary>
        public GameObject GameObject { get; set; }

        /// <summary>
        /// Associate a game object with this component.
        /// </summary>
        /// <param name="go">The associated game object.</param>
        public void AssociateGameObject(GameObject go)
        {
            GameObject = go;
        }
    }
}
