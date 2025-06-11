using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles scene transitions and mode selection on the main menu.
/// </summary>

public class SceneLoader: MonoBehaviour
{
    public Button pvpButton;
    public Button aiButton;

    /// <summary>
    /// Registers button click listeners for PvP and AI mode selection.
    /// </summary>
    void Start()
    {
        pvpButton.onClick.AddListener(SelectPvP);
        aiButton.onClick.AddListener(SelectAI);
        
    }

    /// <summary>
    /// Transitions to the game scene after toggling background music.
    /// </summary>
    private void TransitionScene()
    {
        AudioManager.Instance.ToggleMusic();
        SceneManager.LoadScene(1);
    }
    
    /// <summary>
    /// Sets the game mode to PvP and initiates scene transition.
    /// </summary>
    public void SelectPvP()
    {
        GameModeData.gameMode = GameMode.PvP;
        TransitionScene();
    }

    /// <summary>
    /// Sets the game mode to PvAI and initiates scene transition.
    /// </summary>
    public void SelectAI()
    {
        GameModeData.gameMode = GameMode.PvAI;
        TransitionScene();
    }
}
