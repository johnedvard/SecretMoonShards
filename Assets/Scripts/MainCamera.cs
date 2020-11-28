using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
  public AudioClip killClip;
  AudioSource audioSource;
  // Start is called before the first frame update
  void Start()
  {
    audioSource = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update()
  {
  }

  void OnEnable()
  {
    WMBroadcaster.OnMonetizationStart += OnMonetizationStart;
    WMBroadcaster.OnMonetizationProgress += OnMonetizationProgress;
    Killable.OnEnemyKilled += EnemyKilled;
  }

  // unregister events that you've registered
  void OnDisable()
  {
    WMBroadcaster.OnMonetizationStart -= OnMonetizationStart;
    WMBroadcaster.OnMonetizationProgress -= OnMonetizationProgress;
    Killable.OnEnemyKilled -= EnemyKilled;
  }

  // A monetization start event should occur roughly after a second or two after your game loads as WebGL.
  void OnMonetizationStart(Dictionary<string, object> detail)
  {
    // these are the parameters that you can read from the detail dictionary.
    // recommended: wrap parsing of each of these values in a try/catch in case the spec changes.
    // https://coil.com/docs/#browser-start

    string requestId = detail["requestId"] as string;
    string id = detail["id"] as string;
    string resolvedEndpoint = detail["resolvedEndpoint"] as string;
    string metaContent = detail["metaContent"] as string;
  }

  // A monetization progress event should occur roughly every two seconds after the monetization progress occurs
  void OnMonetizationProgress(Dictionary<string, object> detail)
  {
    // these are the parameters that you can read from the detail dictionary.
    // recommended: wrap parsing of each of these values in a try/catch in case the spec changes.
    // https://coil.com/docs/#browser-progress

    string amount = detail["amount"] as string;
    long amountAsLong = Convert.ToInt64(amount);
    string assetCode = detail["assetCode"] as string;
    long scale = (long)detail["assetScale"];
  }

  public void EnemyKilled(){
    audioSource.PlayOneShot(killClip, 1f);
  }
}
