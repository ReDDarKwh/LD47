using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Linq;

public class SceneManager : MonoBehaviour
{
  public float gameTime;
  public float roundDiffAddition;
  public int numRounds;
  public float startDifficulty;

  public float progress;
  public float difficulty;
  public int round = 0;

  public float maxHealth;

  public float health;
  private float roundStart;

  public float spawnInterval;

  private float lastSpawn = 0;

  public GameObject[] targetObjects;

  public Slider slider;

  public Gradient sliderColor;

  public Image healthbarFill;

  public AudioSource targetSpawnSound;

  public Image guyBubble;
  public Image conscienceBubble;

  public Text guyBubbleText;
  public Text conscienceBubbleText;

  [TextArea(5, 20)]
  public string[] guyTextRoundStart;

  [TextArea(5, 20)]
  public string[] guyTextRoundEnd;

  [TextArea(5, 20)]
  public string[] conscienceText;
  private float time;

  private bool pause = true;

  public SpriteRenderer guySprite;

  public Sprite guyOnPhone;
  public Sprite guyNotPhone;

  public Text roundText;
  public Text roundTextWhite;
  public Slider roundProgress;

  public Text scoreText;
  public Text comboText;
  public Text comboModifierText;

  internal float combo;
  private float score;
  internal float comboMod;
  private bool lose;
  private bool win;
  private GameController gameController;
  public bool endlessMode;

  public void AddToScore(float add) {
    score += add + add * comboMod;
  }

  // Start is called before the first frame update
  void Start()
  {
    health = maxHealth;
    roundStart = Time.time;
    slider.maxValue = maxHealth;
    slider.value = maxHealth;
    healthbarFill.color = sliderColor.Evaluate(1f);

    guyBubble.gameObject.SetActive(false);
    conscienceBubble.gameObject.SetActive(false);

    gameController = GameObject.FindGameObjectWithTag("GameController")?.GetComponent<GameController>();
    endlessMode = gameController?.endless ?? false;

    Pause();
  }

  public void TakeDamage(float damage)
  {
    health -= damage;
    slider.value = health;
    healthbarFill.color = sliderColor.Evaluate(slider.normalizedValue);

    if (health < 0)
    {
      Lose();
    }
  }

  private void SetScore() {
    gameController.score = Mathf.Round(score);
  }

  private void Lose()
  {
    if (!lose && !win) { 
      UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(2);
      lose = true;
      SetScore();
    }
  }

  private void Win()
  {
    if (!lose && !win)
    { 
      SetScore();
      win = true;
      UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(3);
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (pause)
    {
    }
    else
    {
      comboMod = combo / 5;
      scoreText.text = $"SCORE: {Mathf.Round(score)}";
      comboText.text = $"COMBO X {combo}";
      comboModifierText.text = $"Score Modifier +{Mathf.Round(comboMod * 100)}%";

      time += Time.deltaTime;

      difficulty = startDifficulty + time / (gameTime * numRounds) * (1f - startDifficulty);

      roundProgress.value = (Time.time - roundStart) / gameTime;

      if ((Time.time - roundStart) / gameTime > 1)
      {
        Pause();
      }

      if (Time.time - lastSpawn > spawnInterval)
      {
        lastSpawn = Time.time;
        if (Random.value < difficulty)
        {
          var inst = Instantiate(targetObjects[Random.Range(0, targetObjects.Length)], transform.position, Quaternion.identity);
          targetSpawnSound.Play();
          inst.GetComponent<Target>().sceneController = this;
        }
      }
    }
  }

  private void UnPause()
  {
    round++;
    roundText.text = roundTextWhite.text = $"ROUND {round}/{(endlessMode? "infinity" : numRounds.ToString())}";
    roundStart = Time.time;
    pause = false;
    guySprite.sprite = guyOnPhone;
  }

  private void Pause()
  {
    pause = true;
    guySprite.sprite = guyNotPhone;

    foreach (var t in GameObject.FindGameObjectsWithTag("App").Select(x => x.GetComponent<Target>()))
    {
      t.Hit(false);
    };

    if (!endlessMode)
    {
      if (round >= numRounds)
      {
        Win();
      }
      else
      {
        StartCoroutine(dialog(guyTextRoundEnd[round], guyTextRoundStart[round], conscienceText[round]));
      }
    }
    else {
      UnPause();
    }
  }

  IEnumerator dialog(string roundEndGuy, string roundStartGuy, string roundStartConscience)
  {

    var charWait = 0.03f;

    guyBubble.gameObject.SetActive(true);
    var text = "";
    foreach (var character in roundEndGuy.ToCharArray())
    {
      text += character;
      guyBubbleText.text = text;
      yield return new WaitForSeconds(charWait);
    }
    yield return new WaitForSeconds(1);

    guyBubbleText.text = text = "";

    yield return new WaitForSeconds(1);

    foreach (var character in "...".ToCharArray())
    {
      text += character;
      guyBubbleText.text = text;
      yield return new WaitForSeconds(charWait);
    }

    yield return new WaitForSeconds(1);

    guyBubbleText.text = text = "";

    yield return new WaitForSeconds(1);

    foreach (var character in roundStartGuy.ToCharArray())
    {
      text += character;
      guyBubbleText.text = text;
      yield return new WaitForSeconds(charWait);
    }
    yield return new WaitForSeconds(1);

    guyBubble.gameObject.SetActive(false);
    conscienceBubble.gameObject.SetActive(true);
    guyBubbleText.text = text = "";

    foreach (var character in roundStartConscience.ToCharArray())
    {
      text += character;
      conscienceBubbleText.text = text;
      yield return new WaitForSeconds(charWait);
    }
    yield return new WaitForSeconds(1);

    conscienceBubble.gameObject.SetActive(false);

    UnPause();
  }

  
}
