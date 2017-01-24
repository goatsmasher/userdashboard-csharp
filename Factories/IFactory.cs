namespace dashboard {
    public interface IFactory<T> where T : BaseEntity {
        void Add(T item);
    }
}