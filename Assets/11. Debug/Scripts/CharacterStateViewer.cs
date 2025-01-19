using Controller;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterStateViewer : MonoBehaviour
{
    private bool _bInitialized;
    private TextMeshProUGUI _uiInstance;
    private readonly Queue<CharacterState> _stateHistory = new();
    [SerializeField] private Vector3 _uiOffset;
    [SerializeField] private TextMeshProUGUI _uiPrefab;
    [SerializeField] private CharacterComponent _character;
    [SerializeField, Range(1, 100)] private byte _historyCount = 1;

    private void InitializeViewer()
    {
        if (_bInitialized)
        {
            Destroy(_uiInstance);
            _character._character.OnChagedState -= OnChangedCharacterState;
        }

        GetInitializable(out _bInitialized);
        if (_bInitialized)
        {
            _uiInstance = Instantiate(_uiPrefab, transform);
            _character._character.OnChagedState += OnChangedCharacterState;
        }
    }
    private void GetInitializable(out bool initialized)
    {
        initialized = _character != null && _uiPrefab != null && _character._character != null;
    }
    private void OnChangedCharacterState(CharacterState newState)
    {
        if (!_bInitialized)
        {
            return;
        }

        _stateHistory.Enqueue(newState);
        while (_historyCount < _stateHistory.Count)
        {
            _stateHistory.Dequeue();
        }
        UpdateUIText();
    }
    private void UpdateUIText()
    {
        _uiInstance.text = "";
        foreach (var state in _stateHistory)
        {
            _uiInstance.text += (0 < _uiInstance.text.Length ? '\n' : "") + state.ToString();
        }
    }
    private void UpdateDebugLog(CharacterState newState)
    {
        Debug.Log($"[{Time.time.ToString()}]{_character.gameObject.name}:{newState.ToString()}");
    }
    private void Update()
    {
        if (_bInitialized)
        {
            UpdateUITransform();
        }
        else
        {
            InitializeViewer();
        }
    }
    private void UpdateUITransform()
    {
        _uiInstance.transform.position = _character.transform.position + _uiOffset;
        if (Camera.main != null)
        {
            _uiInstance.transform.LookAt(Camera.main.transform);
        }
    }

}
