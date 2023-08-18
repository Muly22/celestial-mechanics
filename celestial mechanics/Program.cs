using SFML.Graphics;
using SFML.Graphics.Glsl;
using SFML.System;
using SFML.Window;
using System.Diagnostics;
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
        private float Density = 1f;
        private float speed_time_baby = 1;

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
            double estimatedTime = 0;
            while (window.IsOpen)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                window.DispatchEvents();
                window.Clear();
                DrawPlanets();
                if (drawGhost)
                {
                    window.Draw(GhostPlanet);
                }
                window.Display();
                Simulation(estimatedTime);
                stopwatch.Stop();
                estimatedTime = stopwatch.Elapsed.TotalNanoseconds;
            }
        }

        private void Simulation(double timeN)
        {
            for (int i = 0; i < PlanetList.Count; i++)
            {
                Planet planet1 = PlanetList[i];
                planet1.acceleration = new Vector2f(0f, 0f);
                for (int j = 0; j < PlanetList.Count; j++)
                {
                    if (i == j)
                        continue;
                    Planet planet2 = PlanetList[j];
                    float distance = Distance(planet1, planet2);
                    float Aot = planet2.mass / (distance * distance);
                    if (CirclevsCircleOptimized(planet1, planet2))
                    {
                        planet1.acceleration -= new Vector2f(Aot * ((planet2.posGlobal.X - planet1.posGlobal.X) / distance), Aot * ((planet2.posGlobal.Y - planet1.posGlobal.Y) / distance));
                    }
                    else
                    {
                        planet1.acceleration += new Vector2f(Aot * ((planet2.posGlobal.X - planet1.posGlobal.X) / distance), Aot * ((planet2.posGlobal.Y - planet1.posGlobal.Y) / distance));
                    }
                }
                planet1.Speed += planet1.acceleration * (float)timeN / 1000000000 * speed_time_baby;
                planet1.posGlobal += planet1.Speed * (float)timeN / 1000000000 * speed_time_baby;
            }
        }
        private bool CirclevsCircleOptimized(Planet a, Planet b)
        {
            Vector2f v = b.posGlobal - a.posGlobal;
            float r = a.radius + b.radius;
            r *= r;
            return r > v.X * v.X + v.Y * v.Y;
        }
        private float Distance(Planet planet1, Planet planet2)
        {
            Vector2f vector = planet2.posGlobal - planet1.posGlobal;
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }
        private void DrawPlanets()
        {
            for (int i = 0; i < PlanetList.Count; i++)
            {
                Planet planet = PlanetList[i];
                Vector3f vector = new Vector3f(planet.posGlobal.X, planet.posGlobal.Y, 0) - CameraPos;
                float radius = planet.radius / Math.Abs(CameraPos.Z);
                Planet.Position = new Vector2f(vector.X * 1 / vector.Z, vector.Y * 1 / vector.Z) - new Vector2f(radius, radius);
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
                    PlanetList.Add(new Planet(GhostRadius * Math.Abs(CameraPos.Z), GhostPlanet.Position * Math.Abs(CameraPos.Z) + new Vector2f(CameraPos.X, CameraPos.Y) + new Vector2f(GhostRadius, GhostRadius), new Color((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)), Density));
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
                case Keyboard.Key.Num1:
                    Density = 1;
                    break;
                case Keyboard.Key.Num2:
                    Density = 2;
                    break;
                case Keyboard.Key.Num3:
                    Density = 5;
                    break;
                case Keyboard.Key.Num4:
                    Density = 10;
                    break;
                case Keyboard.Key.Num5:
                    Density = 50;
                    break;
                case Keyboard.Key.Num6:
                    Density = 100;
                    break;
                case Keyboard.Key.Num7:
                    Density = 500;
                    break;
                case Keyboard.Key.Num8:
                    Density = 1000;
                    break;
                case Keyboard.Key.Num9:
                    Density = 100000;
                    break;
                case Keyboard.Key.Escape:
                    window.Close();
                    break;
                case Keyboard.Key.Left:
                    if (speed_time_baby <= 1)
                    {
                        speed_time_baby -= 0.1f;
                        if (speed_time_baby <=0 )
                        {
                            speed_time_baby = 0f;
                        }
                    }
                    else
                        speed_time_baby--;
                    break;
                case Keyboard.Key.Right:
                    if (speed_time_baby <= 1)
                        speed_time_baby += 0.1f;
                    else
                        speed_time_baby++;
                    break;
            }
        }
    }

    internal class Planet
    {
        public Vector2f Speed { get; set; }
        public Vector2f acceleration { get; set; }
        public float radius { get; private set; }
        public float mass { get; private set; }
        public float density { get; private set; }
        public Vector2f posGlobal { get; set; }
        public Color color { get; private set; }
        public Planet(float _radius, Vector2f _posGlobal, Color _color, float _density)
        {
            
            radius = _radius;
            posGlobal = _posGlobal;
            color = _color;
            density = _density;
            mass = (4f / 3f) * (float)Math.PI * radius * radius * radius * density;
            Speed = new Vector2f(0, 10000000/mass/density);
            Console.WriteLine(mass);
        }
    }
}