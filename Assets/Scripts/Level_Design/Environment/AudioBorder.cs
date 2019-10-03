using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBorder : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.transform.GetComponent<PlayerController>();
        if (player)
        {
            if (player.ReturnDashState())
            {
                AudioManager.Instance.PlaySFX(SFXToPlay.Hit_Bord_Dash);
            }
            else
                AudioManager.Instance.PlaySFX(SFXToPlay.Hit_Bord);
        }
    }
}
