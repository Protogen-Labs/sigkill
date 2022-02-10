public interface IEntity {
    float getHealth();
    float getArmor();
    void damage(float amount);
    bool is2dOnly();
    bool is3dOnly();
}