using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
  public delegate void GoalAction(); 
  public static event GoalAction OnGoal;
  public Animator transition;
  public float transitionTime = 1f;

  void Update(){
    GameControls();
  }

  void OnEnable() {
      Goal.OnGoal += ReachedGoal;
  }
  void OnDisable() {  
      Goal.OnGoal -= ReachedGoal;
  }

  public void LoadNextLevel(){
    StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
  }

  void GameControls(){
    if (Input.GetKeyDown("r")){
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
  }

  IEnumerator LoadLevel(int levelIndex) {
    transition.SetTrigger("Start");
    yield return new WaitForSeconds(transitionTime);

    SceneManager.LoadScene(levelIndex);
  }

  public void ReachedGoal() {
    LoadNextLevel();
  }
}
