using UnityEngine;

public class InputField : MonoBehaviour
{
    public int column;

    public GameManager gameManager;

    private void OnMouseDown()
    {
        gameManager.SelectColumn(column);

    }

    void OnMouseOver()
    {
        gameManager.HoverColumn(column); 
    }

}
