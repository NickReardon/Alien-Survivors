using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackObject : MonoBehaviour
{
    [SerializeField] Transform trackedTransform;
    [SerializeField] bool TrackX = false, TrackY = false, TrackZ = false;
    Vector3 _newPosition;
    // Start is called before the first frame update
    void Start()
    {
        if(trackedTransform == null)
         Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        _newPosition = transform.position;
        if(TrackX)
            _newPosition.x = trackedTransform.position.x;
        if(TrackY)
            _newPosition.y = trackedTransform.position.y;
        if(TrackZ)
            _newPosition.z = trackedTransform.position.z;
        transform.position = _newPosition;
    }
}
