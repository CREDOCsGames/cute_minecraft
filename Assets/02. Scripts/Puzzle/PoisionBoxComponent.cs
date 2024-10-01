using Flow;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Puzzle
{
    public interface IColorPiece
    {
        public Color Color { get; set; }
        public List<MeshRenderer> Renderers { get; }
    }

    public class PoisionBoxComponent : HitBoxComponent
    {
        [SerializeField] Color mColorA;
        [SerializeField] Color mColorB;
        BoxCollider[] mColliders;
        readonly Timer mTimer = new();

        void Awake()
        {
            AddHitEvent(OnHit);
            mColliders = GetComponents<BoxCollider>();
            for (var i = 3; i < mColliders.Length; i++)
            {
                Destroy(mColliders[i]);
            }

            mTimer.SetTimeout(HitDelay);
            mTimer.OnTimeoutEvent += (t) => gameObject.SetActive(false);
        }

        void Update()
        {
            mTimer.Tick();
            if (IsDelay)
            {
                IsAttacker = false;
            }
        }

        void OnEnable()
        {
            mColliders[0].enabled = true;
            mColliders[1].enabled = false;
            mColliders[2].enabled = false;
            IsAttacker = false;
            transform.GetChild(0).gameObject.SetActive(true);
        }

        void OnHit(HitBoxCollision collision)
        {
            if (!IsAttacker)
            {
                IsAttacker = true;
                mColliders[0].enabled = false;
                mColliders[1].enabled = true;
                mColliders[2].enabled = true;
                transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                OnAttack(collision);
            }
        }

        void OnAttack(HitBoxCollision collision)
        {
            var attacked = collision.Victim.GetComponent<IColorPiece>();
            if (attacked == null)
            {
                return;
            }

            var attacker = collision.Attacker;
            if (attacker == null || !attacker.Equals(Actor))
            {
                return;
            }

            var color = FlowerComponent.CompareColor(attacked.Color, mColorA) ? mColorB : mColorA;
            attacked.Color = color;
            mTimer.Start();
        }
    }
}