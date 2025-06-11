using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Manages overall game state, including player turns, AI interaction, UI elements,
/// and handling win conditions.
/// </summary>
public class GameManager : MonoBehaviour
{
    public PlayerType player1Type = PlayerType.Human;
    public PlayerType player2Type = PlayerType.Human;

    public BoardManager boardManager;

    private int currentPlayerID = 1;

    public GameObject Fire1;

    public GameObject Water1;

    public GameObject Earth1;

    public GameObject Air1;

    public GameObject Fire2;

    public GameObject Water2;

    public GameObject Earth2;

    public GameObject Air2;

    public GameObject player1Ghost;
    public GameObject player2Ghost;

    public GameObject winScreen;
    public Text winText;

    public ParticleSystem winParticleEffect1;
    public ParticleSystem winParticleEffect2;
    public ParticleSystem winParticleEffect3;
    public ParticleSystem winParticleEffect4;

    public GameObject FireBowl;

    public GameObject WaterBowl;
    public GameObject AirBowl;
    public GameObject EarthBowl;

    AIController aiController;

    bool isGameOver = false;

    int pieceType = 0;

    int currentTurn = 0;

    bool isAIMakingMove = false;

    const int DESTRUCTION_ROUND_TURN = 6;

    /// <summary>
    /// Initializes the game state and AI controller at the start of the scene.
    /// </summary>
    void Start()
    {
        DeactivatePlayerGhosts();
        winScreen.SetActive(false);
        AudioManager.Instance.PlayMusic("GameScene");
        AudioManager.Instance.ToggleMusic();

        // Initialize AI Controller
        aiController = gameObject.AddComponent<AIController>();
        aiController.Air2 = Air2;
        aiController.Fire2 = Fire2;
        aiController.Water2 = Water2;
        aiController.Earth2 = Earth2;
        aiController.Initialize(this, boardManager);
    }

    /// <summary>
    /// Initializes the game state and AI controller at the start of the scene.
    /// </summary>
    void Update()
    {
        // Only check for AI moves if the game isn't over and if the game mode is AI
        if (!isGameOver && GameModeData.gameMode == GameMode.PvAI)
        {
            if (currentPlayerID == 2)
            {

                if (!isAIMakingMove)
                {
                    StartCoroutine(TriggerAIMove());
                }

                if (!isAIMakingMove)
                {
                    if (currentTurn == DESTRUCTION_ROUND_TURN)
                    {
                        boardManager.TriggerDestruction();
                        currentTurn = 0;
                    }
                    else
                    {
                        currentTurn++;
                    }
                }

            }
        }
    }

    /// <summary>
    /// Coroutine that delays the AI's move to simulate thinking time.
    /// </summary>
    IEnumerator TriggerAIMove()
    {
        isAIMakingMove = true;
        yield return new WaitForSeconds(2.5f); // Small delay before AI moves
        aiController.MakeAIMove();
        isAIMakingMove = false;
    }

    /// <summary>
    /// Disables the ghost pieces for both players.
    /// </summary>
    void DeactivatePlayerGhosts()
    {
        player1Ghost.SetActive(false);
        player2Ghost.SetActive(false);
    }

    /// <summary>
    /// Displays the screen with the result of the game when game ends.
    /// </summary
    void ShowWinScreen(bool isDraw, int playerNum, int[] roundResults)
    {
        isGameOver = true;
        winScreen.SetActive(true);
        AudioManager.Instance.ToggleMusic();
        if (isDraw)
        {
            winText.text = "Draw!";
        }
        else
        {
            boardManager.SpawnWinCircles(roundResults[0], roundResults[1], roundResults[2], roundResults[3]);
            winText.text = "Player " + playerNum + " Won!";

            if (winParticleEffect1 != null)
            {
                winParticleEffect1.Play();
            }

            if (winParticleEffect2 != null)
            {
                winParticleEffect2.Play();
            }

            if (winParticleEffect3 != null)
            {
                winParticleEffect3.Play();
            }

            if (winParticleEffect4 != null)
            {
                winParticleEffect4.Play();
            }
        }
    }

    /// <summary>
    /// Returns the selected piece prefab for the current player and piece type.
    /// </summary>
    GameObject PieceSelector()
    {
        if (currentPlayerID == 1)
        {
            return pieceType switch
            {
                0 => Air1,
                1 => Fire1,
                2 => Earth1,
                3 => Water1,
                _ => Air1,
            };

        }
        else
        {
            return pieceType switch
            {
                0 => Air2,
                1 => Fire2,
                2 => Earth2,
                3 => Water2,
                _ => Air2,
            };

        }
    }
    
    /// <summary>
    /// Gets the ID of the player currently playing
    /// </summary>
    public int GetCurrentPlayerID()
    {
        return currentPlayerID;
    }

    /// <summary>
    /// Triggers the ghost piece display logic when hovering over a column.
    /// </summary>
    public void HoverColumn(int column)
    {
        if (boardManager.IsHoverColumn(column))
        {
            DeactivatePlayerGhosts();
        }
    }

