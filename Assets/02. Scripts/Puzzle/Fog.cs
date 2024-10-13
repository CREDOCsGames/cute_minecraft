using Flow;
using Movement;
using UnityEngine;

public class Fog : MonoBehaviour
{
    void StartMove(Bounds bounds)
    {
        StartCoroutine(FixedIntervalMovement.MoveTo(transform, bounds.center,1f));
    }

    private void Awake()
    {
        GameManager.PuzzleArea.OnEnterEvent += StartMove;
    }
    private void OnDestroy()
    {
        GameManager.PuzzleArea.OnEnterEvent -= StartMove;
    }
}
