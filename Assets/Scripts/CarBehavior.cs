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
    [SerializeField] private TextMeshProUGUI RemainText;
    [SerializeField] private TextMeshProUGUI CountDownText;
    private int totalCollect = 0;
    private int remain = 0;
    private Sprite tempSprite;

    [SerializeField] private RandomPackage randomPackage;

    private SpriteRenderer spriteRenderer;


    [SerializeField] private Sprite SpeedUpSprite;
    [SerializeField] private Sprite SlowDownSprite;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float limitTime;
    private float currentTime;

    private bool isPlayGame = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Time.timeScale = 1;
        currentTime = limitTime;
        text.text = totalCollect.ToString() + " Packages";
        remain = GameObject.FindGameObjectsWithTag("TakePackage").Length;
        RemainText.text = remain.ToString() + " Houses";
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            StartCoroutine(CountDownToStart());
        }
    }

    public int GetCurrentDisplayNumber()
    {
        List<DisplayInfo> displayLayout = new List<DisplayInfo>();
        Screen.GetDisplayLayout(displayLayout);
        return displayLayout.IndexOf(Screen.mainWindowDisplayInfo);
    }

    void Update()
    {
        if(isPlayGame)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            transform.Rotate(0, 0, horizontalInput * rotateSpeed * Time.deltaTime * -1);
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
                remain -= 1;
                RemainText.text = remain.ToString() + " Houses";
            }
        }
        if (collision.CompareTag("LoadScreen"))
        {
            SceneManager.LoadScene(1);
        }

    }

    public void TryAgain()
    {
        
        SceneManager.LoadScene(0);
    }

    public void PlayGame()
    {
        
        StartCoroutine(CountDownToStart());
    }

    IEnumerator CountDownToStart()
    {
        int countDown = 3;
        while (countDown >= 0)
        {
            CountDownText.text = countDown == 0 ? "GO!": countDown.ToString();
            yield return new WaitForSeconds(1f);
            countDown--;
        }
        CountDownText.gameObject.SetActive(false);  
        isPlayGame = true;
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void Continue()
    {
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("posX", transform.position.x);
        PlayerPrefs.SetFloat("posY", transform.position.y);
        PlayerPrefs.SetFloat("rotateZ", transform.rotation.z);
        PlayerPrefs.SetFloat("rotateW", transform.rotation.w);
        PlayerPrefs.SetInt("packages", totalCollect);
        PlayerPrefs.SetInt("remain", totalCollect);
        PlayerPrefs.SetFloat("speed", moveSpeed);
        tempSprite = spriteRenderer.sprite;
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("posX"))
        {
            var posX = PlayerPrefs.GetFloat("posX");
            var posY = PlayerPrefs.GetFloat("posY");
            var rotateZ = PlayerPrefs.GetFloat("rotateZ");
            var rotateW = PlayerPrefs.GetFloat("rotateW");
            moveSpeed = PlayerPrefs.GetFloat("speed");
            var packages = PlayerPrefs.GetInt("packages");
            remain = PlayerPrefs.GetInt("remain");
            transform.position = new Vector3(posX, posY, 0);
            transform.rotation = new Quaternion(0, 0, rotateZ, rotateW);
            totalCollect = packages;
            spriteRenderer.sprite = tempSprite;
            
        }
    }

    
    


}
