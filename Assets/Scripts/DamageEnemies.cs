using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DamageEnemies : MonoBehaviour
{

    EnemyManager em;

    [SerializeField]
    private string collisionText = "Collision";

    [SerializeField]
    private string collisonTag = "Enemy";
     void Start()
    {
        em = FindObjectOfType<EnemyManager>();
    }
    void OnCollisionEnter2D(Collision2D collider)
    {
        Debug.Log(collisionText + " with " + collider.gameObject.name);
        if (collider.gameObject.CompareTag(collisonTag))
        {
            em.EnemyKilled(collider.gameObject);
        }
        //OnTriggerEnter2D: Owning object must be Kinematic, other collider must be "Trigger"
    }
}
