using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer2 : MonoBehaviour
{
    [SerializeField]
    float timerDuration = 1f;
    AudioSource source;
    [SerializeField]
    bool repeating = true;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(MyTimerCoroutine());
    }

    IEnumerator MyTimerCoroutine()
    {
        yield return new WaitForSeconds(timerDuration);
        source.Play();
        while(true)
        {
            yield return new WaitForSeconds(timerDuration);
            if(repeating) source.Play();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
