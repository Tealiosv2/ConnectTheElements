/// <summary>
/// Represents a piece with the earth element; extends on base GamePiece class
/// </summary>
public class EarthPiece: GamePiece
{
 
    private void Awake()
    {
        elementType = ElementType.Earth;
    }
}