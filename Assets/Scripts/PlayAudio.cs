using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    // Начало и конец игры.
    public AudioSource start, end;

    // Выбор клетки поля и хода на панели каждым игроком.
    public AudioSource field, cross, circle;
    
    public void StartAudio()
    {
        start.Play();
    }

    public void EndAudio()
    {
        end.Play();
    }

    public void FieldAudio()
    {
        field.Play();
    }

    public void PanelAudio()
    {
        if (Player.player.CrossTurn)
        {
            cross.Play();
        }
        else
        {
            circle.Play();
        }
    }
}
