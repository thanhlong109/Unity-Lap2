using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePackages : MonoBehaviour
{
    public bool isReceivedPackage = false;
    public float rotateSprite = 90f;
    public Sprite receivedSprite;
    public GameObject renderObj = null;
    private SpriteRenderer spriteRenderer ;

    private void Awake()
    {
        spriteRenderer = renderObj.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        isReceivedPackage = false;
    }
    public void setReceived(bool isReceived)
    {
        this.isReceivedPackage = isReceived;
        renderObj.transform.rotation = Quaternion.Euler(0, 0, rotateSprite);
        spriteRenderer.sprite = receivedSprite;
    }
}
