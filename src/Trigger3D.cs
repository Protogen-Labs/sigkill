using Godot;
using System;

public class Trigger3D : Area {
	public override void _Ready() {
		
	}

	public override void _PhysicsProcess(float delta) {
		foreach (PhysicsBody e in GetOverlappingBodies())
			if (e is Player) 
				Settings.is2d = false;
	}
}
