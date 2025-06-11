using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the game board state and controls general behavior of the board.
/// </summary>
public class BoardManager : MonoBehaviour
{
    public GameObject winCircle;
    public GameObject[] spawnLoc;

    GameObject fallingPiece;

    GamePiece[,] boardState;

    Dictionary<ElementType, ElementType> elemWeakness;

    List<(int, int)> coordinatesToDestroy = new List<(int, int)>();

    const int HEIGHT_OF_BOARD = 6;
    const int LENGTH_OF_BOARD = 7;
    const float PIECE_WIDTH = 1.0f;

     /// <summary>
    /// Initializes the board state.
    /// </summary>
    void Start()
    {
        boardState = new GamePiece[LENGTH_OF_BOARD, HEIGHT_OF_BOARD];
        elemWeakness = new Dictionary<ElementType, ElementType>();
        elemWeakness.Add(ElementType.Fire, ElementType.Water); // Populate dictioanry with type weaknesses 
        elemWeakness.Add(ElementType.Air, ElementType.Fire);
        elemWeakness.Add(ElementType.Water, ElementType.Earth);
        elemWeakness.Add(ElementType.Earth, ElementType.Air);
    }

    /// <summary>
    /// Determines whether the specified column is occupied at the top ("hovered")
    /// </summary>
    /// <param name="column">The index of the column to check.</param>
    /// <returns>True if the top cell of the column is occupied; otherwise, false.</returns>
    public bool IsHoverColumn(int column)
    {
        return boardState[column, HEIGHT_OF_BOARD - 1] != null;
    }

    /// <summary>
    /// Determines whether a column is selectable based on the state of the falling piece and game status.
    /// </summary>
    /// <param name="isGameOver">Indicates whether the game is over.</param>
    /// <returns>
    /// True if no piece is currently falling or if the falling piece has settled and the game is not over; otherwise, false.
    /// </returns>
    public bool IsSelectColumn(bool isGameOver)
    {
        return fallingPiece == null || fallingPiece.GetComponent<Rigidbody>().linearVelocity == Vector3.zero && !isGameOver;
    }

    /// <summary>
    /// Gets the width of a game piece.
    /// </summary>
    /// <returns>The width of a game piece.</returns>
    public float GetPieceWidth() => PIECE_WIDTH;

    /// <summary>
    /// Sets the current falling piece on the board and plays the corresponding sound effect.
    /// </summary>
    /// <param name="piece">The game piece that is falling.</param>
    public void SetFallingPiece(GameObject piece)
    {
        piece.transform.position = new Vector3(piece.transform.position.x, piece.transform.position.y, piece.transform.position.z);
        fallingPiece = piece;
        AudioManager.Instance.PlaySFX("pieceDrop");
    }

    /// <summary>
    /// Updates the board state.
    /// </summary>
    /// <param name="newBoardState">New board state.</param>
    public void SetBoardState(GamePiece[,] newBoardState) => boardState = newBoardState;

    /// <summary>
    /// Gets the current board state.
    /// </summary>
    /// <returns>Current board state.</returns>
    public GamePiece[,] GetBoardState() => boardState;

    /// <summary>
    /// Gets the height of the board.
    /// </summary>
    /// <returns>The number of rows in the board.</returns>
    public int GetBoardHeight() => HEIGHT_OF_BOARD;

    /// <summary>
    /// Returns the length of the board.
    /// </summary>
    /// <returns>The number of columns in the board.</returns>
    public int GetBoardLength() => LENGTH_OF_BOARD;

    /// <summary>
    /// Creates win circle  objects at positions determined by starting indices and steps.
    /// </summary>
    /// <param name="startX">The starting index for the spawn location array on the X-axis.</param>
    /// <param name="startY">The starting Y position index for positioning the win circles.</param>
    /// <param name="stepX">The incremental step for the X-axis.</param>
    /// <param name="stepY">The incremental step for the Y-axis.</param>
    public void SpawnWinCircles(int startX, int startY, int stepX, int stepY)
    {
        for (int i = 0; i < 4; i++)
        {
            int x = startX + (i * stepX);
            int y = startY + (i * stepY);
            Vector3 spawnPos = spawnLoc[x].transform.position + new Vector3(0.1f, (y * PIECE_WIDTH) - 6.1f, 0);
            Instantiate(winCircle, spawnPos, Quaternion.Euler(0, 90, 0));
        }
    }

