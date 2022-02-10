using Godot;

public class PistolWeapon: IWeapon {
    public PistolWeapon() {

    }

    public float getDamage() {
        return 1;
    }
    public float getSpead() {
        return 0;
    }
    public float getLoudness() {
        return 1;
    }
    public int getAmount() {
        return 0;
    }

    public void shoot(Node other) {

    }
}