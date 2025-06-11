using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controls AI behavior for selecting and executing moves in the game.
/// </summary>
public class AIController : MonoBehaviour
{
    private GameManager gameManager;
    private BoardManager boardManager;

    public GameObject Fire2;

    public GameObject Water2;

    public GameObject Earth2;

    public GameObject Air2;

    /// <summary>
    /// Initializes the AI controller with references to the GameManager and BoardManager.
    /// </summary>
    /// <param name="gm">Reference to the game manager.</param>
    /// <param name="bm">Reference to the board manager.</param>
    public void Initialize(GameManager gm, BoardManager bm)
    {
        gameManager = gm;
        boardManager = bm;
    }

    /// <summary>
    /// Executes the AI's logic to make a move based on the current board state.
    /// </summary>
    public void MakeAIMove()
    {
        if (gameManager == null || boardManager == null)
        {
            return; // Serious problem, as it means the managers didn't instantiate properly
        }

        // Get all valid columns
        List<int> validColumns = new List<int>();

        for (int col = 0; col < boardManager.GetBoardLength(); col++)
        {
            if (boardManager.IsColumnValid(col))
            {
                validColumns.Add(col);
            }
        }

        // If there are no valid moves, return;
        if (validColumns.Count == 0)
        {
            return;
        }

        // Create piece for the simulated board state
        GameObject prefab = Air2;
        GameObject newPiece = Instantiate(prefab, new Vector3(0, 0, 0), prefab.transform.rotation);
        newPiece.transform.position += new Vector3(0.6f, 0f, 0f);
        GamePiece pieceComponent = newPiece.GetComponentInChildren<GamePiece>();

        // Check if there is a move that results in a win
        foreach (int col in validColumns)
        {
            if (SimulateMoveAndCheckWin(col, pieceComponent))
            {
                gameManager.TakeTurn(col);
                return;
            }
        }

        // Check whether there is a move that blocks a win state        
        foreach (int col in validColumns)
        {
            if (SimulateMoveAndCheckWin(col, pieceComponent))
            {
                gameManager.TakeTurn(col);
                return;
            }
        }

        // If neither, play a random move
        int randomCol = validColumns[Random.Range(0, validColumns.Count)];
        gameManager.TakeTurn(randomCol);
    }

    /// <summary>
    /// Simulates placing a piece in a column and checks if it results in a win.
    /// </summary>
    /// <param name="column">The column to simulate the move in.</param>
    /// <param name="elementalPiece">The game piece to simulate placing.</param>
    /// <returns>True if the move results in a win; otherwise, false.</returns>
    private bool SimulateMoveAndCheckWin(int column, GamePiece elementalPiece)
    {
        // Simulate placing a piece in the column
        GamePiece[,] simulatedBoard = boardManager.GetSimulatedBoard();
        if (!boardManager.SimulateMove(simulatedBoard, column, elementalPiece))
        {
            return false;
        }

        // Check if this move results in a win
        int[] result = OutcomeManager.RoundResults(
            boardManager.GetBoardLength(),
            boardManager.GetBoardHeight(),
            simulatedBoard
        );

        return result.Length > 0; // A result indicates a win
    }

}