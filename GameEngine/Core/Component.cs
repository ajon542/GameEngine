
namespace GameEngine.Core
{
    public class Component
    {
        /// <summary>
        /// The game object containing this component.
        /// </summary>
        protected GameObject gameObject;

        /// <summary>
        /// Associate a game object with this component.
        /// </summary>
        /// <param name="go">The associated game object.</param>
        public void AssociateGameObject(GameObject go)
        {
            gameObject = go;
        }
    }
}
