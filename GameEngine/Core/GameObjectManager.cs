using System;
using System.Collections.Generic;

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
    /// While it is possible to modify the game object hierarchy by following the
    /// game object parent and child links, this class will provide some of the
    /// common tasks needed.
    /// </remarks>
    class GameObjectManager
    {
        /// <summary>
        /// The root game object. A scene will always have one of these.
        /// </summary>
        private GameObject root = new GameObject();

        /// <summary>
        /// Add a child to the game object with the given guid.
        /// </summary>
        /// <param name="guid">The guid of the parent object.</param>
        /// <param name="child">The child object to add.</param>
        public void AddChild(Guid guid, GameObject child)
        {
            GameObject gameObject = FindGameObject(guid);

            if (gameObject != null)
            {
                gameObject.AddChild(child);
            }
        }

        /// <summary>
        /// Add a list of children to the game object with the given guid.
        /// </summary>
        /// <param name="guid">The guid of the parent object.</param>
        /// <param name="children">The list of child objects to add.</param>
        public void AddChildren(Guid guid, List<GameObject> children)
        {
            GameObject gameObject = FindGameObject(guid);

            if (gameObject != null)
            {
                foreach (GameObject child in children)
                {
                    gameObject.AddChild(child);
                }
            }
        }

        /// <summary>
        /// Remove all children from their parent.
        /// </summary>
        /// <param name="guid">The guid of the parent object.</param>
        public void RemoveChildren(Guid guid)
        {
            GameObject gameObject = FindGameObject(guid);

            if (gameObject != null)
            {
                gameObject.RemoveChildren();
            }
        }

        /// <summary>
        /// Find the game object with the given guid.
        /// </summary>
        /// <param name="guid">The guid to find.</param>
        /// <returns>The game object with the guid or null.</returns>
        public GameObject FindGameObject(Guid guid)
        {
            Queue<GameObject> unvisited = new Queue<GameObject>();
            unvisited.Enqueue(root);

            while (unvisited.Count != 0)
            {
                GameObject current = unvisited.Dequeue();

                List<GameObject> children = current.GetChildren();
                foreach (GameObject child in children)
                {
                    if (child.Guid == guid)
                    {
                        return child;
                    }

                    unvisited.Enqueue(child);
                }
            }
            return null;
        }
    }
}
