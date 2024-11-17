using UnityEngine;

namespace Puzzle
{
    public interface ICopyable<T>
    {
        public T Copy();
    }

    public class Instantiator<T> : ICopyable<T> where T : MonoBehaviour
    {
        private readonly T _origin;
        public Instantiator(T origin)
        {
            this._origin = origin;
        }

        public T Copy()
        {
            return GameObject.Instantiate(_origin);
        }
    }

}

