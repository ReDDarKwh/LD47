using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
  public LineRenderer lineRenderer;
  private bool mouseDown;
  public int previewSteps;
  private Vector3 initialVel;

  public GameObject projectile;
  public float throwMultiplier;
  private Vector3 startPos;

  public Trajectory trajectory;

  public Transform bodyTop;

  public Transform spawnPoint;

  public AudioSource cannonSound;

  public ParticleSystem[] effects;

  // Start is called before the first frame update
  void Start()
  {
    lineRenderer.positionCount = previewSteps;
    lineRenderer.enabled = false;
  }

  // Update is called once per frame
  void Update()
  {

    if (Input.GetMouseButtonDown(0)) {
      mouseDown = true;
      lineRenderer.enabled = true;
      startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

      trajectory.Show();
    }

    if (Input.GetMouseButtonUp(0)) {
      mouseDown = false;
      lineRenderer.enabled = false;

      trajectory.Hide();

      cannonSound.Play();

      foreach (var e in effects) {
        e.Play();
      }

      var rb = Instantiate(projectile, spawnPoint.position, Quaternion.identity).GetComponent<Rigidbody2D>();
      rb.velocity = initialVel;
    }

    if (mouseDown) {

      
      var forceVec = startPos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
      var angle = (Mathf.Atan2(forceVec.y, forceVec.x) * Mathf.Rad2Deg + 360) % 360;

      if (angle > 90 && angle < 200) { 
        bodyTop.rotation = Quaternion.Euler(0, 0, angle + 180); ;
      }
      
      Debug.DrawLine(startPos, Camera.main.ScreenToWorldPoint(Input.mousePosition));

      initialVel = forceVec * throwMultiplier;

      trajectory.UpdateDots(spawnPoint.position, initialVel);

    }


  }
}
