using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Util/GameObjectUtil")]
public class GameObjectUtil : ScriptableObject
{
    public void ToggleEnable(GameObject go)
    {
        go.SetActive(!go.activeSelf);
    }

}
