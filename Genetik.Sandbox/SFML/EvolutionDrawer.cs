using System.Drawing;
using Genetik.Core;
using Genetik.Sandbox.Logic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Color = SFML.Graphics.Color;

namespace Genetik.Sandbox.SFML;

public class EvolutionDrawer
{
    private RenderWindow _window = null!;
    private Field _field;
    private IEvolutionProcess<Vec2> _evolution;
    private IEnumerable<CircleShape> _circles;

    private readonly uint WindowWidth;

    public EvolutionDrawer(IEvolutionProcess<Vec2> evolution, Field field, uint windowWidth)
    {
        _evolution = evolution;
        _field = field;
        WindowWidth = windowWidth;
        InitCircles();
    }

    public void Start()
    {
        _window = new RenderWindow(GetVideoMode(_field), "Genetik", Styles.Close);
        _window.SetVisible(true);
        _window.SetTitle("Genetik [Generation 0]");
        int generationNumber = 0;

        // _window.SetVerticalSyncEnabled(true);

        _window.Closed += OnClosed;
        
        while (_window.IsOpen)
        {
            _evolution.NextGeneration();
            _window.SetTitle($"Genetik [Generation {generationNumber++}]");
            
            _window.DispatchEvents();
            // _window.Clear(Color.White);
            _window.Clear();
            DrawCircles();
            DrawPath(_evolution.CurrGeneration[0].Genes, Color.White);
            DrawPath(_evolution.BestGenome.Genes, Color.Green);
            _window.Display();
        }
    }

    private void OnClosed(object? sender, EventArgs args)
    {
        _window.Close();
    }
    
    private VideoMode GetVideoMode(Field field)
    {
        uint windowHeight = (uint)(WindowWidth * field.Height / field.Width);
        return new VideoMode(WindowWidth, windowHeight);
    }

    private void InitCircles()
    {
        _circles = _field.Circles.Select(ToCircleShape);
    }

    private CircleShape ToCircleShape(Circle circle)
    {
        var shape = new CircleShape(GetRadius(circle.Radius));
        shape.Position = ToWindowCoors(circle.Center) - new Vector2f(shape.Radius, shape.Radius);
        shape.FillColor = Color.Red;
        return shape;
    }
    
    private void DrawCircles()
    {
        foreach (var circle in _circles)
        {
            _window.Draw(circle);
        }
    }

    private void DrawPath(IReadOnlyList<Vec2> steps, Color color)
    {
        var vertices = new Vertex[steps.Count * 2];
        int pos = 0;
        vertices[pos++] = new Vertex(ToWindowCoors(_field.Start)) { Color = color };
        
        Vec2 curr = _field.Start;
        for (int i = 0; i < steps.Count; i++)
        {
            curr += steps[i];
            vertices[pos++] = new Vertex(ToWindowCoors(curr)) { Color = color };
            if (pos < vertices.Length)
                vertices[pos++] = new Vertex(ToWindowCoors(curr)) { Color = color };
        }

        _window.Draw(vertices, PrimitiveType.Lines);
    }

    private float GetRadius(double radius)
    {
        return (float)(radius / _field.Width * WindowWidth);
    }

    private Vector2f ToWindowCoors(Vec2 coors)
    {
        return new Vector2f(
            (float)(coors.X / _field.Width * WindowWidth), 
            (float)(coors.Y /_field.Width * WindowWidth));
    }
}