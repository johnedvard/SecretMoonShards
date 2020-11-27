using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shard : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 shardVelocity;
    private float maxSpeed = 100f;
    public float squashMultiplier = 0.35f;
    public float size = 1f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float mag = rb.velocity.magnitude;
        float squash = Mathf.InverseLerp(0, maxSpeed, mag) * squashMultiplier;
        gameObject.transform.localScale = new Vector3(size + squash, size - squash, 1);
    }
    private void FixedUpdate() {
      shardVelocity = rb.velocity;
    }
    public Vector3 GetShardVelocity(){
      return shardVelocity;
    }
}
