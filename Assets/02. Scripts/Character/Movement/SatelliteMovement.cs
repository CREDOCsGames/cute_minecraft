using System.Collections;
using UnityEngine;

namespace PlatformGame.Character.Movement
{
    [CreateAssetMenu(menuName = "Custom/MovementAction/Satellite")]
    public sealed class SatelliteMovement : MovementAction
    {
        public float mSpeed = 1f;
        public float mRadius = 5f;

        public override void PlayAction(Rigidbody rigid, MonoBehaviour coroutine)
        {
            var satellite = coroutine.transform;
            coroutine.StartCoroutine(AroundTarget(satellite));
        }

        public override void StopAction(Rigidbody rigid, MonoBehaviour coroutine)
        {
            var owner = rigid.transform.parent;
            var satellite = coroutine.transform;
            coroutine.StopCoroutine(AroundTarget(satellite));
        }

        private IEnumerator AroundTarget(Transform satellite)
        {
            var angle = 0f;
            while (true)
            {
                angle += Time.deltaTime * mSpeed;
                if (360 <= angle)
                {
                    angle = 0;
                }
                else
                {
                    var radius = Mathf.Deg2Rad * (angle);
                    var posX = mRadius * Mathf.Sin(radius);
                    var posY = mRadius * Mathf.Cos(radius);
                    satellite.localPosition = new Vector3(posX, 1, posY);
                }

                yield return null;
            }
        }

    }
}