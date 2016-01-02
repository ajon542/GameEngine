using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using OpenTK;

using GameEngine.Core.Debugging;

namespace GameEngine.Core
{
    public class GameObject
    {
        // This is intended to be the base class of all objects that can
        // exist in the scene.
        // At minimum, a GameObject is going to have a position, scale and rotation.
        // A GameObject might have a mesh, renderer, shader, input controllers etc.
        // The entire scene will consist of a group of GameObjects all interacting with
        // each other, or not at all. Scripts will be controlling the behaviour of each
        // GameObject. The purpose of this game engine is to render the scene. It is
        // the game developers responsibility to provide the behaviour through scripts
        // attached to GameObjects.
        // A GameObject can be attached to other GameObjects and be influenced by the
        // behaviour of their parent.

        // Use case:
        // Add an empty GameObject to the hierarchy.
        // Add different components to the GameObjects.
        // GameObject is rendered in scene and game view.

        /// <summary>
        /// Get or sets the guid associated with the game object.
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the name of the game object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the transform of the game object.
        /// </summary>
        public Transform Transform { get; set; }

        /// <summary>
        /// The parent game object.
        /// </summary>
        [JsonProperty]
        private GameObject parent;

        /// <summary>
        /// The list of children game objects.
        /// </summary>
        [JsonProperty]
        private List<GameObject> children;

        /// <summary>
        /// The list of components attached to the game object.
        /// </summary>
        [JsonProperty]
        private List<Component> components;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObject"/> class.
        /// </summary>
        public GameObject()
            : this("GameObject")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObject"/> class.
        /// </summary>
        public GameObject(string name)
        {
            Name = name;
            children = new List<GameObject>();
            components = new List<Component>();
            Transform = new Transform();
            Guid = Guid.NewGuid();
        }

        /// <summary>
        /// Add a child game object.
        /// </summary>
        /// <param name="child">The child game object to add.</param>
        public void AddChild(GameObject child)
        {
            if(child == null)
            {
                throw new GameEngineException("child cannot be null");
            }

            children.Add(child);
            child.SetParent(this);
        }

        /// <summary>
        /// Remove any children game objects.
        /// </summary>
        public void RemoveChildren()
        {
            children = new List<GameObject>();
        }

        /// <summary>
        /// Gets the parent of the game object.
        /// </summary>
        /// <returns>The parent of the game object.</returns>
        public GameObject GetParent()
        {
            return parent;
        }

        /// <summary>
        /// Gets the list of child game objects.
        /// </summary>
        /// <returns>The list of child game objects.</returns>
        public List<GameObject> GetChildren()
        {
            return children;
        }

        /// <summary>
        /// Set the parent of the game object.
        /// </summary>
        /// <param name="parent">The parent of the game object.</param>
        public void SetParent(GameObject parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Adds a component to the game object.
        /// </summary>
        /// <typeparam name="T">The type of component to add.</typeparam>
        /// <param name="component">The component to add.</param>
        public void AddComponent<T>(Component component) where T : class
        {
            if (GetComponent<T>() == null)
            {
                // Associate this game object with the component.
                component.AssociateGameObject(this);

                // Add the component to the list.
                components.Add(component);
            }
        }

        /// <summary>
        /// Removes a component from the game object.
        /// </summary>
        /// <typeparam name="T">The type of component to remove.</typeparam>
        public void RemoveComponent<T>() where T : class
        {
            // Search for a component of type T.
            for (int i = 0; i < components.Count; ++i)
            {
                if (components[i] is T)
                {
                    // Remove the component if found.
                    components.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Gets a component of type T.
        /// </summary>
        /// <typeparam name="T">The type of component to get.</typeparam>
        /// <returns>The component or null if not found.</returns>
        public T GetComponent<T>() where T : class
        {
            // Search for a component of type T.
            foreach (Component component in components)
            {
                if (component is T)
                {
                    return component as T;
                }
            }

            // Could not find component of type T.
            return null;
        }

        /// <summary>
        /// Gets the list of components.
        /// </summary>
        /// <returns>The list of components.</returns>
        public List<Component> GetComponents()
        {
            return components;
        }

        public override string ToString()
        {
            return Name;
        }

        // TODO: Address these for serialization
        [JsonIgnore]
        public Matrix4 ModelMatrix = Matrix4.Identity;
        [JsonIgnore]
        public Matrix4 ViewProjectionMatrix = Matrix4.Identity;
        [JsonIgnore]
        public Matrix4 ModelViewProjectionMatrix = Matrix4.Identity;

        public virtual void CalculateModelMatrix()
        {
            ModelMatrix =
                Matrix4.CreateScale(Transform.Scale) *
                Matrix4.CreateRotationX(Transform.Rotation.X) *
                Matrix4.CreateRotationY(Transform.Rotation.Y) *
                Matrix4.CreateRotationZ(Transform.Rotation.Z) *
                Matrix4.CreateTranslation(Transform.Position);
        }
    }
}
