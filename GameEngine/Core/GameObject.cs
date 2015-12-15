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
        /// Gets or sets the name of the game object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the transform of the game object.
        /// </summary>
        [JsonProperty]
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

        public GameObject()
        {
            Name = "GameObject";
            children = new List<GameObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObject"/> class.
        /// </summary>
        public GameObject(string name)
        {
            Name = name;
            children = new List<GameObject>();

            Transform = new Transform();
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
        /// Set the parent of the game object.
        /// </summary>
        /// <param name="parent">The parent of the game object.</param>
        public void SetParent(GameObject parent)
        {
            this.parent = parent;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
