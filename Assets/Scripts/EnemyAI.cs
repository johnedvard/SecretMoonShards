using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Animator animator;
    private Vector2 movement;
    private Rigidbody2D rb;
    private bool upwards = true;
    public float moveSpeed = 40f;
    public string currentState = "Idle";
    private string[] behaviours = new []{"UpDown", "LeftRight", "None"};
    public string behaviour = "UpDown";
    
    // Start is called before the first frame update
    void Start()
    {
      animator = gameObject.GetComponent<Animator>();
      rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
      if(behaviour.Equals("UpDown")) {
        movement.y = upwards ? 1 : -1;
      }
      
      
      animator.SetFloat("Horizontal", movement.x);
      animator.SetFloat("Vertical", movement.y);
      animator.SetFloat("Speed", movement.sqrMagnitude);
      SimulateInput();
    }
    private void FixedUpdate() {
      rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
    void SimulateInput(){

    }
    private void OnCollisionEnter2D(Collision2D other) {
      if(other.gameObject.tag == "Wall"){
        upwards = !upwards;
      }
    }
}
