using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretShardGoal : MonoBehaviour
{
  public int enemiesToKill = 1;
  private int enemiesKilled = 0;
  public Camera mainCamera;
  private int secretShards = 1;
  private int secretShardsFound = 0;
  public Vector3 offset = new Vector3(-80, 60, 1);
  public float smoothSpeed = 20f;
  
  void OnEnable() {
    Killable.OnEnemyKilled += EnemyKilled;
  }
  void OnDisable() {
    Killable.OnEnemyKilled -= EnemyKilled;
  }

  // Update is called once per frame
  void Update()
  {
    if(enemiesKilled == enemiesToKill && secretShardsFound < secretShards) {
      Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
      tmp.a = 255f;
      gameObject.GetComponent<SpriteRenderer>().color = tmp;
      secretShardsFound++;
    }
  }

  private void FixedUpdate() {
    Vector3 desiredPosition = mainCamera.transform.position + offset;
    Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);
    transform.position = smoothedPosition;
  }
  public int GetSecretSharsds(){
    return secretShards;
  }
  public void EnemyKilled(){
    enemiesKilled ++;
  }
}
