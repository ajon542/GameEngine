using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    public class InputExample : Scene
    {
        private int mouseWheelIndex = 0;

        public override void Update()
        {
            var mouse = Mouse.GetState();
            if (mouse[MouseButton.Left])
            {
                Console.WriteLine("mouse button pressed");
            }
            var keyboard = Keyboard.GetState();
            if (keyboard[Key.A])
            {
                Console.WriteLine("Key.A was pressed");
            }

            if (mouseWheelIndex != mouse.Wheel)
            {
                mouseWheelIndex = mouse.Wheel;
                Console.WriteLine(mouse.Wheel);
            }
        }
    }
}
