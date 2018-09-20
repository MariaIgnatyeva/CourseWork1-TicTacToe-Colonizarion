using UnityEngine;
using UnityEngine.UI;

public class RulesButton : MonoBehaviour
{
    // Кнопка "Правила".
    public Button button;

    // Экран с правилами.
    public GameObject rules;

    // Родитель кнопки button.
    [HideInInspector]
    public GameObject parent; 

    public void Start ()
    {
        parent = button.transform.parent.gameObject;
        button.onClick.AddListener(RulesButtonOnClick);
	}

    /// <summary>
    /// Нажата кнопка "Правила".
    /// </summary>
    private void RulesButtonOnClick()
    {
        parent.SetActive(false);
        rules.SetActive(true);

        // Кнопка "Назад".
        Button backButton = rules.transform.FindChild("Back").GetComponent<Button>(); 

        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(BackButtonOnClick);
    }

    /// <summary>
    /// Нажата кнопка "Назад" на экране с правилами.
    /// </summary>
    private void BackButtonOnClick()
    {
        rules.SetActive(false);
        parent.SetActive(true);
    }
}
