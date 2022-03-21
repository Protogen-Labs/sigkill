using Godot;
using System;

public class MovementGrid : Spatial {
	public override void _Ready() {
		
	}
 	public override void _Process(float delta) {
		 
 	}

	public static Vector3 getClosestPoint(Vector3 pos) {
		float x = applyRounding(pos.x);
		float y = applyRounding(pos.y);
		float z = applyRounding(pos.z);

		return new Vector3(x,y,z);
	}

	public static Vector2 getClosestPoint(Vector2 pos) {
		float x = applyRounding(pos.x);
		float y = applyRounding(pos.y);
		
		return new Vector2(x,y);
	}

	public static float applyRounding(float old) {
		return Mathf.Round(old/25)*25;
	}
}
