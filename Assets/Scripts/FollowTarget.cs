using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
  public Transform target;
  public float smoothSpeed = 20f;
  private Vector3 offset = new Vector3(0f,0f,-1f);

  void FixedUpdate() {
    Vector3 desiredPosition = target.position + offset;
    Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);
    transform.position = smoothedPosition;
  }
}
