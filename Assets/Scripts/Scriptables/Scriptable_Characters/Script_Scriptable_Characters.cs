using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Heroes {Skye, Dawn, Nimbus, Bolt};

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable/Character", order = 1)]
public class Script_Scriptable_Characters : ScriptableObject
{
    [Header ("Movement Horizontal")]
    public float m_maxHorizontalSpeed;
    public float m_horizontalAcceleration;
    public float m_maxHorizontalDrag;

    [Header("Movement Vertical")]
    public float m_maxVerticalUpSpeed;
    public float m_maxVerticalDownSpeed;
    public float m_VerticalUpAcceleration;
    public float m_VerticalDownAcceleration;
    public float m_maxVerticalDrag;

    [Header("Gravity")]
    public float m_gravityMultiplier;//gravity intensity

    [Header("Dash")]
    public float m_dashPower;//distance of dashing
    public float m_maxDashCooldown;//cd after press dash button
    public float m_dashCost;//purcentage of life cost when player dash
    public float m_dashPostStun;//stun time after a dash
    public float m_dashStun;//stun time after a dash

    [Header("Environment")]

    public float m_cloudSlow;//Slow when player enter in cloud

    [Header("Player")]

    public int m_maxHealth;
    public Heroes myChracters;
}
