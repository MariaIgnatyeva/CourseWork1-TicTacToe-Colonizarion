using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    // Кнопка "Заново".
    public Button button;

    // Экран игры.
    public GameObject playScreen; 
    
    /// <summary>
    /// Запускает игру заново.
    /// </summary>
    private void Restart()
    {
        button.transform.parent.gameObject.SetActive(false);
        playScreen.SetActive(true);

        GameManager.instance.Restart();
    }
    
    private void Start()
    {
        button.onClick.AddListener(Restart);
    }
}
