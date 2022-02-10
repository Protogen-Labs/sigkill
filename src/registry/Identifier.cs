public class Identifier {
    public string nameSpace;
    public string path;
    public Identifier(string nameSpace, string path) {
        this.nameSpace = nameSpace.Replace(":","");
        this.path = path.Replace(":","");
    }
    public Identifier(string path) {
        this.nameSpace = "hellstrafe";
        this.path = path.Replace(":","");
    }

    public static Identifier fromString(string id) {
        string[] split = id.Split(':');
        return new Identifier(split[0],split[1]);
    }

    public override string ToString() {
        return nameSpace + ":" + path;
    }

    public override bool Equals(object obj) {
        if (!(obj is Identifier id)) return false;
        return this.nameSpace == id.nameSpace && this.path == id.path;
    }
    public override int GetHashCode() {
        return base.GetHashCode();
    }
}