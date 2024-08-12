using PlatformGame.Character;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RigidBodyHandler : MonoBehaviour
{
    [SerializeField] Rigidbody mRigid;
    Rigidbody Rigid
    {
        get
        {
            Debug.Assert(mRigid != null);
            return mRigid;
        }
    }

    public void Explosion(float force)
    {
        Character.Instances.Where(x => Vector3.Distance(x.transform.position, mRigid.transform.position) < force * 0.1f)
                            .ToList()
                            .ForEach(x => x.Rigid.AddExplosionForce(force, transform.position, force * 0.1f, force * 0.5f));
    }

}
