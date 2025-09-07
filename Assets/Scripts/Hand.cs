using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform hammerHandle;
    public Sprite[] sprites;
    public bool rightHand = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 handDir = hammerHandle.position - transform.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.down, handDir);
        GetComponent<SpriteRenderer>().flipX = rightHand ^ handDir.y > 0;

        int SpriteIndex = Mathf.Clamp((int)(handDir.magnitude * 4), 0, sprites.Length - 1);
        GetComponent<SpriteRenderer>().sprite = sprites[SpriteIndex];
    }
}
