using System.Collections.Generic;

namespace NextOne
{
    [System.Serializable]
    public class Lister<T>
    {
        public List<T> list = new List<T>();

        public int Count()
        {
            return list.Count;
        }

        public T ElementAt(int _index)
        {
            if (Count() == 0 || _index < 0)
                return default(T);

            return _index > Count() ? ElementAt(0) : list[_index];
        }
    }
}