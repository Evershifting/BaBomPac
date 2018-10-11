using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

public class UIManager : MonoBehaviour
{
    public Text foodText, lifeText, flameChargesText, restartTimerText, gameOverText;
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
        if (_player.FlameCharges <= 0)
        {
            flameChargesText.gameObject.SetActive(false);
            flameIcon.gameObject.SetActive(false);
        }
        else
        {
            flameChargesText.gameObject.SetActive(true);
            flameIcon.gameObject.SetActive(true);
            flameChargesText.text = "x" + _player.FlameCharges.ToString();
        }
        shieldIcon.gameObject.SetActive(_player.Shielded);

    }
    public void GameOver(bool gameWon)
    {
        if (!gameOverPanel.activeSelf)
        {
            gameOverPanel.SetActive(true);
            if (gameWon)
            {
                gameOverText.text = "Congratz! Game won!";
            }
            else
            {
                gameOverText.text = "Good news! You can try again!";
            }
            restartTimerText.text = "Press any key to restart";
        }
        else
        {
            gameOverPanel.SetActive(false);
        }
    }
}
