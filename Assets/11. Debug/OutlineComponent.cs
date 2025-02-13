using UnityEngine;

public class OutlineComponent : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    private void OnEnable()
    {
        _renderer.material.SetFloat("_Outline_Width", 40);
    }

    private void OnDisable()
    {
        _renderer.material.SetFloat("_Outline_Width", 0);
    }
}
