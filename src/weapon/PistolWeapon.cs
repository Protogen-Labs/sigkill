using Godot;

public class PistolWeapon: IWeapon {
    public PistolWeapon() {
        
    }

    public override float getDamage() {
        return 1;
    }
    public override float getSpead() {
        return 0;
    }
    public override float getLoudness() {
        return 1;
    }
    public override int getAmount() {
        return 0;
    }

    public override void shoot(Node other) {

    }
}