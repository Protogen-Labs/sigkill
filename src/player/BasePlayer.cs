using Godot;

public class BasePlayer: KinematicBody {
    
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

    
}