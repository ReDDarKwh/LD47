using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextScene : MonoBehaviour
{

  public Text score;
  public bool endlessMode = false;
  private GameController g;

  // Start is called before the first frame update
  void Start()
  {
    g = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

    if (score != null) { 
      score.text = $"Final Score: {g.score}";
    }
   
  }

  // Update is called once per frame
  void Update()
  {

  }


  public void Play()
  {
    g.endless = endlessMode;
    UnityEngine.SceneManagement.SceneManager.LoadScene(1);
  }

}
