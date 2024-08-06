using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGamePlay : MonoBehaviour
{
    public string myScene = "";

    public void LoadLevel()
    {
        SceneManager.LoadScene(myScene);
    }
}