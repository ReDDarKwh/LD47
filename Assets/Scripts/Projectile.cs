using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  public float bottom;
  public bool hasHit;

  private SceneManager sceneManager;

  // Start is called before the first frame update
  void Start()
  {
    sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
  }

  // Update is called once per frame
  void Update()
  {
    if (transform.position.y < bottom)
    {
      if (!hasHit) {
        sceneManager.combo = 0f;
      }
      Destroy(gameObject);
    }
  }

}
