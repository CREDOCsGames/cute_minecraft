using System;
using System.Collections;
using Unity.VisualScripting;
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
            _skipButton.onClick.AddListener(StartCoolTime);
        }
    }
    private void StartCoolTime()
    {
        CoroutineRunner.instance.StartCoroutine(LockButton());
    }
    private IEnumerator LockButton()
    {
        _skipButton.interactable = false;
        yield return new WaitForSeconds(0.5f);
        _skipButton.interactable = true;
    }
}
