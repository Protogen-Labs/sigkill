using Godot;

public interface IWeapon {
    float getDamage();
    float getSpead();
    float getLoudness();
    int getAmount();

    void shoot(Node other);
}