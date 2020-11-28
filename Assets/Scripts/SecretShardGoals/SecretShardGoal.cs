using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretShardGoal : MonoBehaviour
{
  public int enemiesToKill = 1;
  private int enemiesKilled = 0;
  public float killAllWithinSeconds = Mathf.Infinity;
  private float timeEllapsedSinceLastKilledEnemy = 0f;
  public Camera mainCamera;
  private int secretShards = 1;
  private int secretShardsFound = 0;
  public Vector3 offset = new Vector3(-80, 60, 1);
  public float smoothSpeed = 20f;
  private string missionState = "InProgress";
  public AudioClip shardFoundClip;
  public AudioClip missionFailedClip;
  AudioSource audioSource;

  void Start(){
    audioSource = GetComponent<AudioSource>();
  }
  
  void OnEnable() {
    Killable.OnEnemyKilled += EnemyKilled;
  }
  void OnDisable() {
    Killable.OnEnemyKilled -= EnemyKilled;
  }

  // Update is called once per frame
  void Update()
  {
    if(timeEllapsedSinceLastKilledEnemy == 0 || missionState.Equals("Failed") || missionState.Equals("Complete")) return;
    timeEllapsedSinceLastKilledEnemy += Time.deltaTime;
    if(timeEllapsedSinceLastKilledEnemy < killAllWithinSeconds) {
      if(enemiesKilled == enemiesToKill && secretShardsFound < secretShards) {
        Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
        tmp.a = 1f;
        tmp.r = 1f;
        tmp.g = 0.5f;
        tmp.b = 0.8f;
        gameObject.GetComponent<SpriteRenderer>().color = tmp;
        secretShardsFound++;
        audioSource.PlayOneShot(shardFoundClip, 1f);
        missionState = "Complete";
      }
    } else {
      Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
      tmp.a = 1f;
      tmp.r = 0.1f;
      tmp.g = 0.1f;
      tmp.b = 0.1f;
      gameObject.GetComponent<SpriteRenderer>().color = tmp;
      audioSource.PlayOneShot(missionFailedClip, 1f);
      missionState = "Failed";
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
    if(enemiesKilled <= 1 || timeEllapsedSinceLastKilledEnemy <= 0){
      timeEllapsedSinceLastKilledEnemy = 0.01f;
    } 
  }
}
