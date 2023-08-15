using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Numerics;

namespace celestial_mechanics
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            World world = new World(new VideoMode(1000, 1000), "");
        }
    }

    internal class World : RenderWindow
    {
        private RenderWindow window;
        private uint Width;
        private uint Height;
        private Vector3f CameraPos;
        private float scale;
        private Vector2f MousePresPos;
        private Vector2f MousePos;
        private bool drawGhost;
        private float GhostRadius;
        private CircleShape GhostPlanet;
        private CircleShape Planet;
        private bool CameraPosChange;
        private List<Planet> PlanetList;
        public World(VideoMode mode, string title) : base(mode, title, Styles.Close)
        {
            window = this;
            Width = window.Size.X; Height = window.Size.Y;
            MousePresPos = new Vector2f();
            MousePos = new Vector2f();
            GhostPlanet = new CircleShape();
            Planet = new CircleShape();
            CameraPos = new Vector3f(0, 0, -1f);
            PlanetList = new List<Planet>();
            Render();
        }

        private void Render()
        {
            window.KeyPressed += Window_KeyPressed;
            window.Closed += Window_Closed;
            window.MouseButtonPressed += Window_MouseButtonPressed;
            window.MouseButtonReleased += Window_MouseButtonReleased;
            window.MouseLeft += Window_MouseLeft;
            window.MouseWheelScrolled += Window_MouseWheelScrolled;
            window.MouseMoved += Window_MouseMoved;
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();
                DrawPlanets();
                if (drawGhost)
                {
                    window.Draw(GhostPlanet);
                }
                window.Display();
            }
        }

        private void DrawPlanets()
        {
            for (int i = 0; i < PlanetList.Count; i++)
            {
                Planet planet = PlanetList[i];
                Vector3f vector = new Vector3f(planet.posGlobal.X, planet.posGlobal.Y, 0) - CameraPos;
                float radius = planet.radius / Math.Abs(CameraPos.Z);
                Planet.Position = new Vector2f(vector.X * 1 / vector.Z, vector.Y * 1 / vector.Z);
                Planet.Radius = radius;
                Planet.FillColor = planet.color;
                window.Draw(Planet);
            }
        }
        private void Window_MouseMoved(object? sender, MouseMoveEventArgs e)
        {
            MousePos.X = e.X;
            MousePos.Y = e.Y;
            Vector2f vector = MousePos - MousePresPos;
            if (CameraPosChange)
            {
                CameraPos += new Vector3f((-vector.X) * 0.01f, (-vector.Y) * 0.01f, 0);
            }
            if (drawGhost)
            {
                GhostRadius = (float)Math.Sqrt(((int)vector.X * ((int)vector.X) + ((int)vector.Y * (int)vector.Y)));
                GhostPlanet.Radius = GhostRadius;
                GhostPlanet.OutlineThickness = -GhostRadius / 8;
                GhostPlanet.Position = MousePresPos - new Vector2f(GhostRadius, GhostRadius);
                GhostPlanet.FillColor = new Color(100, 100, 250, 100);
            }
        }

        private void Window_MouseWheelScrolled(object? sender, MouseWheelScrollEventArgs e)
        {
            switch (e.Wheel)
            {
                case Mouse.Wheel.VerticalWheel:
                    if (e.Delta < 0)
                    {
                        CameraPos.Z--;
                        CameraPos.X -= Width / 2;
                        CameraPos.Y -= Height / 2;
                    }
                    else
                    {
                        if (CameraPos.Z == -1)
                            return;
                        CameraPos.Z++;
                        CameraPos.X += Width / 2;
                        CameraPos.Y += Height / 2;
                    }
                    break;
            }
        }
        private void Window_MouseLeft(object? sender, EventArgs e)
        {

        }
        private void Window_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case Mouse.Button.Left:
                    drawGhost = false;
                    var random = new Random();
                    PlanetList.Add(new Planet() { radius = GhostRadius * Math.Abs(CameraPos.Z), posGlobal = GhostPlanet.Position * Math.Abs(CameraPos.Z) + new Vector2f(CameraPos.X, CameraPos.Y), color = new Color((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)) });
                    break;
                case Mouse.Button.Right:

                    break;
                case Mouse.Button.Middle:
                    CameraPosChange = false;
                    break;
            }
        }
        private void Window_MouseButtonPressed(object? sender, MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case Mouse.Button.Left:
                    MousePresPos.X = e.X;
                    MousePresPos.Y = e.Y;
                    drawGhost = true;
                    GhostPlanet.Position = MousePresPos;
                    GhostPlanet.Radius = 0;
                    GhostPlanet.OutlineThickness = 0;
                    GhostPlanet.OutlineColor = new Color(50, 50, 250, 100);
                    break;
                case Mouse.Button.Right:
                    break;
                case Mouse.Button.Middle:
                    MousePresPos.X = e.X;
                    MousePresPos.Y = e.Y;
                    CameraPosChange = true;
                    break;
            }
        }
        private void Window_Closed(object? sender, EventArgs e)
        {
            window.Close();
        }
        private void Window_KeyPressed(object? sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.Unknown:
                    break;
                case Keyboard.Key.A:
                    break;
                case Keyboard.Key.B:
                    break;
                case Keyboard.Key.C:
                    break;
                case Keyboard.Key.D:
                    break;
                case Keyboard.Key.E:
                    break;
                case Keyboard.Key.F:
                    break;
                case Keyboard.Key.G:
                    break;
                case Keyboard.Key.H:
                    break;
                case Keyboard.Key.I:
                    break;
                case Keyboard.Key.J:
                    break;
                case Keyboard.Key.K:
                    break;
                case Keyboard.Key.L:
                    break;
                case Keyboard.Key.M:
                    break;
                case Keyboard.Key.N:
                    break;
                case Keyboard.Key.O:
                    break;
                case Keyboard.Key.P:
                    break;
                case Keyboard.Key.Q:
                    break;
                case Keyboard.Key.R:
                    break;
                case Keyboard.Key.S:
                    break;
                case Keyboard.Key.T:
                    break;
                case Keyboard.Key.U:
                    break;
                case Keyboard.Key.V:
                    break;
                case Keyboard.Key.W:
                    break;
                case Keyboard.Key.X:
                    break;
                case Keyboard.Key.Y:
                    break;
                case Keyboard.Key.Z:
                    break;
                case Keyboard.Key.Num0:
                    break;
                case Keyboard.Key.Num1:
                    break;
                case Keyboard.Key.Num2:
                    break;
                case Keyboard.Key.Num3:
                    break;
                case Keyboard.Key.Num4:
                    break;
                case Keyboard.Key.Num5:
                    break;
                case Keyboard.Key.Num6:
                    break;
                case Keyboard.Key.Num7:
                    break;
                case Keyboard.Key.Num8:
                    break;
                case Keyboard.Key.Num9:
                    break;
                case Keyboard.Key.Escape:
                    break;
                case Keyboard.Key.LControl:
                    break;
                case Keyboard.Key.LShift:
                    break;
                case Keyboard.Key.LAlt:
                    break;
                case Keyboard.Key.LSystem:
                    break;
                case Keyboard.Key.RControl:
                    break;
                case Keyboard.Key.RShift:
                    break;
                case Keyboard.Key.RAlt:
                    break;
                case Keyboard.Key.RSystem:
                    break;
                case Keyboard.Key.Menu:
                    break;
                case Keyboard.Key.LBracket:
                    break;
                case Keyboard.Key.RBracket:
                    break;
                case Keyboard.Key.Semicolon:
                    break;
                case Keyboard.Key.Comma:
                    break;
                case Keyboard.Key.Period:
                    break;
                case Keyboard.Key.Quote:
                    break;
                case Keyboard.Key.Slash:
                    break;
                case Keyboard.Key.Backslash:
                    break;
                case Keyboard.Key.Tilde:
                    break;
                case Keyboard.Key.Equal:
                    break;
                case Keyboard.Key.Hyphen:
                    break;
                case Keyboard.Key.Space:
                    break;
                case Keyboard.Key.Enter:
                    break;
                case Keyboard.Key.Backspace:
                    break;
                case Keyboard.Key.Tab:
                    break;
                case Keyboard.Key.PageUp:
                    break;
                case Keyboard.Key.PageDown:
                    break;
                case Keyboard.Key.End:
                    break;
                case Keyboard.Key.Home:
                    break;
                case Keyboard.Key.Insert:
                    break;
                case Keyboard.Key.Delete:
                    break;
                case Keyboard.Key.Add:
                    break;
                case Keyboard.Key.Subtract:
                    break;
                case Keyboard.Key.Multiply:
                    break;
                case Keyboard.Key.Divide:
                    break;
                case Keyboard.Key.Left:
                    break;
                case Keyboard.Key.Right:
                    break;
                case Keyboard.Key.Up:
                    break;
                case Keyboard.Key.Down:
                    break;
                case Keyboard.Key.Numpad0:
                    break;
                case Keyboard.Key.Numpad1:
                    break;
                case Keyboard.Key.Numpad2:
                    break;
                case Keyboard.Key.Numpad3:
                    break;
                case Keyboard.Key.Numpad4:
                    break;
                case Keyboard.Key.Numpad5:
                    break;
                case Keyboard.Key.Numpad6:
                    break;
                case Keyboard.Key.Numpad7:
                    break;
                case Keyboard.Key.Numpad8:
                    break;
                case Keyboard.Key.Numpad9:
                    break;
                case Keyboard.Key.F1:
                    break;
                case Keyboard.Key.F2:
                    break;
                case Keyboard.Key.F3:
                    break;
                case Keyboard.Key.F4:
                    break;
                case Keyboard.Key.F5:
                    break;
                case Keyboard.Key.F6:
                    break;
                case Keyboard.Key.F7:
                    break;
                case Keyboard.Key.F8:
                    break;
                case Keyboard.Key.F9:
                    break;
                case Keyboard.Key.F10:
                    break;
                case Keyboard.Key.F11:
                    break;
                case Keyboard.Key.F12:
                    break;
                case Keyboard.Key.F13:
                    break;
                case Keyboard.Key.F14:
                    break;
                case Keyboard.Key.F15:
                    break;
                case Keyboard.Key.Pause:
                    break;
                case Keyboard.Key.KeyCount:
                    break;
                default:
                    break;
            }
        }
    }

    internal class Planet
    {
        public float radius = 1;
        private double mass;
        private double density;
        public Vector2f posGlobal = new Vector2f();
        public Color color;
    }
}