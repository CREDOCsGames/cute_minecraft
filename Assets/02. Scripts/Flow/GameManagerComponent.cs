using Flow;
using TMPro;
using UnityEngine;

public class GameManagerComponent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ui;
    private void Awake()
    {
        StartGame();
    }
    private void Update()
    {
        _ui.text = "Map_"+GameSceneManager.CurrentMap;
    }
    public void StartGame()
    {
        GameManager.StartGame();
    }
}
