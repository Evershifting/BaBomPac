using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

public class UIManager : MonoBehaviour
{
    public Text foodText, lifeText, flameChargesText, restartTimerText;
    public Image flameIcon, shieldIcon;
    public GameObject gameOverPanel;
    [Inject]
    GameManager _gameManager;
    [Inject]
    Player _player;

    public void UpdateUI()
    {
        foodText.text = _gameManager.FoodAmount.ToString();
        lifeText.text = _gameManager.Life.ToString();
        flameChargesText.text = _player.FlameCharges.ToString();
        if (_player.FlameCharges <= 0)
        {
            flameIcon.enabled = false;
        }
        else
        {
            flameIcon.enabled = true;
        }
        shieldIcon.enabled = _player.Shielded;

    }
    public void GameOver()
    {
        if (!gameOverPanel.activeSelf)
        {
            gameOverPanel.SetActive(true);
            restartTimerText.text = "Press any key to restart";
        }
        else
        {
            gameOverPanel.SetActive(false);
        }
    }
}
