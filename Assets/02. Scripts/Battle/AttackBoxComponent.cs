using Battle;
using UnityEngine;
using UnityEngine.Events;

public class AttackBoxComponent : MonoBehaviour
{
    [Header("[Refer]")]
    [SerializeField] protected Transform _actor;
    [Header("[Options]")]
    [SerializeField] private UnityEvent<HitBoxCollision> _onAttack;
    [Range(0, 10000)][SerializeField] private float _attackWindow;
    private AttackBox _attackBox;
    public AttackBox AttackBox
    {
        get
        {
            Debug.Assert(_actor != null, $"Specify an Actor : {name}");
            _attackBox ??= CreateAttackBox();
            return _attackBox;
        }
    }
    public static AttackBoxComponent AddComponent(GameObject obj, AttackBox attackBox)
    {
        var component = obj.AddComponent<AttackBoxComponent>();
        component._actor = attackBox.Actor;
        component._attackBox = attackBox;
        return component;
    }

    public void OpenAttackWindow()
    {
        AttackBox.OpenAttackWindow();
    }

    private AttackBox CreateAttackBox()
    {
        var hitBox = new AttackBox(_actor, _attackWindow);
        hitBox.OnCollision += _onAttack.Invoke;
        return hitBox;
    }

    private void OnTriggerStay(Collider other)
    {
        AttackBox.CheckCollision(other);
    }

}
