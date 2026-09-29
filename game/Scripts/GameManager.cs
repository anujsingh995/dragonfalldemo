using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Shows the Winner screen when a dragon dies and restarts the scene.</summary>
public class GameManager : MonoBehaviour
{
    public Health player;
    public Health enemy;
    public GameObject winnerPanel;
    public TMP_Text winnerText;

    void Start()
    {
        Time.timeScale = 1f;
        winnerPanel.SetActive(false);

        player.OnDied += HandleDeath;
        enemy.OnDied += HandleDeath;
    }

    void HandleDeath(Health dead)
    {
        Health winner = dead == player ? enemy : player;
        winnerText.text = winner.name + " Wins!";
        winnerPanel.SetActive(true);
        Time.timeScale = 0f; // freeze the game
    }

    // Hook this to the Restart button's OnClick
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
