using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public float moveSpeed = 5f;
  Vector2 movement;
  private Rigidbody2D rb;
  private Animator animator;
  // Start is called before the first frame update
  void Start()
  {
    rb = gameObject.GetComponent<Rigidbody2D>();
    animator = gameObject.GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update()
  {
    movement.x = Input.GetAxisRaw("Horizontal");
    movement.y = Input.GetAxisRaw("Vertical");

    animator.SetFloat("Horizontal",movement.x);
    animator.SetFloat("Vertical",movement.y);
    animator.SetFloat("Speed",movement.sqrMagnitude);
  }

  void FixedUpdate() 
  {
    rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
  }
}
