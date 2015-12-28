using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using OpenTK;

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

        // One thing I'm going to have to solve is the view-model to view scene data
        // transfer. I say data transfer, but it is really just a way of notifying the
        // view of the information it needs to display the scene.
        // The scene is not going to be doing much apart from setting the stage for the
        // game. The game is really going to be doing all the work and is to be handling
        // the object interaction.
        // The scene should be able to be described with a simple data format. It is the
        // scripts attached to the objects that will determine their behaviour in the scene.

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
                return;
            }

            children.Add(child);
            child.SetParent(this);
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
        public void AddComponent<T>(Component component)
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
        /// Gets a component of type T.
        /// </summary>
        /// <typeparam name="T">The type of component to get.</typeparam>
        /// <returns>The component or null if not found.</returns>
        public Component GetComponent<T>()
        {
            // Search for a component of type T.
            foreach (Component component in components)
            {
                if (component is T)
                {
                    return component;
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
