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
        }

        UpdateSprite();
    }

    void UpdateSprite()
    {
        RegisteredSprite = UpdatedSprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
