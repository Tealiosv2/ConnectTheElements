/// <summary>
/// Represents a piece with the fire element; extends on base GamePiece class
/// </summary>
public class FirePiece: GamePiece
{
 
    private void Awake()
    {
        elementType = ElementType.Fire;
    }
}