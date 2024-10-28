using Battle;
using UnityEngine;
using UnityEngine.Events;

public class AttackBoxComponent : MonoBehaviour
{
    [Header("[Refer]")]
    [SerializeField] UnityEvent<HitBoxCollision> OnAttack;
    [Header("[Options]")]
    [SerializeField] protected Transform Actor;
    [Range(0, 10000)][SerializeField] float AttackWindow;
    AttackBox mAttackBox;
    public AttackBox AttackBox
    {
        get
        {
            Debug.Assert(Actor != null, $"Specify an Actor : {name}");
            mAttackBox ??= CreateAttackBox();
            return mAttackBox;
        }
    }
    public static AttackBoxComponent AddComponent(GameObject obj, AttackBox attackBox)
    {
        var component = obj.AddComponent<AttackBoxComponent>();
        component.Actor = attackBox.Actor;
        component.mAttackBox = attackBox;
        return component;
    }

    public void OpenAttackWindow()
    {
        mAttackBox?.OpenAttackWindow();
    }

    AttackBox CreateAttackBox()
    {
        var hitBox = new AttackBox(Actor, AttackWindow);
        hitBox.OnCollision += OnAttack.Invoke;
        return hitBox;
    }

    void OnTriggerStay(Collider other)
    {
        AttackBox.CheckCollision(other);
    }

}
