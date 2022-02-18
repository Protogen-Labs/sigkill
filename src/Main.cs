using Godot;
using static Settings;

public class Main: Node {
	public override void _Ready() {
		
	}

	public override void _Process(float delta) {
		if (debug && Input.IsActionPressed("ui_cancel"))
			GetTree().Quit();
	}
}
