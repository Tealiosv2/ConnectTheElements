/// <summary>
/// Provides methods to evaluate the outcome of a game round, including checking for wins and draws.
/// </summary>
public class OutcomeManager
{   
    /// <summary>
    /// Checks the board for a winning sequence of four matching game pieces horizontally, vertically, or diagonally.
    /// </summary>
    /// <param name="length">The number of columns in the board.</param>
    /// <param name="height">The number of rows in the board.</param>
    /// <param name="board">A 2D array representing the current state of the board.</param>
    /// <returns>
    /// An integer array containing the starting (x, y) position and direction increments for the winning sequence.
    /// If no winning sequence is found, returns an empty array indicating that the game continues.
    /// </returns>
    public static int[] RoundResults(int length, int height, GamePiece[,] board)
    {
        // Horizontal
        for (int x = 0; x < length - 3; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (IsMatch(board[x, y], board[x + 1, y], board[x + 2, y], board[x + 3, y]))
                    return new int[] { x, y, 1, 0 };
            }
        }

        // Vertical
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < height - 3; y++)
            {
                if (IsMatch(board[x, y], board[x, y + 1], board[x, y + 2], board[x, y + 3]))
                    return new int[] { x, y, 0, 1 };
            }
        }

        // Diagonal down-right
        for (int x = 0; x < length - 3; x++)
        {
            for (int y = 0; y < height - 3; y++)
            {
                if (IsMatch(board[x, y], board[x + 1, y + 1], board[x + 2, y + 2], board[x + 3, y + 3]))
                    return new int[] { x, y, 1, 1 };
            }
        }

        // Diagonal up-right
        for (int x = 0; x < length - 3; x++)
        {
            for (int y = 3; y < height; y++)
            {
                if (IsMatch(board[x, y], board[x + 1, y - 1], board[x + 2, y - 2], board[x + 3, y - 3]))
                    return new int[] { x, y, 1, -1 };
            }
        }

        return new int[] { }; // game continues
    }

    /// <summary>
    /// Determines whether four game pieces match by comparing their element types and player IDs.
    /// </summary>
    /// <param name="a">The first game piece.</param>
    /// <param name="b">The second game piece.</param>
    /// <param name="c">The third game piece.</param>
    /// <param name="d">The fourth game piece.</param>
    /// <returns>
    /// True if all four game pieces are non-null and have the same element type and player ID; otherwise, false.
    /// </returns>
    private static bool IsMatch(GamePiece a, GamePiece b, GamePiece c, GamePiece d)
    {
        if (a == null || b == null || c == null || d == null)
        {
            return false;
        }

        bool match = a.elementType == b.elementType &&
                    a.elementType == c.elementType &&
                    a.elementType == d.elementType &&
                    a.playerID == b.playerID &&
                    a.playerID == c.playerID &&
                    a.playerID == d.playerID;

        return match;
    }

    /// <summary>
    /// Determines whether the game has ended in a draw by checking if the top row of every column is filled.
    /// </summary>
    /// <param name="length">The number of columns in the board.</param>
    /// <param name="height">The number of rows in the board.</param>
    /// <param name="board">A two-dimensional array representing the current state of the board.</param>
    /// <returns>True if every column's top cell is occupied, indicating a draw; otherwise, false.</returns>
    public static bool IsDraw(int length, int height, GamePiece[,] board)
    {
        for (int x = 0; x < length; x++)
        {
            if (board[x, height - 1] == null)
                return false;
        }
        return true;
    }
}
