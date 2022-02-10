using Godot;
using static Settings;

public class Player3D: Player2D {
    public void handleMovement_3D(float delta) {
		updateInput_3D();
		if (IsOnFloor()) {
			snap = -GetFloorNormal() - GetFloorVelocity() * delta;
			if (velocity.y < 0)
				velocity.y = 0;
			tryJump_3D();
		} else {
			if (snap != Vector3.Zero && velocity.y != 0)
				velocity.y = 0;
			snap = Vector3.Zero;
			velocity.y -= gravity * delta;
		}

		checkSprint_3D(delta);
		applyAccel_3D(delta);

		velocity = MoveAndSlideWithSnap(velocity, snap, Vector3.Up, true, 4, maxFloorAngle);
		jumpInput = false;
		sprintInput = false;
	}

	public void updateInput_3D() {
		direction = new Vector3(0,0,0);
		Basis aim = GlobalTransform.basis;
		direction = -aim.z * moveAxis.x;
		direction += aim.x * moveAxis.y;
		direction.y = 0;
		direction = direction.Normalized();
	}

	public void applyAccel_3D(float delta) {
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

	public void tryJump_3D() {
		if (jumpInput) {
			velocity.y = jumpHeight;
			snap = Vector3.Zero;
		}
	}

	public void checkSprint_3D(float delta) {
		if (canSprint_3D()) {
			camera.Fov = Mathf.Lerp(camera.Fov, fov*1.05f, delta*8);
			sprinting = true;
			return;
		}
		camera.Fov = Mathf.Lerp(camera.Fov, fov, delta*8);
		sprinting = false;
	}

	public bool canSprint_3D() {
		return sprintEnabled && sprintInput && isMoving_3D();
	}

	public bool isMoving_3D() {
		return Mathf.Abs(moveAxis.x) >= 0.5 || Mathf.Abs(moveAxis.y) >= 0.5;
	}

	public void updateCameraRotation_3D() {
		float horizontal = -mouseAxis.x * (mouseSensitivity / 100);
		float vertical   = -mouseAxis.y * (mouseSensitivity / 100);
		mouseAxis = new Vector2();
		RotateY(Mathf.Deg2Rad(horizontal));
		head.RotateX(Mathf.Deg2Rad(vertical));
		if (head.Rotation.x < minHeadAngle)
			head.Rotation = new Vector3(minHeadAngle, head.Rotation.y, head.Rotation.z);
		if (head.Rotation.x > maxHeadAngle)
			head.Rotation = new Vector3(maxHeadAngle, head.Rotation.y, head.Rotation.z);
		mouseAxis = new Vector2();
	}
}