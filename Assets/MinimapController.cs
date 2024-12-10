using UnityEngine;

public class MinimapController : MonoBehaviour
{
    // Minimap camera
    public Camera minimapCamera;

    private bool isPaused = false;

    void Start()
    {
        minimapCamera.enabled = isPaused;
        Bounds levelBounds = new Bounds(new Vector3(20, -20, -1), new Vector3(55, 55, 0));

        // Set the size of the minimap camera's viewport to cover the entire level
        minimapCamera.rect = new Rect(0.7f, 0.5f, 0.3f, 0.75f);

        // Set the orthographic size of the minimap camera to cover the entire level
        minimapCamera.orthographicSize = Mathf.Max(levelBounds.size.x, levelBounds.size.y) / 2;

        // Set the position of the minimap camera to the center of the level
        minimapCamera.transform.position = levelBounds.center;
    }

    private void Update()
    {
        // Check if the "M" key was pressed
        if (Input.GetKeyDown(KeyCode.M))
        {
            // Toggle the pause state
            isPaused = !isPaused;

            // Enable or disable the minimap camera based on the pause state
            minimapCamera.enabled = isPaused;

            // Pause or resume the game based on the pause state
            Time.timeScale = isPaused ? 0 : 1;
        }
    }
}