using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Star : MonoBehaviour
{

  public Target target;
  public ParticleSystem explosion;
  public ParticleSystem sparkles;
  public SpriteRenderer sprite;

  public BoxCollider2D collider;

  public AudioSource explosionSound;
  public AudioSource sparkleSound;


  public float explosionForce;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (target.hit && sprite.enabled)
    {
      sparkles.Stop();
      sparkleSound.Stop();
      explosionSound.Play();
      explosion.Play();
      sprite.enabled = false;
      collider.enabled = false;
      foreach (var target in GameObject.FindGameObjectsWithTag("App").Select(x => x.GetComponent<Target>())) {
        target.Hit(true);
        target.rb.AddForce((target.transform.position - transform.position).normalized * explosionForce, ForceMode2D.Impulse);
      }
    }
  }
}
