﻿using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
    public Sprite dmgSprite;
    public int Hp = 4;
    private SpriteRenderer spriteRenderer;
    public AudioClip chopSound1;
    public AudioClip chopSound2;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int loss)
    {
        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);
        spriteRenderer.sprite = dmgSprite;
        Hp -= loss;
        if (Hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
	
}
