using UnityEngine;

/// <summary>
/// Abstract base class that represents a game piece within the game, to be extended by elemental pieces
/// </summary>
public abstract class GamePiece : MonoBehaviour
{
    public int playerID; //p1 or p2
    public GameObject model;
    public ElementType elementType;

    /// <summary>
    /// Initializes the game piece with the specified player ID, model prefab, and element type.
    /// </summary>
    /// <param name="playerID">The identifier for the player owning this game piece.</param>
    /// <param name="modelPrefab">The prefab for the model that will be instantiated.</param>
    /// <param name="elementType">The element type associated with this game piece.</param>
    public virtual void Initialize(int playerID, GameObject modelPrefab, ElementType elementType)
    {
        this.playerID = playerID;
        this.elementType = elementType;
        if (modelPrefab)
        {
            model = Instantiate(modelPrefab, transform.position, Quaternion.identity, transform);
        }
    }
}