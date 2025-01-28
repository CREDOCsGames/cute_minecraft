using System;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneUIComponent : MonoBehaviour
{
    [SerializeField] private Button _skipButton;
    public event Action OnSkip;

    public void OnUI()
    {
        gameObject.SetActive(true);
    }
    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        if (_skipButton != null)
        {
            _skipButton.onClick.AddListener(() => { OnSkip?.Invoke(); });
        }
    }
}
