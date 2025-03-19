using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickParticle : MonoBehaviour
{
    bool expanding = true;
    float timer = 0, curScale = 0;
    public float maxScale;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
        var sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(expanding)
        {
            curScale = Mathf.Lerp(0,maxScale,timer);
            if(timer >= 1f)
            {
                expanding = false;
                timer = 0;
            }
        }
        else
        {
            curScale = Mathf.Lerp(maxScale,0,timer);
            if(timer >= 1f)
                Destroy(gameObject);
        }
        transform.localScale = Vector3.one * curScale;
    }
}
