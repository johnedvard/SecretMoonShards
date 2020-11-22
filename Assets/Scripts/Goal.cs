using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public delegate void GoalAction(); 
    public static event GoalAction OnGoal;
    void OnTriggerEnter2D(Collider2D other)
    {
      Debug.Log("Collision");
      if(other.gameObject.tag == "Player") {
        if(OnGoal != null) {
          OnGoal();
          Debug.Log("OnGoal plz");
        }
      }
    }
}
