using Godot;
using static Settings;

public class Player: KinematicBody {
	private IPlayer _2d;
	private IPlayer _3d;
	public IPlayer currMode {
		get {
			return is2d? _2d: _3d;
		}
	}

	public IWeapon heldWeapon;
	public float minHeadAngle = Mathf.Deg2Rad(-45);
	public float maxHeadAngle = Mathf.Deg2Rad(45);
	public NodePath headPath = "Head";
	public Spatial head;
	public NodePath cameraPath = "camera";
	public Camera camera;
	public Vector2 mouseAxis = new Vector2();
	public Vector3 velocity = new Vector3();
	public Vector3 direction = new Vector3();
	public Vector2 moveAxis = new Vector2();
	public Vector3 snap = new Vector3();
	public bool sprinting = false;
	public bool sprintEnabled = true;
	public float maxFloorAngle = Mathf.Deg2Rad(46);
	public bool jumpInput = false;
	public bool sprintInput = false;

	public override void _Ready() {
		_2d = new Player2D(this);
		_3d = new Player3D(this);
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
		currMode.handleMovement(delta);
	}

	public override void _Input(InputEvent input) {
		if (input is InputEventMouseMotion mouseMotion) {
			mouseAxis = mouseMotion.Relative;
			currMode.updateCameraRotation();
		}
	}
}
