using Godot;
using static Settings;

public class Player: Player3D {
	public override void _Ready() {
		heldWeapon = Registry.WEAPON.get(new Identifier("pistol"));
		Input.SetMouseMode(Input.MouseMode.Captured);
		head = GetNode<Spatial>(headPath);
		camera = head.GetNode<Camera>(cameraPath);
		camera.Fov = fov;
	}

	public override void _Process(float delta) {
		moveAxis.x = Input.GetActionStrength("move_forward") - Input.GetActionStrength("move_backward");
		moveAxis.y = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");

		if (Input.IsActionJustPressed("move_jump"))
			jumpInput = true;
		if (Input.IsActionPressed("move_sprint"))
			sprintInput = true;
	}

	public override void _PhysicsProcess(float delta) {
		if (is2d) 
			handleMovement_2D(delta);
		else handleMovement_3D(delta);
	}

	public override void _Input(InputEvent input) {
		if (input is InputEventMouseMotion mouseMotion) {
			mouseAxis = mouseMotion.Relative;
			if (!is2d)
				updateCameraRotation_3D();
		}
	}
}
