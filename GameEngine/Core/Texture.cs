﻿using System.Drawing;
using System.Drawing.Imaging;

using OpenTK.Graphics.OpenGL;

namespace GameEngine.Core
{
    public static class Texture
    {
        /// <summary>
        /// Loads a texture from file.
        /// </summary>
        /// <param name="filename">The file name.</param>
        /// <returns>The generated texture id.</returns>
        public static int LoadTexture(string filename)
        {
            // TODO: Probably need to check if the file exists.

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            // We will not upload mipmaps, so disable mipmapping (otherwise the texture will not appear).
            // We can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
            // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            Bitmap bmp = new Bitmap(filename);
            BitmapData bmpData = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);

            bmp.UnlockBits(bmpData);

            return id;
        }
    }
}