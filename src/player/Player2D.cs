using Godot;
using static Settings;

public class Player2D: IPlayer {
    Player player;
    Camera camera;

    public Player2D(Player super) {
        player = super;
        camera = player.GetParent<Main>().GetNode<Camera>("cam2d");
    }

    public void handleMovement(float delta) {
        camera.Current = true;
        updateInput();
        if (player.IsOnFloor()) {
			player.snap = -player.GetFloorNormal() - player.GetFloorVelocity() * delta;
			if (player.velocity.y < 0)
				player.velocity.y = 0;
            tryJump();
        } else {
			if (player.snap != Vector3.Zero && player.velocity.y != 0)
				player.velocity.y = 0;
			player.snap = Vector3.Zero;
			player.velocity.y -= gravity * delta;
        }

        checkSprint(delta);
        applyAccel(delta);

        player.velocity = player.MoveAndSlideWithSnap(player.velocity, player.snap, Vector3.Up, true, 4, player.maxFloorAngle);
		player.jumpInput = false;
		player.sprintInput = false;
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

		if (player.direction.Dot(player.velocity) == 0) {
			float clamp = 0.01f;
			if (Mathf.Abs(player.velocity.x) < clamp)
				player.velocity.x = 0;
		}
        player.velocity.z = 0;
	}
    
	public void checkSprint(float delta) {
		if (canSprint()) {
			player.camera.Fov = Mathf.Lerp(player.camera.Fov, fov*1.05f, delta*8);
			player.sprinting = true;
			return;
		}
		player.camera.Fov = Mathf.Lerp(player.camera.Fov, fov, delta*8);
		player.sprinting = false;
	}

	public bool canSprint() {
		return player.sprintEnabled && player.sprintInput && isMoving();
	}

	public bool isMoving() {
		return Mathf.Abs(player.moveAxis.x) >= 0.5 || Mathf.Abs(player.moveAxis.y) >= 0.5;
	}

    public void tryJump() {
		if (player.jumpInput) {
			player.velocity.y = jumpHeight;
			player.snap = Vector3.Zero;
		}
    }

    public void updateInput() {
		player.direction = new Vector3(0,0,0);
		player.direction.x = player.moveAxis.y;
		player.direction.y = 0;
		player.direction = player.direction.Normalized();
    }
    public void updateCameraRotation() {}
}