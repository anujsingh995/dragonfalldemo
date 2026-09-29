using UnityEngine;
using UnityEngine.UI;

public class WinnerButtonBinder : MonoBehaviour
{
    public GameObject panel;
    GameManager manager;

    void Start()
    {
        manager = FindAnyObjectByType<GameManager>();
        Button button = GetComponent<Button>();
        if (button != null && manager != null)
            button.onClick.AddListener(manager.Restart);
    }
}
