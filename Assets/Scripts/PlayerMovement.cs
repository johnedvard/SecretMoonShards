using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlayerMovement : MonoBehaviour
{
  public float moveSpeed = 5f;
  private Vector2 movement;
  private Rigidbody2D rb;
  private Rigidbody2D shardRB;
  private GameObject shardGameObject;
  private Animator animator;
  public SkeletonAnimation skeletonAnimation;
  public AnimationReferenceAsset idle, charging, shoot, landed;
  public string currentShardState = "Idle";

  // Start is called before the first frame update
  void Start()
  {
    rb = gameObject.GetComponent<Rigidbody2D>();
    shardGameObject = gameObject.transform.GetChild(0).gameObject;
    Debug.Log("shardGameObject");
    shardRB = shardGameObject.GetComponent<Rigidbody2D>();
    Debug.Log(shardRB);
    animator = gameObject.GetComponent<Animator>();
    setShardState(currentShardState);
  }

  // Update is called once per frame
  void Update()
  {
    movement.x = Input.GetAxisRaw("Horizontal");
    movement.y = Input.GetAxisRaw("Vertical");

    animator.SetFloat("Horizontal",movement.x);
    animator.SetFloat("Vertical",movement.y);
    animator.SetFloat("Speed",movement.sqrMagnitude);
    ShardControls();
  }

  void FixedUpdate() 
  {
    rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    if(currentShardState.Equals("Idle") || currentShardState.Equals("Charge")){
      shardRB.MovePosition((rb.position + new Vector2(0f,-0.25f)) + movement * moveSpeed * Time.fixedDeltaTime);
    }
  }
  private void ShardControls(){
    if (Input.GetMouseButtonDown(0)){
        setShardState("Charge");
    }
    if (Input.GetMouseButtonUp(0)){
        setShardState("Shoot");
    }
  }
  private void setShardAnimation(AnimationReferenceAsset animation, bool loop, float timeScale) {
    skeletonAnimation.state.SetAnimation(0,animation,loop).TimeScale = timeScale;
  }
  private void setShardState(string state) {
    if (state.Equals("Idle")) {
      setShardAnimation(idle,false,1f);
    } else if (state.Equals("Charge")) {
      setShardAnimation(shoot,false,1f);
    } else if (state.Equals("Shoot")) {
      skeletonAnimation.state.SetEmptyAnimation(0,0f);
      // setShardAnimation(idle,false,1f);
    }
  }
}
