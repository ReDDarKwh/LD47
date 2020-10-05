using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

  public Rigidbody2D rb;
  public float blastForce;
  internal Vector3 headPos;

  public float maxDis;
  public float minDis;

  public float maxRotationSpeed;
  public float minRotationSpeed;

  private float rotationSpeed;
  private bool clockwise;
  private float dis;

  private float angle;
  public bool hit;
  private float bottom = -10;
  internal SceneManager sceneController;

  public AudioSource hitSound;


  public float damage;

  // Start is called before the first frame update
  void Start()
  {
    dis = Random.Range(minDis, maxDis);
    clockwise = Random.value > 0.5;
    rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
    angle = Random.value * (Mathf.PI * 2);
    headPos = transform.position;

    transform.position = headPos + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * dis;
    
  }

  private void Update()
  {
    if (transform.position.y < bottom)
    {
      Destroy(gameObject);
    }

    if (hit) return;
    sceneController.TakeDamage(damage * Time.deltaTime);
  }

  private void FixedUpdate()
  {
    if (hit) return;
    angle += (clockwise ? -rotationSpeed : rotationSpeed) * Time.fixedDeltaTime;
    rb.MovePosition(headPos + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * dis);
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    Hit(true);
    rb.AddForce((transform.position - collision.collider.transform.position).normalized * blastForce, ForceMode2D.Impulse);
    hitSound.Play();
    collision.gameObject.GetComponent<Projectile>().hasHit = true;
  }

  public void Hit(bool countsInScore) {

    if (!hit && countsInScore) {
      sceneController.AddToScore(1);
      sceneController.combo++;
    }
    rb.gravityScale = 1;
    hit = true;
  }

}
