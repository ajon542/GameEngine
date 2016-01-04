using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using GameEngine.Core.Graphics;

namespace GameEngine.Core
{
    public class Grid
    {
        // The grid class will be used to store the data to display
        // a grid in the scene. The grid shall provide some orientation
        // in the scene i.e. to look like the ground.

        // width
        // height
        // space
        // scale - scale the grid
        // resize - increase the width and height, add more spaces

        //  -----------------  
        // |        |        |
        // |        |        |
        // |        |        |
        // |--------|--------|
        // |        |        |
        // |        |        |
        // |        |        |
        //  ------------------

        public List<Vector3> Vertices { get; set; }
        public List<Vector3> Colours { get; set; }
        public List<int> Indices { get; set; }

        public Grid()
        {
            Vertices = new List<Vector3>
            {
                new Vector3(0, 0, -3),
                new Vector3(0, 1, -3),
                new Vector3(1, 0, -3),
                new Vector3(1, 1, -3),
            };
            Colours = new List<Vector3>
            {
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
            };
            Indices = new List<int>
            {
                0, 1, 2, 3
            };
        }
    }
}
