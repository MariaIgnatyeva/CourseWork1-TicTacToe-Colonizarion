using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    // Кнопки вкл/выкл звук на экранах главного меню и паузы.
    public Button buttonMM, buttonP;

    // Спрайты, меняющиеся при нажатии на кнопку.
    public Sprite mute, unmute;

    private void Start()
    {
        SetSprite(mute);
        buttonMM.onClick.AddListener(MuteOrUnmute);
        buttonP.onClick.AddListener(MuteOrUnmute);
    }

    /// <summary>
    /// Включает или выключает звуки в игре.
    /// </summary>
    private void MuteOrUnmute()
    {
        AudioListener.volume = 1 - AudioListener.volume;
        ChangeSprite();
    }

    /// <summary>
    /// Меняет спрайт обеих кнопок.
    /// </summary>
    private void ChangeSprite()
    {
        if (AudioListener.volume == 0)
        {
            SetSprite(unmute);
        }
        else
        {
            SetSprite(mute);
        }
    }

    /// <summary>
    /// Задает измененный спрайт кнопкам.
    /// </summary>
    /// <param name="image">спрайт для изменения</param>
    private void SetSprite(Sprite image)
    {
        buttonMM.image.sprite = image;
        buttonP.image.sprite = image;
    }
}
