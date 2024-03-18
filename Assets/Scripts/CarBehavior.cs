using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarBehavior : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed = 1f;

    [SerializeField]
    private float rotateSpeed = 1f;

    [SerializeField]
    private float SpeedUp = 1f;

    [SerializeField]
    private float slowDown = 1f;



    [SerializeField] private TextMeshProUGUI text;

    private int totalCollect = 0;

    [SerializeField] private RandomPackage randomPackage;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite SpeedUpSprite;
    [SerializeField] private Sprite SlowDownSprite;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float limitTime;
    private float currentTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentTime = limitTime;
        text.text = totalCollect.ToString() + " Packages";
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(0,0, horizontalInput * rotateSpeed * Time.deltaTime * -1);
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime * verticalInput);
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime; // Reduce the current time by deltaTime
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);

            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            UIController.Instance.ShowTryAgain();
            //Time.timeScale = 0;
            timeText.text = string.Format("{0:00}:{1:00}", 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Package"))
        {
            totalCollect += 1;
            text.text = totalCollect.ToString()+" Packages";
            randomPackage.RandomP();
           collision.GameObject().SetActive(false);

        }
        if (collision.CompareTag("SpeedUp"))
        {
            spriteRenderer.sprite = SpeedUpSprite;
            moveSpeed = SpeedUp;
        }
        if(collision.CompareTag("SlowDown"))
        {
            moveSpeed = slowDown;
            spriteRenderer.sprite = SlowDownSprite;
        }
        if (collision.CompareTag("TakePackage"))
        {
            var takePackage = collision.gameObject.GetComponent<TakePackages>();
           if (takePackage != null && !takePackage.isReceivedPackage && totalCollect>0)
            {
                totalCollect -= 1;
                text.text = totalCollect.ToString() + " Packages";
                takePackage.setReceived(true);
            }
        }
        if (collision.CompareTag("LoadScreen"))
        {
            SceneManager.LoadScene(1);
        }


    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
    


}
