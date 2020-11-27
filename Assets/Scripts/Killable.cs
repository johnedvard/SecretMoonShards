using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem ps;
    public delegate void EnemyKilledAction(); 
    public static event EnemyKilledAction OnEnemyKilled; 
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsKillable(){
      return true;
    }

    public void Kill(){
      Debug.Log("Kill");

      var spawnedPs = Instantiate(ps, transform.position, transform.rotation);
      if(OnEnemyKilled != null) OnEnemyKilled();
      Destroy(gameObject);
    }
}
