using Godot;
using System;

public class PalmTree : StaticBody2D
{
    public override void _Ready()
    {
        Random rand = new Random();

        // -Random coconuts
        if (rand.NextDouble() >= 0.5)
        {
            TileMap PalmTreeCoconutSprite = GetNode<TileMap>("PalmTreeCoconut");
            PalmTreeCoconutSprite.Visible = true;
        }   
    }
}
