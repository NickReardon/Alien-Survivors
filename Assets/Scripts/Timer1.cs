using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer1 : MonoBehaviour
{
    [SerializeField]
    float timerDuration = 1f;
    [SerializeField]
    bool repeating = false;
    float elapsedTime = 0f;
    AudioSource source;
    [SerializeField]
    AudioClip soundOverride;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > timerDuration)
        {
            if(soundOverride)
                source.PlayOneShot(soundOverride);
            else
                source.Play();
            
            if(repeating)
                elapsedTime = 0f;
            else
                enabled = false;
            //source.Play
        }
    }
}
