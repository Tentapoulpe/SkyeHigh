using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class UltraPolygonCollider2D: MonoBehaviour
{
    private PolygonCollider2D Collider { get { return GetComponent<PolygonCollider2D>(); } }
    private Sprite UpdatedSprite { get { return GetComponent<SpriteRenderer>().sprite; } }
    private Sprite RegisteredSprite = null;
    [HideInInspector]
    public bool isTrigger;

    private void Awake()
    {
        UpdateSprite();
    }

    private void FixedUpdate()
    {
        if (RegisteredSprite != UpdatedSprite)
        {
            Destroy(Collider);
            gameObject.AddComponent<PolygonCollider2D>();
            if (isTrigger)
            {
                Collider.isTrigger = true;
            }
        }

        UpdateSprite();
    }

    void UpdateSprite()
    {
        RegisteredSprite = UpdatedSprite;
    }

}
