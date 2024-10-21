using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public LevelObject[] levelObjects;
    public static int levelCurr;

    public static int UnlockedLevels;
    private GameObject spawnedPrefab;

    //public Board board;
    public bool IsStartLevel;
    public GameObject Main;
    public GameObject levelMenu;
    public GameObject Starwin1;
    public GameObject Starwin2;
    public GameObject Completedscene;
    public GameObject Failedscene;
    public LevelData levelData;
    public float countdownTime = 45f;
    private float currentTime;
    public Text countdownText;
    Vector3 offset = new Vector3(-38f, 50.0f, 0.0f);
   

    void Start()
    {   UnlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 0);
        currentTime = countdownTime;
        //SpawnPrefabs();
        //PlayerPrefs.SetInt("UnlockedLevels", 0);
        for (int i = 0; i < levelObjects.Length; i++)
        {   int stars = PlayerPrefs.GetInt("stars" + i.ToString(), 0);
            //PlayerPrefs.SetInt("stars" + i.ToString(), 0);
            if (UnlockedLevels >= i)
            {
                levelObjects[i].LevelButton.interactable = true;
                levelObjects[i].Lock.gameObject.SetActive(false);
                 PlayerPrefs.Save();
                
            }
        }
        
    }

    private void Update()
    {
        if (levelMenu.activeSelf)
        {
            //Debug.Log(levelObjects.Length);
            for (int i = 0; i < levelObjects.Length; i++)
            {     int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevels", 0);
                
                if (unlockedLevel >= i)
                {   
                    levelObjects[i].LevelButton.interactable = true;
                    levelObjects[i].Lock.gameObject.SetActive(false);

                    int star = PlayerPrefs.GetInt("stars" + i.ToString(), 0);
                    
                    for (int j = 0; j < star; j++)
                    {
                        levelObjects[i].Star[j].gameObject.SetActive(true);
                    }
                }
                
            }
        }

        
        
        
        if (IsStartLevel == true && currentTime > 1 && currentTime <= 45)
        {
            currentTime -= Time.deltaTime;

            if (countdownText != null)
            {
                countdownText.text = Mathf.CeilToInt(currentTime).ToString();
                int minutes = Mathf.FloorToInt(currentTime / 60f);
                int seconds = Mathf.FloorToInt(currentTime % 60f);

                // Cập nhật giá trị của TimeText với định dạng "00:00"
                countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }

            if (Board.Instance != null && Board.Instance.GetIsWin())
            {   
                Completedscene.SetActive(true);
                if (levelCurr == UnlockedLevels+1)
                {  
                    UnlockedLevels++;
                    //Debug.Log(UnlockedLevels+ "level");
                    PlayerPrefs.SetInt("UnlockedLevels", UnlockedLevels);
                    PlayerPrefs.Save();
                }

                int a = (levelCurr - 1);
                if (currentTime > 30)
                {
                    PlayerPrefs.SetInt("stars" + a.ToString(), 3);
                    PlayerPrefs.Save();
                }

                if (currentTime < 30)
                    {
                        Starwin1.SetActive(false);
                        PlayerPrefs.SetInt("stars" + a.ToString(), Math.Max(2,PlayerPrefs.GetInt("stars" + a.ToString(), 0)));

                    }

                    if (currentTime < 15)
                    {
                        Starwin2.SetActive(false);
                        PlayerPrefs.SetInt("stars" + a.ToString(), Math.Max(1,PlayerPrefs.GetInt("stars" + a.ToString(), 0)));
                    }
                    PlayerPrefs.Save();
                    currentTime = 46;
                    


                }

            }
            else if (currentTime ==0)
            {
                Failedscene.SetActive(true);
                currentTime = 46f;
            }

        }
    

    void SpawnPrefabs(int levelNum)
    {
        spawnedPrefab = Instantiate(levelData.prefabs[levelNum - 1], transform.position + offset, Quaternion.identity);

        Main.SetActive(true);
            Starwin2.SetActive(true);
            Starwin1.SetActive(true);

            currentTime = countdownTime;
    }

        public void OnClickHomeButton()
        {   
            SceneManager.LoadScene("Play");
            spawnedPrefab.SetActive(false);
            Completedscene.SetActive(false);
            


        }

         public void OnClickLevelButton(int levelNum)
        {
            IsStartLevel = true;
            levelCurr = levelNum;
            levelMenu.SetActive(false);
            SpawnPrefabs(levelNum);
        }

         public void OnClickNextButton()
         {
             levelCurr++;
             Completedscene.SetActive(false);
             spawnedPrefab.SetActive(false);
             SpawnPrefabs(levelCurr);
         }

         public void OnClickBackButton()
         {
             SceneManager.LoadScene("GameMenu");
         }

         public void OnClickResetButton()
         {
             Completedscene.SetActive(false);
             spawnedPrefab.SetActive(false);
             SpawnPrefabs(levelCurr);
         }
             
         
    }
