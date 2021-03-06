﻿using System;
using System.Collections.Generic;
using GameEngine.Core.Debugging;

namespace GameEngine.Core.Utilities.ObjParser
{
    // List of vertex normals in (x,y,z) form; normals might not be unit vectors.
    // vn 0.707 0.000 0.707
    class NormalParser : BaseType
    {
        private List<Normal> normals;

        protected override string Id { get { return "vn"; } }

        public NormalParser(List<Normal> normals)
        {
            this.normals = normals;
        }

        public override void Parse(string input)
        {
            string[] normalData = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (normalData.Length != 3)
            {
                throw new GameEngineException("could not deserialize normal data");
            }

            normals.Add(new Normal
            {
                X = float.Parse(normalData[0]),
                Y = float.Parse(normalData[1]),
                Z = float.Parse(normalData[2]),
            });
        }
    }
}
