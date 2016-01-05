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

        private Vector3 defaultColor = new Vector3(.3f, .3f, .3f);

        public Grid()
        {
            Vertices = new List<Vector3>();
            Colours = new List<Vector3>();
            Indices = new List<int>();

            int indexCount = 0;
            for (int i = 0; i >= -1000; i -= 1)
            {
                Vertices.Add(new Vector3(i, 0.0f, 0.0f));
                Colours.Add(defaultColor);
                Indices.Add(indexCount++);

                Vertices.Add(new Vector3(i, 0.0f, -1000));
                Colours.Add(defaultColor);
                Indices.Add(indexCount++);

                Vertices.Add(new Vector3(0.0f, 0.0f, i));
                Colours.Add(defaultColor);
                Indices.Add(indexCount++);

                Vertices.Add(new Vector3(-1000, 0.0f, i));
                Colours.Add(defaultColor);
                Indices.Add(indexCount++);
            }
        }
    }
}
