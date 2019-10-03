using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{

    public SFXToPlay SfxToPlay;

    public void PlaySound()
    {
        AudioManager.Instance.PlaySFX(SfxToPlay);
    }
}
