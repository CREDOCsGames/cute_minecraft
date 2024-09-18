using PlatformGame.Manager;
using PlatformGame.Util;
using System.Collections;
using UnityEngine;

namespace PlatformGame.Contents
{
    public class LanternComponent : MonoBehaviour
    {
        [SerializeField] float mRange;
        [SerializeField, Range(1, 100)] float mDistributionAmount;
        [SerializeField, Range(0.1f, 100f)] float mDistributionInterval;
        Bettery mBettery;
        int mCount;
        public int Count
        {
            get => mCount;
            set
            {
                mCount = value;
                if (ClearCount == mCount)
                {
                    GameManager.PuzzleArea.OnClear();
                }
                if (ClearCount < 2)
                {
                    return;
                }
                var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.SetParent(transform);
                sphere.transform.SetLocalPositionAndRotation(Vector3.down * (2f + mCount), Quaternion.identity);
            }
        }
        [SerializeField] int ClearCount;

        public void SetGameManger()
        {
            GameManager.Lantern = this;
        }

        public void ResetGameManager()
        {
            GameManager.Lantern = null;
        }

        void DistributeElectricity(Bettery bettery)
        {
            if (bettery.Type.HasFlag(BetteryType.MinusPole))
            {
                return;
            }

            var before = mBettery.Amount;
            mBettery.Amount -= mDistributionAmount;
            bettery.Amount += before - mBettery.Amount;
        }

        void DistributeElectricityInRange()
        {
            foreach (var bettery in InstancesMonobehaviour<Bettery>.Instances)
            {
                if (mRange < Vector3.Distance(bettery.transform.position, transform.position))
                {
                    continue;
                }

                DistributeElectricity(bettery);
            }
        }

        IEnumerator Tick()
        {
            var delay = new WaitForSeconds(mDistributionInterval);
            while (true)
            {
                mBettery.FullChargeBettery();
                DistributeElectricityInRange();
                yield return delay;
            }
        }

        void Awake()
        {
            mBettery = GetComponent<Bettery>();
        }

        void OnEnable()
        {
            StartCoroutine(Tick());
        }

        void OnDisable()
        {
            StopAllCoroutines();
        }
#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, mRange);
        }
#endif

    }
}
