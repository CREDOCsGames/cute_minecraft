using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonEventPlace : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private UnityEvent _buttonEvent;
    private readonly List<Collider> _players = new();


    private void OnEnable()
    {
        _icon.gameObject.SetActive(0 < _players.Count);
    }
    private void Update()
    {
        if (_icon.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            _icon.gameObject.SetActive(false);
            _buttonEvent.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player"))
        {
            return;
        }
        if (_players.Contains(other))
        {
            return;
        }
        _players.Add(other);
        if (_players.Count == 1)
        {
            _icon.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _players.Remove(other);
        if (_players.Count == 0)
        {
            _icon.gameObject.SetActive(false);
        }
    }
}