    /// <summary>
    /// Handles logic when a column is selected by a player.
    /// </summary>
    public void SelectColumn(int column)
    {
        if (boardManager.IsSelectColumn(isGameOver))
        {
            TakeTurn(column);
            DeactivatePlayerGhosts();
        }
    }

    /// <summary>
    /// Handles the logic for placing a piece on the board for the current player.
    /// </summary>
    public void TakeTurn(int column)
    {

        // Determine which player is making the move
        PlayerType currentPlayerType = currentPlayerID == 1 ? player1Type : player2Type;

        if (GameModeData.gameMode == GameMode.PvAI)
        {

            // For AI turns, only accept moves from the AI controller
            if (currentPlayerType == PlayerType.AI && !isAIMakingMove)
            {
                return;
            }

        }

        GameObject prefab = PieceSelector();
        GameObject newPiece = Instantiate(prefab, boardManager.spawnLoc[column].transform.position, prefab.transform.rotation);
        newPiece.transform.position += new Vector3(0.6f, 0f, 0f);

        boardManager.SetFallingPiece(prefab);

        GamePiece pieceComponent = newPiece.GetComponentInChildren<GamePiece>();
        if (pieceComponent == null)
        {
            // Serious error, game component uninstantiated
            Destroy(newPiece);
            return;
        }

        pieceComponent.playerID = currentPlayerID;

        if (boardManager.UpdateBoardState(column, pieceComponent))
        {
            // 4. Drop it visually
            boardManager.SetFallingPiece(newPiece);

            // Check if destruction round starts
            if (currentTurn == DESTRUCTION_ROUND_TURN)
            {
                boardManager.TriggerDestruction();
                currentTurn = 0;
            }
            else
            {
                currentTurn++;
            }

            GamePiece[,] board = boardManager.GetBoardState();
            int[] result = OutcomeManager.RoundResults(boardManager.GetBoardLength(), boardManager.GetBoardHeight(), board);

            if (result.Length > 0)
            {
                int winner = board[result[0], result[1]].playerID;
                ShowWinScreen(false, winner, result);
            }
            else if (OutcomeManager.IsDraw(boardManager.GetBoardLength(), boardManager.GetBoardHeight(), board))
            {
                ShowWinScreen(true, 0, new int[] { });
            }
            currentPlayerID = currentPlayerID == 1 ? 2 : 1;
            DeactivatePlayerGhosts();
        }
        else
        {
            Destroy(newPiece); // failed to place it
        }
    }

    /// <summary>
    /// Quits the game. Only works in a built version of the game.
    /// </summary>
    public static void QuitGame()
    {
        Application.Quit(); // does not work in editor, only in build
    }

    /// <summary>
    /// Reloads the current scene to restart the game.
    /// </summary>
    public static void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Resets the game to its initial state.
    /// </summary>
    public void ResetGame()
    {
        // Reset game variables
        isGameOver = false;
        currentPlayerID = 1; // Player 1 starts
        pieceType = 0; // Default piece type

        // Reset the board
        boardManager.ClearBoard();

        // Reset UI elements
        winScreen.SetActive(false);
        winText.text = "";

        if (GameModeData.gameMode == GameMode.PvAI)
        {
            player1Type = PlayerType.Human;
            player2Type = PlayerType.AI;
        }
        else
        {
            player1Type = PlayerType.Human;
            player2Type = PlayerType.Human;
        }

        // Deactivate player ghosts
        DeactivatePlayerGhosts();

    }

    /// <summary>
    /// Selects an air piece for the next move.
    /// </summary>
    public void SelectAirPiece()
    {
        pieceType = 0;
        SetSelectedBowl(AirBowl);
    }
    
    /// <summary>
    /// Selects a fire piece for the next move.
    /// </summary>
    public void SelectFirePiece()
    {

        pieceType = 1;

        SetSelectedBowl(FireBowl);

    }

    /// <summary>
    /// Selects an earth piece for the next move.
    /// </summary>
    public void SelectEarthPiece()
    {
        pieceType = 2;
        

        SetSelectedBowl(EarthBowl);
    }

     /// <summary>
    /// Selects a water piece for the next move.
    /// </summary>
    public void SelectWaterPiece()
    {
        pieceType = 3;
        SetSelectedBowl(WaterBowl);
    }

    /// <summary>
    /// Sets outliner shader to correct bowl
    /// </summary>
    /// <param name="selectedBowl"></param>
    private void SetSelectedBowl(GameObject selectedBowl){
        GameObject[] bowls = new GameObject[] {AirBowl, FireBowl, EarthBowl, WaterBowl};

    foreach (GameObject bowl in bowls)
    {
        float outlineValue = (bowl == selectedBowl) ? 1.02f : 1.0f;

        Transform bowlTransform = bowl.transform.Find("bowl/Bowl_LOD1");
        if (bowlTransform != null)
        {
            Renderer rend = bowlTransform.GetComponent<Renderer>();
            if (rend != null && rend.materials.Length > 1)
            {
                Material outlineMat = rend.materials[1];
                outlineMat.SetFloat("_scale", outlineValue); 
            }
        }
    }
    }
}
