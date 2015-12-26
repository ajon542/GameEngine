using System.Windows.Forms;

namespace GameEngine.Core
{
    /// <summary>
    /// The behaviour component.
    /// </summary>
    public class Behaviour : Component
    {
        // A GameObject is going to be controlled by a BehaviourComponent.
        // This will allow actions such as initialization and updates.

        /// <summary>
        /// Initialization phase of all the game objects.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// All game objects are initialized and are ready to go.
        /// </summary>
        public virtual void Start()
        {
        }

        /// <summary>
        /// Update the game object.
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// Handle the key press events.
        /// </summary>
        /// <param name="key">The key pressed event args.</param>
        public virtual void KeyDown(KeyEventArgs key)
        {
            
        }

        /// <summary>
        /// Reset the game object.
        /// </summary>
        public virtual void Reset()
        {
        }
    }
}
