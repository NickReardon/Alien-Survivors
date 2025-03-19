using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ClickToMove : MonoBehaviour
{
    [SerializeField]
    Vector3 moveDestination;
    [SerializeField]
    float speed = 5f;
    [SerializeField] 
    Camera mainCam, minimapCam;
    [SerializeField]
    Tilemap tilemap;
    [SerializeField]
    List<TileBase> tiles = new List<TileBase>();
    [SerializeField]
    List<float> tileSpeeds = new List<float>();
    [SerializeField]
    GameObject victoryIndicator, timer;
    // Start is called before the first frame update
    void Start()
    {
        moveDestination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            //Debug.Log("Clicked");
            Vector3 mousePos = Input.mousePosition;//To reduce references and clean code
            if(minimapCam.pixelRect.Contains(mousePos))
            {
                //Debug.Log("Minimap clicked!");
                moveDestination = minimapCam.ScreenToWorldPoint(mousePos);
            }
            else
            {
                //Debug.Log("Main Camera Clicked");
                moveDestination = mainCam.ScreenToWorldPoint(mousePos);

            }
            //Drifting z value causes issues
            moveDestination.z = 0;
        }
        TileBase t = tilemap.GetTile(tilemap.WorldToCell( transform.position));
        int index = tiles.IndexOf(t);
        if(index == -1) speed = 5f;
        else speed = tileSpeeds[index];
        if((transform.position-moveDestination).sqrMagnitude > .1f)
        transform.position = Vector3.MoveTowards(transform.position, moveDestination, speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        victoryIndicator.SetActive(true);
        timer.GetComponent<GameTimer>().enabled = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        victoryIndicator.SetActive(true);
        timer.GetComponent<GameTimer>().enabled = false;
    }
}
