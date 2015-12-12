using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Threading;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.View
{
    /// <summary>
    /// Interaction logic for SceneView.xaml
    /// </summary>
    public partial class SceneView : System.Windows.Controls.UserControl
    {
        public SceneView()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }

        #region Example DependencyProperties

        // This is an example of a DependencyProperty. In the MainWindow.xaml when we
        // setup the <view:SceneView DrawFPS="False"/>
        // We can now use this property within the SceneView.xaml.cs code.
        // TODO: Can we use this in other ways?
        private static readonly DependencyProperty DrawFPSProperty =
          DependencyProperty.Register("DrawFPS", typeof(bool), typeof(SceneView),
          new PropertyMetadata(false, null));

        public bool DrawFPS
        {
            get { return (bool)GetValue(DrawFPSProperty); }
            set { SetValue(DrawFPSProperty, value); }
        }

        private static readonly DependencyProperty IntProperty =
            DependencyProperty.Register("IntProp", typeof(int), typeof(SceneView),
            new PropertyMetadata(0, null));

        public int IntProp
        {
            get { return (int)GetValue(IntProperty); }
            set { SetValue(IntProperty, value); }
        }

        private static readonly DependencyProperty DoubleProperty =
            DependencyProperty.Register("DoubleProp", typeof(double), typeof(SceneView),
            new PropertyMetadata(0.0, null));

        public double DoubleProp
        {
            get { return (int)GetValue(DoubleProperty); }
            set { SetValue(DoubleProperty, value); }
        }

        #endregion
    }
}
