using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
  public delegate void FallInHoleAction(); 
  public static event FallInHoleAction OnFallInHole; 
  string currentState = "Idle";
  Collider2D playerCollider;
  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
  }

  void OnTriggerEnter2D(Collider2D other) {
    if(other.gameObject.tag == "Player") {
      if(OnFallInHole != null) OnFallInHole();
    }
  }
  void OnTriggerExit2D(Collider2D other) {
    if(other.gameObject.tag == "Player") {
    }
  }
  void OnTriggerStay2D(Collider2D other) {
    Debug.Log("Stay in holew" + other.gameObject.tag);
    if(other.gameObject.tag == "Player") {
      if(OnFallInHole != null) OnFallInHole();
    }
  }
}
