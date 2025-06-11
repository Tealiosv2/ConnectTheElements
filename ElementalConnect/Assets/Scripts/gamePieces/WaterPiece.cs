/// <summary>
/// Represents a piece with the water element; extends on base GamePiece class
/// </summary>
public class WaterPiece: GamePiece
{
 
    private void Awake()
    {
        elementType = ElementType.Water;
    }
}