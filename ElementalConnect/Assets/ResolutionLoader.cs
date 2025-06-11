using UnityEngine;

public class ResolutionLoader : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        #if UNITY_STANDALONE_WIN
            Screen.SetResolution(416, 900, false);
        #endif
            Screen.orientation = ScreenOrientation.Portrait;
    }
}
