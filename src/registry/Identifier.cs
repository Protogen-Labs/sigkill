public class Identifier {
    public string nameSpace;
    public string path;

    public Identifier(string nameSpace, string path) {
        this.nameSpace = nameSpace;
        this.path = path;
    }

    public override string ToString() {
        return nameSpace + ":" + path;
    }
}