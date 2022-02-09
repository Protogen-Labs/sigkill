using Godot;
using System;
using static Settings;

public class Player: KinematicBody {
	public IWeapon heldWeapon;
	public  NodePath headPath = "Head";
	public Spatial head;
	public  NodePath cameraPath = "camera";
	public Camera camera;
	public Vector2 mouseAxis = new Vector2();
	public Vector3 velocity = new Vector3();
	public Vector3 direction = new Vector3();
	public Vector2 moveAxis = new Vector2();
	public Vector3 snap = new Vector3();
	public bool sprinting = false;
	public bool sprintEnabled = true;
	public float maxFloorAngle = Mathf.Deg2Rad(46);
	public float speed = 0;
	public bool jumpInput = false;
	public bool sprintInput = false;


	public override void _Ready() {
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
		
	}

	public override void _Input(InputEvent input) {
		if (input is InputEventMouseMotion mouseMotion) {
			mouseAxis = mouseMotion.Relative;
			cameraRotation();
		}
	}

	public void walk(float delta) {

	}

	public void cameraRotation() {
		if (mouseAxis.Length() > 0) {
			float horizontal = -mouseAxis.x * (mouseSensitivity / 100);
			float vertical   = -mouseAxis.y * (mouseSensitivity / 100);
			mouseAxis = new Vector2();
			RotateY(Mathf.Deg2Rad(horizontal));
			head.RotateX(Mathf.Deg2Rad(vertical));
		}
	}
}
