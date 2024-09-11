using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Contents
{
    public class BetteryUser : MonoBehaviour
    {
        [SerializeField] float mEnergyTickRate;
        [SerializeField] UnityEvent mEnergyTickEvent;
        [SerializeField] UnityEvent mDischargeEvent;
        [SerializeField] UnityEvent mChargeEvent;
        Bettery mBettery;

        void UseBettery()
        {
            mBettery.Amount -= mEnergyTickRate;
            if (mBettery.IsEmpty)
            {
                return;
            }
            mEnergyTickEvent.Invoke();
        }

        IEnumerator OnTick()
        {
            var delay = new WaitForSeconds(0.1f);
            while (true)
            {
                UseBettery();
                yield return delay;
            }
        }

        void Awake()
        {
            mBettery = GetComponent<Bettery>();
            mBettery.OnCharge.AddListener((t) => { if (mBettery.IsEmpty) mChargeEvent.Invoke(); });
            mBettery.OnDischarge.AddListener(() => mDischargeEvent.Invoke());
        }

        void OnEnable()
        {
            StartCoroutine(OnTick());
        }

    }
}
