namespace Tabler
{
    public class Singleton<T> where T : new()
    {
        private static readonly Lazy<T> lazy = new Lazy<T>(() => new T());
        public static T Instance { get => lazy.Value; }
    }
}
