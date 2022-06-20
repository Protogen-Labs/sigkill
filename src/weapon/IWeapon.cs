using Godot;

public abstract class IWeapon : Node {
    public abstract float getDamage();
    public abstract float getSpread();
    public abstract float getLoudness();
    public abstract int getAmount();

    public abstract void shoot(Node other);
}