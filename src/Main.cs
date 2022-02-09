using Godot;
using System;
using static Settings;

public class Main : Node {
	public override void _Ready() {
		Console.WriteLine("Hellstrafe Initialized.");
	}

	public override void _Process(float delta) {
		if (debug && Input.IsActionPressed("ui_cancel"))
			this.GetTree().Quit();
	}
}
