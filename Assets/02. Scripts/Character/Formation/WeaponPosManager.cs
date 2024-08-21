using UnityEngine;

public class WeaponPosManager : MonoBehaviour
{
    [SerializeField] Rigidbody mModel;
    [SerializeField] int distance = 3;
    Vector3 forward;

    void Update()
    {
        if (mModel.velocity.magnitude < 1f)
        {
            return;
        }

        forward = Vector3.zero;
        if (Mathf.Abs(mModel.velocity.normalized.x) > 0.5)
        {
            forward.x += mModel.velocity.normalized.x < 0 ? -2 * distance : 2 * distance;
        }

        if (Mathf.Abs(mModel.velocity.normalized.z) > 0.5)
        {
            forward.z += mModel.velocity.normalized.z < 0 ? -2 * distance : 2 * distance;
        }

        if (forward == Vector3.zero)
        {
            var dx = mModel.velocity.normalized.x < 0 ? Vector3.left : Vector3.right;
            var dz = mModel.velocity.normalized.z < 0 ? Vector3.back : Vector3.forward;
            forward += Mathf.Abs(mModel.velocity.normalized.x) > Mathf.Abs(mModel.velocity.normalized.z) ? dx * 2 * distance : dz * 2 * distance;
        }
    }

    public void SetPos()
    {
        var pos = mModel.position;
        pos.x = (pos.x / 2) * 2 + 1;
        pos.y = (pos.y / 2) * 2;
        pos.z = (pos.z / 2) * 2 + 1;


        transform.position = pos + forward;
    }
}
