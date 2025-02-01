using UnityEngine;

public class GameResource<T> where T : MonoBehaviour
{
    private readonly string _path;
    private T _instance;
    public T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Instantiate(Resources.Load<T>(_path));
                _instance.gameObject.SetActive(false);
                GameObject.DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }
    public GameResource(string path)
    {
        _path = path;
    }
}

public static class UIResources
{
    public static GameResource<CutSceneUIComponent> CutSceneUI
        = new("CutSceneUI");
}
