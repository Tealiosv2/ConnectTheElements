using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Switches cutscene.
/// </summary>
public class CutsceneSwitcher: MonoBehaviour
{
    public void OnEnable()
    {
        SceneManager.LoadScene(2);
    }
}
