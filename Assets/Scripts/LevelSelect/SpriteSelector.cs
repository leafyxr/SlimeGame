using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSelector : MonoBehaviour
{
    public MapSprites sprites;
    public bool up, down, left, right;
    public int id;

    public Color color, SelectedColor;
    Color MainColor;

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        MainColor = color;
        select();
        gameObject.AddComponent<PolygonCollider2D>();
    }

    void select()
    {
        if (up && down && left && right) spriteRenderer.sprite = sprites.C;
        else if (up && down && left) spriteRenderer.sprite = sprites.TBL;
        else if (up && down && right) spriteRenderer.sprite = sprites.TBR;
        else if (up && left && right) spriteRenderer.sprite = sprites.TLR;
        else if (down && left && right) spriteRenderer.sprite = sprites.BLR;
        else if (up)
        {
            if (down) spriteRenderer.sprite = sprites.TB;
            else if (left) spriteRenderer.sprite = sprites.TL;
            else if (right) spriteRenderer.sprite = sprites.TR;
            else spriteRenderer.sprite = sprites.T;
        }
        else if (down)
        {
            if (left) spriteRenderer.sprite = sprites.BL;
            else if (right) spriteRenderer.sprite = sprites.BR;
            else spriteRenderer.sprite = sprites.B;
        }
        else if (left)
        {
            if (right) spriteRenderer.sprite = sprites.LR;
            else spriteRenderer.sprite = sprites.L;
        }
        else if (right) spriteRenderer.sprite = sprites.R;
    }
}
