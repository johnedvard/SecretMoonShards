using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlayerMovement : MonoBehaviour
{
  public float moveSpeed = 50f;
  public float teleportSpeed = 100f;
  private Vector2 movement;
  private Rigidbody2D rb;
  private Rigidbody2D shardRB;
  private GameObject shardGameObject;
  private TrailRenderer trail;
  private Animator animator;
  public SkeletonAnimation skeletonAnimation;
  public AnimationReferenceAsset idle, charging, shoot, landed;
  public string currentShardState = "Idle";
  public string currentPlayerState = "Idle";
  public float shotSpeed = 100f;
  public float maxChargeTime = 2f;
  private float chargingTime = 1f;
  private float teleportTime = 0.1f;
  private float maxTeleportTime = 0.1f;
  private bool canChangeCharacter = false;
  private Vector3 originalScale;
  private Vector2 mousePos;
  public GameObject[] spawnPoints;
  public Vector3 shardVelocity;
  public int selectedAnimatorIndex; 
  public RuntimeAnimatorController[] animatorCtrls;
  
  void OnEnable() {
    Hole.OnFallInHole += FallInHole;
    WMBroadcaster.OnMonetizationStart += OnMonetizationStart;
    WMBroadcaster.OnMonetizationProgress += OnMonetizationProgress;
  }
  void OnDisable() {
    Hole.OnFallInHole -= FallInHole;
    WMBroadcaster.OnMonetizationStart -= OnMonetizationStart;
    WMBroadcaster.OnMonetizationProgress -= OnMonetizationProgress;
  }
   
  void Start() {
    originalScale = transform.localScale;
    rb = gameObject.GetComponent<Rigidbody2D>();
    trail = gameObject.GetComponent<TrailRenderer>();
    trail.emitting  = false;
    shardGameObject = gameObject.transform.GetChild(0).gameObject;
    shardRB = shardGameObject.GetComponent<Rigidbody2D>();
    animator = gameObject.GetComponent<Animator>();
    SetShardState(currentShardState);

  }

  void Update() {
    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    UpdatePlayer();
    if(currentPlayerState.Equals("Falling")){
      // Nothing
    } else {
      ShardControls();
      ChargeShot();
    }
    ChangeSkinControls();
  }

  void ChangeSkinControls(){
    if (Input.GetKeyDown("space")) {
      if(canChangeCharacter) {
        selectedAnimatorIndex++;
        if(selectedAnimatorIndex >= animatorCtrls.Length) {
          selectedAnimatorIndex = 0;
        }
        animator.runtimeAnimatorController = animatorCtrls[selectedAnimatorIndex];
      }
    }
  }

  void UpdatePlayer(){
    if(currentPlayerState.Equals("Teleport")){
      teleportTime += Time.deltaTime;
      if(teleportTime >= maxTeleportTime) {
        SetPlayerState("TeleportEnd");
      }
    } else if (currentPlayerState.Equals("Falling")){
      animator.SetFloat("Horizontal", 0);
      animator.SetFloat("Vertical", 0);
      animator.SetFloat("Speed", 0);
      if(transform.localScale.x >= 0) {
        transform.localScale -= new Vector3(10f,10f,0f)*Time.deltaTime;
      }else{
        SetPlayerState("Idle");
        transform.localScale = originalScale;
        transform.position = spawnPoints[0].transform.position;
      }
    }else{
      movement.x = Input.GetAxisRaw("Horizontal");
      movement.y = Input.GetAxisRaw("Vertical");

      animator.SetFloat("Horizontal", movement.x);
      animator.SetFloat("Vertical", movement.y);
      animator.SetFloat("Speed", movement.sqrMagnitude);
      if(movement.magnitude>0){
        SetPlayerState("Walking");
      } else {
        SetPlayerState("Idle");
      }
    }
  }
  void ChargeShot(){
    if(currentShardState.Equals("Charge")){
      chargingTime += Time.fixedDeltaTime;
      if(chargingTime >= maxChargeTime) {
        chargingTime = maxChargeTime;
      }
    } else {
      chargingTime = 1f;
    }
    if(shardRB.velocity.magnitude <= 0.5f && currentShardState.Equals("Shoot")) {
      SetShardState("Landed");
    }
  }

  void FixedUpdate()
  {
    if(currentPlayerState.Equals("Falling")) return;
    rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    if(currentShardState.Equals("Idle") || currentShardState.Equals("Charge")){
      shardRB.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
      Vector2 lookDir = mousePos - shardRB.position;
      float angle = Mathf.Atan2(lookDir.y,lookDir.x) * Mathf.Rad2Deg;
      shardRB.rotation = angle;
    }
    shardVelocity = shardRB.velocity;
  }
  
  private void ShardControls(){
    if (Input.GetMouseButtonDown(0)){
        SetShardState("Charge");
    }
    if (Input.GetMouseButtonUp(0)){
      SetShardState("Shoot");
    }
    if (Input.GetMouseButtonDown(1)){
      if(currentShardState.Equals("Shoot") || currentShardState.Equals("Landed")) {
        TeleportPlayer();
      }
    }
  }

  private void TeleportPlayer(){
    SetPlayerState("Teleport");
    Vector3 teleportStartPos = rb.position;
    rb.position = shardRB.position;
    teleportTime = 0f;
    CheckIfTeleportedThroughSomething(teleportStartPos, shardRB.position);
  }
  void CheckIfTeleportedThroughSomething(Vector3 startPos, Vector3 endPos){
    Debug.Log(LayerMask.NameToLayer("Enemy"));
    int layerMask = 1 << LayerMask.NameToLayer("Enemy");
    RaycastHit2D hit = Physics2D.CircleCast(startPos, 10, endPos-startPos, Vector3.Distance(endPos, startPos),layerMask );
    
    if (hit.transform != null) {
      GameObject hitObject = hit.transform.gameObject;
      Killable killable = hitObject.GetComponent<Killable>();
      Debug.Log("hit object" + hitObject);
      if(killable && killable.IsKillable()){
        killable.Kill();
        CheckIfTeleportedThroughSomething(hit.transform.position, endPos);
      }
    } else {
      Debug.Log("Nothing was hit");
    }
  }

  private void SetShardAnimation(AnimationReferenceAsset animation, bool loop, float timeScale) {
    skeletonAnimation.state.SetAnimation(0,animation,loop).TimeScale = timeScale;
  }

  private void SetPlayerState(string state){
    trail.emitting = false;
    if(!state.Equals(currentPlayerState)) {
      currentPlayerState = state;
    }
    if(state.Equals("Walking") || state.Equals("Idle")) {
    }else if(state.Equals("Teleport")) {
      trail.emitting = true;
    }else if(state.Equals("TeleportEnd")) {
      teleportTime = maxTeleportTime;
    }
  }

  private void SetShardState(string state) {
    TrailRenderer shardTrail = shardGameObject.GetComponent<TrailRenderer>();
    if(!state.Equals(currentShardState)) {
      currentShardState = state;
    }
    if(!state.Equals("Idle")){
      shardGameObject.GetComponent<Renderer>().enabled = true;
    }
    if (state.Equals("Idle")) {
      shardGameObject.GetComponent<Renderer>().enabled = false;
    } else if (state.Equals("Charge")) {
      SetShardAnimation(shoot,false,1f);
    } else if (state.Equals("Shoot")) {
      var trackEntry = skeletonAnimation.state.SetEmptyAnimation(0,0f);
      trackEntry.MixDuration = 0.2f; // smooth transition between animations
      shardRB.AddForce((mousePos - rb.position) * shotSpeed * chargingTime);
    } else if (state.Equals("Landed")) {
      SetShardAnimation(landed,true,0.5f);
    }
  }

 void OnTriggerEnter2D(Collider2D other) { // We use two box colliders, one is for trigger only, the other for collisions
    if(other.gameObject == shardGameObject) {
      if(currentShardState.Equals("Landed")) {
        SetShardState("Idle");
      }
    }
  }

  void OnCollisionEnter2D(Collision2D other) { // We cannot use the Physics2D collision matrix if we want to use triggerEnter.
    if(other.gameObject == shardGameObject) {
      Physics2D.IgnoreCollision( other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
  }

  public void FallInHole() {
    Debug.Log("FallInHole");
    SetPlayerState("Falling");
  }

  void OnMonetizationStart(Dictionary<string, object> detail)
  {
    Debug.Log("MonetizationProgres in player");
  }

  void OnMonetizationProgress(Dictionary<string, object> detail)
  {
    Debug.Log("MonetizationProgres in player");
    canChangeCharacter = true;
  }
}
