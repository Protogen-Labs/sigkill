using Godot;
using static Settings;

public class Player2D: IPlayer {
    Player player;

    public Player2D(Player super) {
        player = super;
    }

	public void handleMovement(float delta) {
		//TODO: Impliment movement physics for 2d
	}

    public void updateCameraRotation() {
        
    }
}