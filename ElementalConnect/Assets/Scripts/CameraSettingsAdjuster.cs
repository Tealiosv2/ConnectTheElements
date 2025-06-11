using UnityEngine;

/// <summary>
/// Adjusts the main camera's position and rotation based on the current platform the game is being played on.
/// </summary>
public class CameraSettingsAdjuster : MonoBehaviour
{

    void Start()
    {
        void Start()
        {
            Camera cam = Camera.main;

#if UNITY_IOS
            cam.transform.position = new Vector3(13.66f, 9.78f, -2.81f);
            cam.transform.rotation = Quaternion.Euler(5.708f, -89.96f, 0f);
#else // Desktop or Editor
            cam.transform.position = new Vector3(9.93f, 12.59f, -2.81f);
            cam.transform.rotation = Quaternion.Euler(28.592f, -89.96f, 0f);
#endif
        }
    }

}
