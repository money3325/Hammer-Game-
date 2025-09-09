using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomizableElement : MonoBehaviour
{

    [SerializeField]
    private CustomizationType _type;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    //这边list调用的是另一个脚本的名称
    private List<positionedSprite> _spriteOptions;
    [field: SerializeField]
    public int SpriteIndex { get; private set; }
    [ContextMenu("Next Sprite")]
    public void NextSprite()
    {
        SpriteIndex = Mathf.Min(SpriteIndex + 1, _spriteOptions.Count - 1);
        UpdateSprite();
        //return _spriteOptions[SpriteIndex];
    }
    [ContextMenu("Previous Sprite")]
    public void PreviousSprite()
    {
        SpriteIndex = Mathf.Max(SpriteIndex - 1, 0);
        UpdateSprite();
        //return _spriteOptions[SpriteIndex];
    }
    public void UpdateSprite()
    {
        SpriteIndex = Mathf.Clamp(SpriteIndex, 0, _spriteOptions.Count - 1);
        var positionedSprite = _spriteOptions[SpriteIndex];
        _spriteRenderer.sprite = positionedSprite.Sprite;
        transform.localPosition = positionedSprite.Position;
    }
    public CustomzationDate GetCustomizationDate()
    {
        return new CustomzationDate(_type, _spriteOptions[SpriteIndex]);
    }
    [ContextMenu("Updata Position Modifier")]
    public void UpdataSpritePositionModifier()
    {
        _spriteOptions[SpriteIndex].Position = transform.localPosition;
    }
}
