using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
    public GameObject gamePanel;
    public GameObject menuPanel;

    public GameObject gameoverPanel;
    public GameObject pausePanel;

    public Image timerImage;
    public Image timerImg;
    public Text timerText;
    private float currentTimeTimer = 0;
    public float maxTimeTimer;

    private int citizensRecruited;
    public Text citizensAmountText;
    private int citizensAmount = 0;
    public Image citizensDelayLine;
    public Button addCitizensButton;
    public Text addCitizensText;
    public float delayAddCitizensTimer;
    private float currentAddCitizensTime = 0;

    private int soldiersRecruited;
    public Text soldiersAmountText;
    private int soldiersAmount = 0;
    public Image soldiersDelayLine;
    public Button addSoldiersButton;
    public Text addSoldiersText;
    public float delayAddSoldiersTimer;
    private float currentAddSoldiersTime = 0;

    private int totalFood;
    private int totalFoodEaten;
    public Text foodAmountText;
    public Text foodProfitText;
    public GameObject FoodProfitSign;
    public Text foodProfitSignText;
    private int foodAmount = 0;
    private int foodProfit;

    private int enemiesDefeated;
    private int enemyAmount;
    public Text enemyAmountText;
    public Text amountWavesText;
    private int wave = 0;
    public int wavesNoAttack;
    int random;

    public Button boostCitizensButton;
    public int costBoostCitizens;
    public Text costBoostCitizensText;
    private int boostCitizens = 1;

    public Button boostSoldiersButton;
    public int costBoostSoldiers;
    public Text costBoostSoldiersText;
    private int boostSoldiers = 1;

    public Button pauseButton;
    private bool paused;

    public Button restartButton;
    public Text statisticText;
    public Text resultGameText;

    private bool mute;
    public Button muteButton;
    public Image muteButtonImage;
    private new AudioSource audio;

    public AudioClip click;
    public AudioClip ready;
    public AudioClip win;
    public AudioClip defeat;
    public AudioClip attackSound;
    public AudioClip pauseOff;
    public AudioClip pauseOn;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        gamePanel.SetActive(false);
        timerImage = GetComponent<Image>();
        gameoverPanel.SetActive(false);
        pausePanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    private void timerTick()
    {
        if (currentTimeTimer > 0)
        {
            currentTimeTimer -= Time.deltaTime;
            timerImg.fillAmount = currentTimeTimer / maxTimeTimer;
            timerText.text = (System.Math.Round(currentTimeTimer, 0)).ToString();
        }
        else if (currentTimeTimer <= 0)
        {
            currentTimeTimer = maxTimeTimer;
            timerImg.fillAmount = currentTimeTimer / maxTimeTimer;
            timerText.text = maxTimeTimer.ToString();
        }
    }

    private void attack()
    {
        random = Random.Range(3, 6);
        if (currentTimeTimer == maxTimeTimer)
        {
            if (wavesNoAttack == 0)
            {
                if (wave > 0)
                {
                    if (soldiersAmount >= enemyAmount)
                    {
                        enemiesDefeated += enemyAmount;
                    }
                    else
                    {
                        enemiesDefeated += soldiersAmount;
                    }
                    soldiersAmount -= enemyAmount;
                }

                if (mute == false)
                {
                    audio.clip = attackSound;
                    audio.Play();
                }
                enemyAmount = System.Convert.ToInt32(Mathf.Round(enemyAmount * 1.15f + random));
                wave++;
                amountWavesText.text = wave.ToString();
            }
            else
            {
                wavesNoAttack--;
            }
        }
    }

    private void addCitizensTick()
    {
        if (currentAddCitizensTime < delayAddCitizensTimer && addCitizensButton.interactable == false) // Создание жителей
        {
            currentAddCitizensTime += Time.deltaTime;
            citizensDelayLine.fillAmount = currentAddCitizensTime / delayAddCitizensTimer;
        }
        else if (currentAddCitizensTime >= delayAddCitizensTimer && addCitizensButton.interactable == false) // Житель нанят
        {
            currentAddCitizensTime = 0;
            citizensDelayLine.fillAmount = 1;

            if (mute == false)
            {
                audio.clip = ready;
                audio.Play();
            }
            addCitizensButton.interactable = true;
            citizensAmount += boostCitizens;
            citizensRecruited += boostCitizens;
        }
    }

    private void addSoldiersTick()
    {
        if (currentAddSoldiersTime < delayAddSoldiersTimer && addSoldiersButton.interactable == false) // Создание воинов
        {
            currentAddSoldiersTime += Time.deltaTime;
            soldiersDelayLine.fillAmount = currentAddSoldiersTime / delayAddSoldiersTimer;
        }
        else if (currentAddSoldiersTime >= delayAddSoldiersTimer && addSoldiersButton.interactable == false) // Воин нанят
        {
            currentAddSoldiersTime = 0;
            soldiersDelayLine.fillAmount = 1;

            if (mute == false)
            {
                audio.clip = ready;
                audio.Play();
            }
            addSoldiersButton.interactable = true;
            soldiersAmount += boostSoldiers;
            soldiersRecruited += boostSoldiers;
        }
    }

    private void foodTick()
    {
        foodProfit = citizensAmount * 4 - soldiersAmount * 6;
        if (currentTimeTimer == maxTimeTimer)
        {
            foodAmount += foodProfit;
            totalFood += citizensAmount * 4;
            totalFoodEaten += soldiersAmount * 6;
        }
    }

    private void interfaceTick()
    {
        if (foodProfit <= 0)
        {
            FoodProfitSign.SetActive(false);
            foodProfitText.color = new Color(0.8f, 0.3f, 0.3f);
        }
        else
        {
            FoodProfitSign.SetActive(true);
            foodProfitText.color = new Color(0.3f, 0.8f, 0.3f);
            foodProfitSignText.color = new Color(0.3f, 0.8f, 0.3f);
        }

        if (enemyAmount > soldiersAmount)
        {
            enemyAmountText.color = new Color(0.8f, 0.3f, 0.3f);
            soldiersAmountText.color = new Color(0.8f, 0.3f, 0.3f);
        }
        else
        {
            enemyAmountText.color = new Color(0.3f, 0.8f, 0.3f);
            soldiersAmountText.color = new Color(0.3f, 0.8f, 0.3f);
        }

        costBoostCitizensText.text = costBoostCitizens.ToString();
        costBoostSoldiersText.text = costBoostSoldiers.ToString();
        enemyAmountText.text = enemyAmount.ToString();
        foodAmountText.text = foodAmount.ToString();
        foodProfitText.text = foodProfit.ToString();
        citizensAmountText.text = citizensAmount.ToString();
        soldiersAmountText.text = soldiersAmount.ToString();
    }

    public void clickOnPauseButton()
    {
        if (paused)
        {
            if (mute == false)
            {
                audio.clip = pauseOff;
                audio.Play();
            }
            Time.timeScale = 1;
            paused = false;
            pausePanel.SetActive(false);
        }
        else
        {
            if (mute == false)
            {
                audio.clip = pauseOn;
                audio.Play();
            }
            Time.timeScale = 0;
            paused = true;
            pausePanel.SetActive(true);
        }
    }

    private void resultDefeatGame()
    {
        if (wave <= 1)
            wave += 1;
        gameoverPanel.SetActive(true);
        resultGameText.text = "Поражение";
        statisticText.text = $"{wave - 2}\n{enemiesDefeated}\n{soldiersRecruited}\n{citizensRecruited}\n{totalFood}\n{totalFoodEaten}";
    }

    private void resultWinGame()
    {
        gameoverPanel.SetActive(true);
        resultGameText.text = "Победа";
        statisticText.text = $"{wave - 1}\n{enemiesDefeated}\n{soldiersRecruited}\n{citizensRecruited}\n{totalFood}\n{totalFoodEaten}";
    }

    private void checkBoostButtonTick()
    {
        if (foodAmount >= costBoostCitizens)
        {
            boostCitizensButton.interactable = true;
        }
        else
        {
            boostCitizensButton.interactable = false;
        }

        if (foodAmount >= costBoostSoldiers)
        {
            boostSoldiersButton.interactable = true;
        }
        else
        {
            boostSoldiersButton.interactable = false;
        }
    }

    public void clickSound()
    {
        if (mute == false)
        {
            audio.clip = click;
            audio.Play();
        }
    }

    public void clickOnMuteButton()
    {
        if (mute == false)
        {
            mute = true;
            muteButtonImage.color = new Color(0.7f, 0.2f, 0.2f);
        }
        else
        {
            mute = false;
            audio.clip = click;
            audio.Play();
            muteButtonImage.color = new Color(0.2f, 0.7f, 0.2f);

        }
    }

    public void ClickOnAddCitizensButton()
    {
        if (mute == false)
        {
            audio.clip = click;
            audio.Play();
        }
        addCitizensButton.interactable = false;
    }

    public void ClickOnAddSoldiersButton()
    {
        if (mute == false)
        {
            audio.clip = click;
            audio.Play();
        }
        addSoldiersButton.interactable = false;
    }

    public void ClickOnBoostCitizensButton()
    {
        if (mute == false)
        {
            audio.clip = click;
            audio.Play();
        }
        foodAmount -= costBoostCitizens;
        costBoostCitizens *= 3;
        boostCitizens *= 2;
        addCitizensText.text = boostCitizens.ToString();
    }

    public void ClickOnBoostSoldiersButton()
    {
        if (mute == false)
        {
            audio.clip = click;
            audio.Play();
        }
        foodAmount -= costBoostSoldiers;
        costBoostSoldiers *= 3;
        boostSoldiers *= 2;
        addSoldiersText.text = boostSoldiers.ToString();
    }

    public void clikOnExitMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private bool resultMusic = true;

    void Update()
    {
        if (gamePanel.activeSelf == true)
        {
            if (foodAmount < 0 || soldiersAmount < 0)
            {
                resultDefeatGame();
                if (mute == false && resultMusic)
                {
                    audio.clip = defeat;
                    audio.Play();
                    resultMusic = false;
                }
            }
            else if (wave > 25)
            {
                resultWinGame();
                if (mute == false && resultMusic)
                {
                    audio.clip = win;
                    audio.Play();
                    resultMusic = false;
                }
            }
            else
            {
                checkBoostButtonTick();
                timerTick();
                foodTick();
                addCitizensTick();
                addSoldiersTick();
                interfaceTick();
                attack();
            }
        }
    }
}
