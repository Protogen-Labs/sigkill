using Godot;
using System;

public class Preload: Node {
    public override void _Ready() {
        Console.WriteLine("Initializing Hellstrafe...");
        Weapons.register();
		Console.WriteLine("Hellstrafe Initialized.");
    }
}