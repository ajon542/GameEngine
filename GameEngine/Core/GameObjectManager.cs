using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core
{
    /// <summary>
    /// The Game Object Manager is responsible for managing all the game objects
    /// in a scene.
    /// </summary>
    /// <remarks>
    /// Scenes can consist of a complex game object hierarchy, starting with a
    /// single root game object linking to many child game objects. Being able
    /// to manage and debug this hierarchy will be important.
    /// </remarks>
    class GameObjectManager
    {
        /// <summary>
        /// The root game object. A scene will always have one of these.
        /// </summary>
        private GameObject root = new GameObject();

        public void AddChildTo(Guid guid, GameObject child)
        {
            throw new NotImplementedException();
        }

        public void AddChildrenTo(Guid guid, List<GameObject> children)
        {
            throw new NotImplementedException();
        }

        public void RemoveChildrenFrom(Guid guid)
        {
            throw new NotImplementedException();
        }

        public GameObject FindGameObject(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}
