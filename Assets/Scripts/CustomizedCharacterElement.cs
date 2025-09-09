using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomizedCharacterElement : MonoBehaviour
{
    [field: SerializeField]
    public CustomizationType Type { get; private set; }
    [field: SerializeField]
    private CustomizedCharacter _character;
    private SpriteRenderer _spriteRenderer;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        var customzation = _character.Data.FirstOrDefault(d => d.Type == Type);
        if (customzation == null) return;
        _spriteRenderer.sprite = customzation.Sprite.Sprite;
        transform.localPosition = customzation.Sprite.Position;
    }
    
}