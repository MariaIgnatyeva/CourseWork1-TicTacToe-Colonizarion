using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    public Button button;

    private void Start()
    {
        button.onClick.AddListener(GameManager.instance.Restart);
	}
}
