using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable/Character", order = 1)]

public class Script_Scriptable_Characters : ScriptableObject
{
    [Header ("Movement Horizontal")]
    public float m_maxHorizontalSpeed;
    public float m_horizontalAcceleration;
    public float m_maxHorizontalDrag;

    [Header("Movement Vertical")]
    public float m_maxVerticalUpSpeed;
    public float m_VerticalUpAcceleration;
    public float m_VerticalDownAcceleration;
    public float m_maxVerticalDrag;

    [Header("Gravity")]
    public float m_gravityMultiplier;

    [Header("Dash")]
    public float m_dashPower;
    public float m_maxDashCooldown;
    public float m_dashCost;
    public float m_dashPostStun;
    public float m_dashStun;

    [Header("Environment")]

    public float m_cloudSlow;
    public float m_delayToRegenerate;

}
