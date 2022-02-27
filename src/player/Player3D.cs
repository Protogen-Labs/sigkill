using Godot;
using static Settings;

public class Player3D: IPlayer {
    Player player;
	int currentJumps = 0; 
	int coyote = 0;
	public Camera camera {
		get {
			return player.camera;
		}
	}

    public Player3D(Player super) {
        player = super;
    }
    public void handleMovement(float delta) {
		camera.Current = true;

		updateInput();
		if (player.IsOnFloor()) {
			coyote = 0;
			player.snap = -player.GetFloorNormal() - player.GetFloorVelocity() * delta;
			if (player.velocity.y < 0)
				player.velocity.y = 0;
			currentJumps = 0;
			tryJump();
		} else if (coyote < 7){
			if (player.snap != Vector3.Zero && player.velocity.y != 0)
				player.velocity.y = 0;
			player.snap = Vector3.Zero;
			player.velocity.y -= gravity * delta;
			coyote++;
			tryJump();
		} else {
			if (player.snap != Vector3.Zero && player.velocity.y != 0)
				player.velocity.y = 0;
			player.snap = Vector3.Zero;
			player.velocity.y -= gravity * delta;
			if (currentJumps < maxJumps) tryJump();
		}

		checkSprint(delta);
		applyAccel(delta);

		player.velocity = player.MoveAndSlideWithSnap(player.velocity, player.snap, Vector3.Up, true, 4, player.maxFloorAngle);
		player.jumpInput = false;
		player.sprintInput = false;
	}

	public void updateInput() {
		player.direction = new Vector3(0,0,0);
		Basis aim = player.GlobalTransform.basis;
		player.direction = -aim.z * player.moveAxis.x;
		player.direction += aim.x * player.moveAxis.y;
		player.direction.y = 0;
		player.direction = player.direction.Normalized();
	}

	public void applyAccel(float delta) {
		Vector3 tempVel = player.velocity;
		float tempAccel;
		Vector3 target = player.direction * (walkSpeed + (player.sprinting? sprintAdd: 0));

		tempVel.y = 0;
		tempAccel = deceleration;
		if (player.direction.Dot(tempVel) > 0)
			tempAccel = acceleration;
		
		if (!player.IsOnFloor())
			tempAccel *= airControl;
		
		tempVel = tempVel.LinearInterpolate(target, tempAccel * delta);

		player.velocity.x = tempVel.x;
		player.velocity.z = tempVel.z;

		if (player.direction.Dot(player.velocity) == 0) {
			float clamp = 0.01f;
			if (Mathf.Abs(player.velocity.x) < clamp)
				player.velocity.x = 0;
			if (Mathf.Abs(player.velocity.z) < clamp)
				player.velocity.z = 0;
		}
	}

	public void tryJump() {
		if (player.jumpInput) {
			player.velocity.y = jumpHeight;
			player.snap = Vector3.Zero;
			currentJumps++;
			coyote = 7;
		}
	}

	public void checkSprint(float delta) {
		if (canSprint()) {
			camera.Fov = Mathf.Lerp(camera.Fov, fov*1.05f, delta*8);
			player.sprinting = true;
			return;
		}
		camera.Fov = Mathf.Lerp(camera.Fov, fov, delta*8);
		player.sprinting = false;
	}

	public bool canSprint() {
		return player.sprintEnabled && player.sprintInput && isMoving();
	}

	public bool isMoving() {
		return Mathf.Abs(player.moveAxis.x) >= 0.5 || Mathf.Abs(player.moveAxis.y) >= 0.5;
	}

	public void updateCameraRotation() {
		float horizontal = -player.mouseAxis.x * (mouseSensitivity / 100);
		float vertical   = -player.mouseAxis.y * (mouseSensitivity / 100);
		player.mouseAxis = new Vector2();
		player.RotateY(Mathf.Deg2Rad(horizontal));
		player.head.RotateX(Mathf.Deg2Rad(vertical));
		if (player.head.Rotation.x < player.minHeadAngle)
			player.head.Rotation = new Vector3(player.minHeadAngle, player.head.Rotation.y, player.head.Rotation.z);
		if (player.head.Rotation.x > player.maxHeadAngle)
			player.head.Rotation = new Vector3(player.maxHeadAngle, player.head.Rotation.y, player.head.Rotation.z);
		player.mouseAxis = new Vector2();
	}
}