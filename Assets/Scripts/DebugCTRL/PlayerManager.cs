using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private int Controltype = 0;
    private bool canTriggerLTXBOX = true;
    private bool canTriggerRTXBOX = true;

    [Header("Sounds")]
    public GameObject GhostPower, GhostPowerEnd, ShieldPower, ShieldReturn, BoomerangPower, Maskchange, Death;

    [Header("Yes")]
    public GameObject DeathFX;
    private Transform PlayerSpawn;

    private Animator a_Animator;


    #region SKILLS_DATA

    [SerializeField] private GameObject BoomerangMask;  //
    [SerializeField] private GameObject ShieldMask;     //  Les refs des masques
    [SerializeField] private GameObject GhostMask;      //

    private int i_MaskFound = 0; // Nombre de masque trouvé (à augmenter quand on découvre un nouveau masque
    private int i_MaskIdx = -1; // Index du masque [-1 : Aucun | 0 : Boomerang | 1 : Shield | 2 : Ghost]

    private Transform t_SpawnPosition;

    private string F1, F2;
    public GameObject FireHeadBase;

    // BOOMERANG
    public GameObject BoomerangPrefab; // Le prefab du boomerang à spawn
    private bool b_CanUseBoomerang = true; // Est ce que l'on peut utiliser le boomerang (cooldown) 
    public GameObject FireHeadBoomerang;
    private GameObject clone;

    // SHIELD
    public GameObject ShieldPrefab; // Le prefab du shield à spawn
    private bool b_IsShieldSetup = false; // Est ce que le shield est setup
    private GameObject ShieldSetup = null; // Le shield qui est setup actuellement
    public GameObject FireHeadShield;

    // GHOST
    public GameObject GhostPrefab; // Le prefab du ghost à spawn;
    private bool b_IsInGhostMode = false; // Est ce que le joueur contrôle le ghost;
    private GameObject GhostSetup = null; // Le Ghost qui est setup actuellement
    public GameObject FireHeadGhost;

    #endregion

    #region LIFE_DATA

    private float f_MaxLife = 100f;
    private float f_PlayerLife;

    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
        f_PlayerLife = f_MaxLife;
        t_SpawnPosition = transform.GetChild(0);
        a_Animator = GetComponent<Animator>();
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Controltype == 0)
        {
            F1 = "F1_PC";
            F2 = "F2_PC";
        }
        else if (Controltype == 1)
        {
            F1 = "F1_PS4";
            F2 = "F2_PS4";
        }
        else if (Controltype == 2)
        {
            F1 = "F1_XBOX";
            F2 = "F2_XBOX";
        }
        if (Controltype == 2)
        {
            if (Input.GetAxisRaw(F1) == 1f && i_MaskIdx != -1 && canTriggerRTXBOX)
            {
                UseSkill();
                canTriggerRTXBOX = false;
            }

            if (Input.GetAxisRaw(F2) == 1f && canTriggerLTXBOX)
            {
                SwitchMask();
                canTriggerLTXBOX = false;
            }

            if (Input.GetAxisRaw(F1) == 0f)
            {
                canTriggerRTXBOX = true;
            }

            if (Input.GetAxisRaw(F2) == 0f)
            {
                canTriggerLTXBOX = true;
            }
        }
        else
        {
            if (Input.GetButtonDown(F1) && i_MaskIdx != -1)
            {
                UseSkill();
            }

            if (Input.GetButtonDown(F2))
            {
                SwitchMask();
            }
        }

    }

    public void HurtMe(float health)
    {
        f_PlayerLife -= health;
    }

    public void HealMe(float health)
    {
        f_PlayerLife += health;
    }


    public void ResetBoomerang()
    {
        b_CanUseBoomerang = true;
        if (i_MaskIdx == 0)
        {
            FireHeadBoomerang.SetActive(true);
            BoomerangMask.SetActive(true);
        }
        if (clone != null)
        {
            Destroy(clone);
        }
    }

    // CHANGER DE MASQUE
    public void SwitchMask()
    {
        switch (i_MaskIdx)
        {
            case -1:
                if (i_MaskFound >= 1)
                {
                    FireHeadBase.SetActive(false);
                    BoomerangMask.SetActive(true);
                    FireHeadBoomerang.SetActive(true);
                    i_MaskIdx++;
                    Maskchange.SetActive(false);
                    Maskchange.SetActive(true);
                }
                break;
            case 0:
                if (i_MaskFound >= 2)
                {
                    BoomerangMask.SetActive(false);
                    FireHeadBoomerang.SetActive(false);
                    if (!b_IsShieldSetup)
                    {
                        ShieldMask.SetActive(true);
                        FireHeadShield.SetActive(true);
                    }
                    i_MaskIdx++;
                    Maskchange.SetActive(false);
                    Maskchange.SetActive(true);
                }
                break;
            case 1:
                if (i_MaskFound == 3)
                {
                    ShieldMask.SetActive(false);
                    FireHeadShield.SetActive(false);
                    GhostMask.SetActive(true);
                    FireHeadGhost.SetActive(true);
                    i_MaskIdx++;
                    Maskchange.SetActive(false);
                    Maskchange.SetActive(true);
                }
                else
                {
                    ShieldMask.SetActive(false);
                    FireHeadShield.SetActive(false);
                    if (b_CanUseBoomerang)
                    {
                        BoomerangMask.SetActive(true);
                        FireHeadBoomerang.SetActive(true);
                    }
                    i_MaskIdx = 0;
                    Maskchange.SetActive(false);
                    Maskchange.SetActive(true);
                }
                break;
            case 2:
                if (i_MaskFound >= 1)
                {
                    if (b_IsInGhostMode)
                    {
                        DestroyGhost();
                    }
                    GhostMask.SetActive(false);
                    FireHeadGhost.SetActive(false);
                    if (b_CanUseBoomerang)
                    {
                        BoomerangMask.SetActive(true);
                        FireHeadBoomerang.SetActive(true);
                    }
                    i_MaskIdx = 0;
                    Maskchange.SetActive(false);
                    Maskchange.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    // UTILISATION DU SKILL
    public void UseSkill()
    {
        switch (i_MaskIdx)
        {
            case 0:
                if (b_CanUseBoomerang)
                {
                    BoomerangPower.SetActive(false);
                    BoomerangPower.SetActive(true);
                    a_Animator.SetTrigger("Throw");
                    clone = Instantiate(BoomerangPrefab, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation) as GameObject;
                    b_CanUseBoomerang = false;
                    FireHeadBoomerang.SetActive(false);
                }
                break;
            case 1:
                if (!b_IsShieldSetup)
                {
                    ShieldPower.SetActive(false);
                    ShieldPower.SetActive(true);
                    a_Animator.SetTrigger("Throw");
                    ShieldSetup = Instantiate(ShieldPrefab, ShieldMask.transform.position, ShieldMask.transform.rotation) as GameObject;
                    b_IsShieldSetup = true;
                    ShieldMask.SetActive(false);
                    FireHeadShield.SetActive(false);
                    break;
                }
                else if (b_IsShieldSetup)
                {
                    ShieldReturn.SetActive(false);
                    ShieldReturn.SetActive(true);
                }
                break;
            case 2:
                if (!b_IsInGhostMode)
                {
                    GhostPower.SetActive(false);
                    GhostPower.SetActive(true);
                    a_Animator.SetTrigger("Throw");
                    GhostSetup = Instantiate(GhostPrefab, t_SpawnPosition.position, t_SpawnPosition.rotation) as GameObject;
                    b_IsInGhostMode = true;
                    GhostMask.SetActive(false);
                    FireHeadGhost.SetActive(false);
                    a_Animator.SetBool("IsMoving", false);
                    break;
                }
                if (b_IsInGhostMode)
                {
                    DestroyGhost();
                }
                break;
            default:
                break;
        }
    }

    public void DestroyGhost()
    {
        GhostPowerEnd.SetActive(false);
        GhostPowerEnd.SetActive(true);
        Destroy(GhostSetup);
        b_IsInGhostMode = false;
        GhostMask.SetActive(true);
        FireHeadGhost.SetActive(true);
    }

    public void DestroyShield()
    {
        Destroy(ShieldSetup);
        ShieldMask.SetActive(true);
        FireHeadShield.SetActive(true);
        b_IsShieldSetup = false;
    }

    // NEW MASK
    public void UnlockNewMask()
    {
        i_MaskFound++;
        switch (i_MaskFound)
        {
            case 1:
                SwitchMask();
                break;
            case 2:
                SwitchMask();
                break;
            case 3:
                ShieldMask.SetActive(false);
                FireHeadShield.SetActive(false);
                BoomerangMask.SetActive(false);
                FireHeadBoomerang.SetActive(false);
                GhostMask.SetActive(true);
                FireHeadGhost.SetActive(true);
                i_MaskIdx = 2;
                Maskchange.SetActive(false);
                Maskchange.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void Die(Transform Spawn)
    {
        Death.SetActive(false);
        Death.SetActive(true);
        DeathFX.SetActive(true);
        PlayerSpawn = Spawn;
        a_Animator.SetTrigger("Die");
        Invoke("Respawn", 0.2f);
    }

    public void SetControlType(int Type)
    {
        Controltype = Type;
    }

    public void Respawn()
    {
        transform.position = PlayerSpawn.position;
        transform.rotation = PlayerSpawn.rotation;
        if (GhostSetup != null)
        {
            DestroyGhost();
        }
        if (ShieldSetup != null)
        {
        }
        DeathFX.SetActive(false);
        a_Animator.SetTrigger("Respawn");
    }




}
