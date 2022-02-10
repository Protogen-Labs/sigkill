using System.Collections.Generic;

public class SimpleRegistry<T>: IRegistry<T> {
    private Dictionary<string, T> registry = new Dictionary<string, T>();

    public void register(Identifier id, T item) {
        registry[id.ToString()] = item;
    }
    public T get(Identifier id) {
        return registry[id.ToString()];
    }
}