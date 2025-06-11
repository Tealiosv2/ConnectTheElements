/// <summary>
/// Represents a piece with the air element; extends on base GamePiece class
/// </summary>
public class AirPiece: GamePiece
{
    private void Awake()
    {
        elementType = ElementType.Air;
    }
}
