using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Animator animator;
    public string currentState = "Idle";
    // Start is called before the first frame update
    void Start()
    {
      animator = gameObject.GetComponent<Animator>();
      animator.SetFloat("Horizontal", 0);
      animator.SetFloat("Vertical", 0);
      animator.SetFloat("Speed", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
