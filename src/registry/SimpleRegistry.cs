using System.Collections.Generic;

public class SimpleRegistry<T>: IRegistry<T> {
    private Dictionary<Identifier, T> registry;

    public void register(Identifier id, T item) {
        registry[id] = item;
    }
    public T get(Identifier id) {
        return registry[id];
    }
}