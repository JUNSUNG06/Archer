using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    static public UIManager Instance;

    Canvas canvas;

    PlayerStats playerStats;
    PlayerController playerController;
    BossHp bossHp;

    Slider playerHpSlider; 
    Slider playerStaminaSlider;
    Slider playerPowerSlider;
    Slider bossHpSlider;

    RectTransform playerPowerSliderTrm;

    Transform playerTrm;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        canvas = GameObject.FindWithTag("Canvas").GetComponent<Canvas>();

        playerHpSlider = GameObject.Find("PlayerHpSlider").GetComponent<Slider>();
        playerStaminaSlider = GameObject.Find("PlayerStaminaSlider").GetComponent<Slider>();
        playerPowerSlider = GameObject.Find("PlayerPowerSlider").GetComponent<Slider>();
        bossHpSlider = GameObject.Find("BossHpSlider").GetComponent<Slider>();

        playerPowerSliderTrm = GameObject.Find("PlayerPowerSlider").GetComponent<RectTransform>();
        playerTrm = GameObject.Find("Player").GetComponent<Transform>();

        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        bossHp = GameObject.Find("Boss").GetComponent<BossHp>();

        DisapperUI("PlayerPowerSlider");
    }

    private void Update()
    {
        playerHpSlider.value = playerStats.Hp / playerStats.MaxHp;
        playerStaminaSlider.value = playerStats.Stamina / playerStats.MaxStamina;
        playerPowerSlider.value = playerController.Power / playerController.MaxPower;
        bossHpSlider.value = bossHp.Hp / bossHp.MaxHp;       

        Vector2 Pos = new Vector2 (playerTrm.position.x, playerTrm.position.y + 1.2f);
        playerPowerSliderTrm.position = Camera.main.WorldToScreenPoint(Pos);
    }

    public void DisapperUI(string uiName)
    {
        GameObject ui = canvas.transform.Find(uiName).gameObject;
        ui.SetActive(false);
    }

    public void ApperUI(string uiName)
    {
        GameObject ui = canvas.transform.Find(uiName).gameObject;
        ui.SetActive(true);
    }
}
