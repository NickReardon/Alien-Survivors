using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour
{
    public GameObject backgroundTilePrefab; // Prefab of the background tile
    private Transform cameraTransform;
    private List<GameObject> backgroundTiles = new List<GameObject>();
    private Vector2 backgroundSize;
    private int tilesX, tilesY;
    private Vector3 previousCameraPosition;
    public float overlapOffset = 0.1f; // Amount of overlap

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;

        // Get the size of the background tile from the SpriteRenderer component
        SpriteRenderer spriteRenderer = backgroundTilePrefab.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            backgroundSize = spriteRenderer.bounds.size;
        }
        else
        {
            Debug.LogError("Background tile prefab does not have a SpriteRenderer component.");
            return;
        }

        // Adjust the background size to include the overlap offset
        backgroundSize -= new Vector2(overlapOffset, overlapOffset);

        // Calculate the number of tiles needed to cover the camera's view
        float cameraHeight = Camera.main.orthographicSize * 2f;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        tilesX = Mathf.CeilToInt(cameraWidth / backgroundSize.x) + 1;
        tilesY = Mathf.CeilToInt(cameraHeight / backgroundSize.y) + 1;

        // Create a grid of background tiles
        for (int x = -tilesX; x <= tilesX; x++)
        {
            for (int y = -tilesY; y <= tilesY; y++)
            {
                Vector3 tilePosition = new Vector3(x * backgroundSize.x, y * backgroundSize.y, 0) + transform.position;
                GameObject tile = Instantiate(backgroundTilePrefab, tilePosition, Quaternion.Euler(0, 0, 90 * Random.Range(0, 4)), transform);
                backgroundTiles.Add(tile);
            }
        }

        // Initialize the previous camera position
        previousCameraPosition = cameraTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the camera has moved a significant distance
        if (Vector3.Distance(cameraTransform.position, previousCameraPosition) > 0.1f)
        {
            // Reposition tiles to maintain seamless background
            foreach (GameObject tile in backgroundTiles)
            {
                Vector3 tilePosition = tile.transform.position;
                bool tileMoved = false;

                if (cameraTransform.position.x - tilePosition.x >= backgroundSize.x * tilesX)
                {
                    tilePosition.x += backgroundSize.x * (tilesX * 2);
                    tileMoved = true;
                }
                else if (tilePosition.x - cameraTransform.position.x >= backgroundSize.x * tilesX)
                {
                    tilePosition.x -= backgroundSize.x * (tilesX * 2);
                    tileMoved = true;
                }

                if (cameraTransform.position.y - tilePosition.y >= backgroundSize.y * tilesY)
                {
                    tilePosition.y += backgroundSize.y * (tilesY * 2);
                    tileMoved = true;
                }
                else if (tilePosition.y - cameraTransform.position.y >= backgroundSize.y * tilesY)
                {
                    tilePosition.y -= backgroundSize.y * (tilesY * 2);
                    tileMoved = true;
                }

                if (tileMoved)
                {
                    tile.transform.position = tilePosition;
                    tile.transform.rotation = Quaternion.Euler(0, 0, 90 * Random.Range(0, 4));
                }
            }

            // Update the previous camera position
            previousCameraPosition = cameraTransform.position;
        }
    }
}