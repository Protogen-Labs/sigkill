using Godot;
using static Settings;

public class Player: KinematicBody {
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
			handleMovement2d(delta);
		else handleMovement3d(delta);
	}

	public override void _Input(InputEvent input) {
		if (input is InputEventMouseMotion mouseMotion) {
			mouseAxis = mouseMotion.Relative;
			if (!is2d)
				updateCameraRotation3d();
		}
	}

	public void handleMovement2d(float delta) {
		//TODO: Impliment movement physics for 2d
	}

	public void handleMovement3d(float delta) {
		updateInput3d();
		if (IsOnFloor()) {
			snap = -GetFloorNormal() - GetFloorVelocity() * delta;
			if (velocity.y < 0)
				velocity.y = 0;
			tryJump();
		} else {
			if (snap != Vector3.Zero && velocity.y != 0)
				velocity.y = 0;
			snap = Vector3.Zero;
			velocity.y -= gravity * delta;
		}

		checkSprint(delta);
		applyAccel(delta);

		velocity = MoveAndSlideWithSnap(velocity, snap, Vector3.Up, true, 4, maxFloorAngle);
		jumpInput = false;
		sprintInput = false;
	}

	public void updateInput3d() {
		direction = new Vector3(0,0,0);
		Basis aim = GlobalTransform.basis;
		direction = -aim.z * moveAxis.x;
		direction += aim.x * moveAxis.y;
		direction.y = 0;
		direction = direction.Normalized();
	}

	public void applyAccel(float delta) {
		Vector3 tempVel = velocity;
		float tempAccel;
		Vector3 target = direction * (walkSpeed + (sprinting? sprintAdd: 0));

		tempVel.y = 0;
		tempAccel = deceleration;
		if (direction.Dot(tempVel) > 0)
			tempAccel = acceleration;
		
		if (!IsOnFloor())
			tempAccel *= airControl;
		
		tempVel = tempVel.LinearInterpolate(target, tempAccel * delta);

		velocity.x = tempVel.x;
		velocity.z = tempVel.z;

		if (direction.Dot(velocity) == 0) {
			float clamp = 0.01f;
			if (Mathf.Abs(velocity.x) < clamp)
				velocity.x = 0;
			if (Mathf.Abs(velocity.z) < clamp)
				velocity.z = 0;
		}
	}

	public void tryJump() {
		if (jumpInput) {
			velocity.y = jumpHeight;
			snap = Vector3.Zero;
		}
	}

	public void checkSprint(float delta) {
		if (canSprint()) {
			camera.Fov = Mathf.Lerp(camera.Fov, fov*1.05f, delta*8);
			sprinting = true;
			return;
		}
		camera.Fov = Mathf.Lerp(camera.Fov, fov, delta*8);
		sprinting = false;
	}

	public bool canSprint() {
		return sprintEnabled && sprintInput && isMoving();
	}

	public bool isMoving() {
		return Mathf.Abs(moveAxis.x) >= 0.5 || Mathf.Abs(moveAxis.y) >= 0.5;
	}

	public void updateCameraRotation3d() {
		if (mouseAxis.Length() > 0) {
			float horizontal = -mouseAxis.x * (mouseSensitivity / 100);
			float vertical   = -mouseAxis.y * (mouseSensitivity / 100);
			mouseAxis = new Vector2();
			RotateY(Mathf.Deg2Rad(horizontal));
			head.RotateX(Mathf.Deg2Rad(vertical));
			if (head.Rotation.x < minHeadAngle)
				head.Rotation = new Vector3(minHeadAngle, head.Rotation.y, head.Rotation.z);
			if (head.Rotation.x > maxHeadAngle)
				head.Rotation = new Vector3(maxHeadAngle, head.Rotation.y, head.Rotation.z);
		}
	}
}
