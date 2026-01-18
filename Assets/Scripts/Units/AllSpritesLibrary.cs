using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteLibrary", menuName = "Sprites/Sprite Library")]
public class AllSpritesLibrary : ScriptableObject
{
    public List<Sprite> allPossibleSprites;

    public Sprite GetRandomSprite()
    {
        return allPossibleSprites[Random.Range(0, allPossibleSprites.Count)];
    }
}
