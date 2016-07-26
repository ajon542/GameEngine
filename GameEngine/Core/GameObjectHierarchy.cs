using System;
using System.Collections.Generic;

using NLog;

namespace GameEngine.Core
{
    /// <summary>
    /// Provides some useful methods to traverse the game object hierarchy.
    /// </summary>
    public static class GameObjectHierarchy
    {
        /// <summary>
        /// Reference to the logging mechanism.
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

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

            logger.Log(LogLevel.Info, "could not find game object with guid {0}", guid);

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
        /// Gets the root game object.
        /// </summary>
        /// <remarks>
        /// If the game object is already root, it returns the game object.
        /// </remarks>
        /// <param name="gameObject">The game object from which to find the root.</param>
        /// <returns>The root game object.</returns>
        public static GameObject GetRoot(GameObject gameObject)
        {
            GameObject parent = gameObject;
            while (parent.GetParent() != null)
            {
                parent = parent.GetParent();
            }

            return parent;
        }

        /// <summary>
        /// Find the closest common parent of two game objects.
        /// </summary>
        /// <remarks>
        /// If the game objects do not have a common root the method returns null.
        /// </remarks>
        /// <param name="gameObject1">The first game object.</param>
        /// <param name="gameObject2">The second game object.</param>
        /// <returns>The game object with the matching guid or null.</returns>
        public static GameObject FindClosestCommonParent(GameObject gameObject1, GameObject gameObject2)
        {
            // If the game objects do not have a common root, they cannot
            // be part of the same hierarchy.
            if (GetRoot(gameObject1).Guid != GetRoot(gameObject2).Guid)
            {
                logger.Log(LogLevel.Info, "game objects do not have a common root");
                return null;
            }

            // Get the levels of the objects.
            int level1 = GetLevel(gameObject1);
            int level2 = GetLevel(gameObject2);

            while (level1 >= 0 && gameObject1.Guid != gameObject2.Guid)
            {
                if(level1 > level2)
                {
                    gameObject1 = gameObject1.GetParent();
                    level1--;
                }
                else if(level2 > level1)
                {
                    gameObject2 = gameObject2.GetParent();
                    level2--;
                }
                else
                {
                    gameObject1 = gameObject1.GetParent();
                    gameObject2 = gameObject2.GetParent();
                    level1--;
                }
            }

            return gameObject1;
        }
    }
}
