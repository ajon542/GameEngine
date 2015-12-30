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

        /// <summary>
        /// Gets the level in the tree hierarchy of the game object.
        /// </summary>
        /// <remarks>
        /// The root game object, i.e. the one with no parent has level 0.
        /// The children of the root game object have level 1 and so on.
        /// </remarks>
        /// <param name="gameObject">The game object for which to find the level for.</param>
        /// <returns>The level of the game object.</returns>
        public static int GetLevel(GameObject gameObject)
        {
            int level = 0;

            GameObject parent = gameObject.GetParent();
            while (parent != null)
            {
                parent = parent.GetParent();
                level++;
            }

            return level;
        }

        /// <summary>
        /// Find the closest common parent of two game objects.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="gameObject1">The first game object.</param>
        /// <param name="gameObject2">The second game object.</param>
        /// <returns>The game object with the matching guid or null.</returns>
        public static GameObject FindClosestCommonParent(GameObject gameObject1, GameObject gameObject2)
        {
            // TODO: Create a function to GetRoot();
            // TODO: What to do if the one of the game objects is already root?
            // TODO: What if their root objects are different?

            // Get the levels of the objects.
            int level1 = GetLevel(gameObject1);
            int level2 = GetLevel(gameObject2);

            // Get each object to the same level.
            int diff = Math.Abs(level1 - level2);

            if(level2 > level1)
            {
                while(level2 > level1)
                {
                    gameObject2 = gameObject2.GetParent();
                    level2--;
                }
            }
            else if (level1 > level2)
            {
                while(level1 > level2)
                {
                    gameObject1 = gameObject1.GetParent();
                    level1--;
                }
            }

            // Get the parent ofthe game objects.
            GameObject p1 = gameObject1.GetParent();
            GameObject p2 = gameObject2.GetParent();

            // Iterate through the parents until they match or we reach the root.
            while(level1 > 0 && p1.Guid != p2.Guid)
            {
                p1 = p1.GetParent();
                p2 = p2.GetParent();
                level1--;
            }

            return p1;
        }
    }
}
