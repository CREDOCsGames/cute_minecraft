using UnityEngine;

namespace PlatformGame.Character
{
    public class LifeTime : MonoBehaviour
    {
        public Vector2 mLifeRange;
        float mLife;

        void Awake()
        {
            Debug.Assert(mLifeRange.x <= mLifeRange.y);
            mLife = Random.Range(mLifeRange.x, mLifeRange.y);
        }

        void Update()
        {
            mLife -= Time.deltaTime;
            if (mLife <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}