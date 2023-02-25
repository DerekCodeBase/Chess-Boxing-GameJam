using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwitcher : MonoBehaviour
{
    public Sprite[] sprites;
    public SpriteRenderer thisImage;

    public void SetSprite(int sprite){
        thisImage.sprite = sprites[sprite];
    }
}
