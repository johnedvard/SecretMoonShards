using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOff : MonoBehaviour
{
    private Vector3 oldShardVelocity; 
    
    void OnCollisionEnter2D(Collision2D other)
    {
      GameObject go = other.gameObject;
      if(go.tag == "Shard") {

        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        // Need the velocity before the collision physics is applied
        Vector3 shardVelocity = go.GetComponent<Shard>().GetShardVelocity();
        ContactPoint2D contact = other.contacts[0];
        Vector3 reflected = Vector3.Reflect(shardVelocity, contact.normal);
        rb.velocity = reflected;
        // rotate the object by the same ammount we changed its velocity
        Quaternion rotation = Quaternion.FromToRotation(shardVelocity, reflected);
        go.transform.rotation = rotation * go.transform.rotation;

      }
    }
}
