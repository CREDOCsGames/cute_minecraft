using Puzzle;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentControllerComponent : MonoBehaviour
{
    private string _beforeAnim;
    private Vector3 _goal;
    private Action _action;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _walkLeft;
    [SerializeField] private string _walkRight;
    [SerializeField] private string _walkFWD;
    [SerializeField] private string _walkBWD;
    [SerializeField] private string _idle;
    [SerializeField] private GameObject _flower;
    [SerializeField] private BoxCollider _box;

    private void OnEnable()
    {
        if (!FlowerComponent.Instances.Any())
        {
            CreateElements();
            return;
        }

        var i = UnityEngine.Random.Range(0, 2);
        if (i == 0)
        {
            ChangeColor();
        }
        else
        {
            CreateElements();
        }
    }

    private void Update()
    {
        if (!_agent.enabled)
        {
            return;
        }
        _agent.SetDestination(_goal);
        var normal = new Vector2(_agent.velocity.x, _agent.velocity.z).normalized;
        string anim = "";
        if (normal.x > 0.5f)
        {
            anim = _walkRight;
        }
        else if (normal.x < 0.5f)
        {
            anim = _walkLeft;
        }
        if (normal.y > 0.5f)
        {
            anim = _walkFWD;
        }
        else if (normal.y < 0.5f)
        {
            anim = _walkBWD;
        }
        if (Vector3.Distance(transform.position, _goal) <= 2f)
        {
            _action?.Invoke();
            _action = null;
            anim = _idle;
        }
        if (anim.Equals(_beforeAnim))
        {
            return;
        }
        _animator.Play(anim);
        _beforeAnim = anim;
    }

    private void ChangeColor()
    {
        var flower = FlowerComponent.Instances.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).First();
        _goal = flower.transform.position;
        _action = () =>
        {
            var flower1 = flower.GetComponent<FlowerComponent>();
            flower1.Color = flower1.Color == new Color(12f / 255f, 255f / 255f, 255f / 255f) ?
            new Color(118f / 255f, 53f / 255f, 231f / 255f) :
            new Color(12f / 255f, 255f / 255f, 255f / 255f);
            gameObject.SetActive(false);
        };
    }

    private void CreateElements()
    {
        _goal = GetRandomPositionInBox(_box);
        _action = () =>
        {
            var flower = Instantiate(_flower);
            flower.transform.position = _goal;
            gameObject.SetActive(false);
        };

    }

    private Vector3 GetRandomPositionInBox(BoxCollider box)
    {
        // BoxCollider의 중심과 크기를 가져옵니다.
        Vector3 center = box.transform.position;
        Vector3 size = box.size;

        // 랜덤한 x, y, z 좌표를 계산합니다.
        float randomX = UnityEngine.Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float randomY = UnityEngine.Random.Range(center.y - size.y / 2, center.y + size.y / 2);
        float randomZ = UnityEngine.Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        // 랜덤 위치를 반환합니다.
        return box.transform.TransformPoint(new Vector3((int)randomX, (int)randomY, (int)randomZ));
    }
}
