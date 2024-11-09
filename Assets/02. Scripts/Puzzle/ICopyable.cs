using UnityEngine;

namespace Puzzle
{
    public interface ICopyable<T>
    {
        public T Copy();
    }

    public class Instantiator<T> : ICopyable<T> where T : MonoBehaviour
    {
        readonly T mOrigin;
        public Instantiator(T origin)
        {
            this.mOrigin = origin;
        }

        public T Copy()
        {
            return GameObject.Instantiate(mOrigin);
        }
    }

}

