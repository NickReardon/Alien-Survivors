using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnClick : MonoBehaviour
{
    AudioSource _audio;
    [SerializeField]
    Tilemap tilemap;
    Camera main;
    void Start()
    {
        //Here, a reference to the desired component is established, and will be reused throughout the class
        _audio = GetComponent<AudioSource>();
        main = Camera.main;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int mouseCell = tilemap.WorldToCell(mouseWorldPos);
            if(tilemap.GetTile(mouseCell) != null)
            {
                //The below will work, but it is wasteful of resources compared to caching
                //GetComponent<AudioSource>().Play();

                //Instead, we will use our cached component
                _audio.Play();
            }
        }
    }
}
