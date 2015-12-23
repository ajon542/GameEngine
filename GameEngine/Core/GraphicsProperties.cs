
namespace GameEngine.Core
{
    /// <summary>
    /// Container for properties related to graphics.
    /// </summary>
    public class GraphicsProperties
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsProperties"/> class.
        /// </summary>
        /// <param name="width">The width of the rendering view.</param>
        /// <param name="height">The height of the rendering view.</param>
        public GraphicsProperties(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets the width of the rendering view.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height of the rendering view.
        /// </summary>
        public int Height { get; private set; }
    }
}
