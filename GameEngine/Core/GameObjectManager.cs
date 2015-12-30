using System;
using System.Collections.Generic;

namespace GameEngine.Core
{
    public static class GameObjectManager
    {
        /// <summary>
        /// Find the game object with the given guid.
        /// </summary>
        /// <param name="root">The root game object to start searching from.</param>
        /// <param name="guid">The guid to find.</param>
        /// <returns>The game object with the matching guid or null.</returns>
        public static GameObject FindGameObject(GameObject root, Guid guid)
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
