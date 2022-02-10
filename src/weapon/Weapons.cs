public class Weapons {

    public static IWeapon pistol = new PistolWeapon();
    public static void register() {
        Registry.WEAPON.register(new Identifier("pistol"), pistol);
    }
}