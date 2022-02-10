public interface IRegistry<T> {
    void register(Identifier id, T item);
    T get(Identifier id);
}