    /// <summary>
    /// Updates the board state by placing a game piece into the specified column at the first available row.
    /// </summary>
    /// <param name="column">The column index where the piece should be placed.</param>
    /// <param name="piece">The game piece to place.</param>
    /// <returns>True if the piece was placed successfully; otherwise, false.</returns>
    public bool UpdateBoardState(int column, GamePiece piece)
    {
        for (int row = 0; row < HEIGHT_OF_BOARD; row++)
        {
            if (boardState[column, row] == null)
            {
                boardState[column, row] = piece;

                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Clears the board by destroying all game pieces and setting all indices to null.
    /// </summary>
    public void ClearBoard()
    {
        // Clear the board state
        for (int x = 0; x < LENGTH_OF_BOARD; x++)
        {
            for (int y = 0; y < HEIGHT_OF_BOARD; y++)
            {
                if (boardState[x, y] != null)
                {
                    Destroy(boardState[x, y].gameObject); // Remove the piece from the scene
                    boardState[x, y] = null; // Clear the board state
                }
            }
        }

    }
    
    /// <summary>
    /// Creates and returns a deep copy of the current board state.
    /// </summary>
    /// <returns>A 2D array representing a simulated copy of the board state.</returns>
    public GamePiece[,] GetSimulatedBoard()
    {
        GamePiece[,] simulatedBoard = new GamePiece[LENGTH_OF_BOARD, HEIGHT_OF_BOARD];
        for (int x = 0; x < LENGTH_OF_BOARD; x++)
        {
            for (int y = 0; y < HEIGHT_OF_BOARD; y++)
            {
                simulatedBoard[x, y] = boardState[x, y];
            }
        }
        return simulatedBoard;
    }

    /// <summary>
    /// Determines if the specified column is valid for a move by checking boundaries and available space.
    /// </summary>
    /// <param name="column">The column index to validate.</param>
    /// <returns>True if the column is within bounds and has space available; otherwise, false.</returns>
    public bool IsColumnValid(int column)
    {
        // Check if column is within bounds
        if (column < 0 || column >= LENGTH_OF_BOARD) return false;

        // Check if there's space in the column (top row is empty)
        return boardState[column, HEIGHT_OF_BOARD - 1] == null;
    }

    /// <summary>
    /// Triggers the destruction of game pieces that are surrounded by opposing pieces based on elemental weaknesses,
    /// then applies gravity to update the board state accordingly.
    /// </summary>
    public void TriggerDestruction()
    {
        // Create an array of gamepieces to be destroyed

        GamePiece[] piecesToDestroy = new GamePiece[HEIGHT_OF_BOARD * LENGTH_OF_BOARD];
        int filledPieces = 0;


        // First, loop through each board to determine if the pieces will be destroyed
        for (int i = 0; i < LENGTH_OF_BOARD; i++)
        {

            for (int j = 0; j < HEIGHT_OF_BOARD; j++)
            {

                if (boardState[i, j] != null)
                {
                    int piecesSurrounded = 0;
                    ElementType strongElem = elemWeakness[boardState[i, j].elementType];

                    // Checking up only on rows not 1st row
                    if (i > 0 && boardState[i - 1, j] != null)
                    {
                        if (boardState[i - 1, j].elementType == strongElem && boardState[i - 1, j].playerID != boardState[i, j].playerID)
                        {
                            piecesSurrounded++;
                        }
                    }

                    // Check down only on rows not last row
                    if (i < HEIGHT_OF_BOARD - 1 && boardState[i + 1, j] != null)
                    {
                        if (boardState[i + 1, j].elementType == strongElem && boardState[i + 1, j].playerID != boardState[i, j].playerID)
                        {
                            piecesSurrounded++;
                        }
                    }

                    if (j < LENGTH_OF_BOARD - 1 && boardState[i, j + 1] != null)
                    {
                        if (boardState[i, j + 1].elementType == strongElem && boardState[i, j + 1].playerID != boardState[i, j].playerID)
                        {
                            piecesSurrounded++;
                        }
                    }

                    // Check left only on columns not first column
                    if (j > 0 && boardState[i, j - 1] != null)
                    {
                        if (boardState[i, j - 1].elementType == strongElem && boardState[i, j - 1].playerID != boardState[i, j].playerID)
                        {
                            piecesSurrounded++;
                        }
                    }

                    if (piecesSurrounded > 1)
                    {
                        piecesToDestroy[filledPieces] = boardState[i, j];
                        coordinatesToDestroy.Add((i, j));
                        filledPieces++;
                    }

                }


            }
        }
        // We want to destroy all these pieces, if deleting them in an array causes visual issues we can always attach them to a parent and delete that
        for (int i = 0; i < piecesToDestroy.Length; i++)
        {
            if (piecesToDestroy[i] != null)
            {
                Destroy(piecesToDestroy[i].gameObject);
            }
        }
        // 1. Clear destroyed pieces (with bounds checking)
        foreach ((int x, int y) in coordinatesToDestroy)
        {
            // Swap x and y when accessing boardState!
            if (x >= 0 && x < LENGTH_OF_BOARD && y >= 0 && y < HEIGHT_OF_BOARD)
            {
                boardState[x, y] = null; // y = row, x = column
            }
        }

        Array.Clear(piecesToDestroy, 0, piecesToDestroy.Length);
        coordinatesToDestroy.Clear();

        // 2. Apply downward gravity to each column
        for (int x = 0; x < LENGTH_OF_BOARD; x++) // Process each column
        {
            int emptyY = 0; // Start checking from bottom

            // Scan from bottom up (y = height-1 to 0)
            for (int y = 0; y < HEIGHT_OF_BOARD; y++)
            {
                if (boardState[x, y] != null) // If there's a block here
                {
                    // Move it down if needed
                    if (emptyY != y)
                    {
                        boardState[x, emptyY] = boardState[x, y];
                        boardState[x, y] = null;
                    }
                    emptyY++; // Next empty spot is above
                }
            }
        }

    }

    /// <summary>
    /// Simulates placing a game piece on a provided board in the specified column.
    /// </summary>
    /// <param name="simulatedBoard">A 2D array representing the simulated board state.</param>
    /// <param name="column">The column index where the piece should be placed.</param>
    /// <param name="elementalPiece">The game piece to place in the simulated move.</param>
    /// <returns>True if the piece was successfully placed; otherwise, false.</returns>
    public bool SimulateMove(GamePiece[,] simulatedBoard, int column, GamePiece elementalPiece)
    {
        // Check if the column is valid
        if (column < 0 || column >= LENGTH_OF_BOARD || simulatedBoard[column, HEIGHT_OF_BOARD - 1] != null)
        {
            return false;
        }

        // Find the first empty row in the column
        for (int row = 0; row < HEIGHT_OF_BOARD; row++)
        {
            // Checks if the column and row does not have a piece
            if (simulatedBoard[column, row] == null)
            {
                // Simulate placing a piece in the column
                simulatedBoard[column, row] = elementalPiece;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Prints the current board state.
    /// </summary>
    public void PrintBoard()
    {
        for (int x = 0; x < LENGTH_OF_BOARD; x++)
        {
            for (int y = 0; y < HEIGHT_OF_BOARD; y++)
            {
                if (boardState[x, y] != null)
                {

                    Debug.Log("Position (" + x + "," + y + "): ");
                    Debug.Log(boardState[x, y]);
                }
            }
        }
    }

}