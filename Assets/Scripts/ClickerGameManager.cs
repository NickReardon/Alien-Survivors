using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class ClickerGameManager : MonoBehaviour
{
    SaveData saveData;
    int clicks = 0;
    int increment_amt = 1;
    [SerializeField]
    TextMeshProUGUI clicksText;
    [SerializeField]
    SpriteSO spriteObject;
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    bool randomColor = true;
    float height, width;
    Camera main;
    void Start()
    {
        DateTime d = new DateTime();
        //Example of playerprefs, suitable for basic types including strings
        //The datetime is saved using ToString and we use TryParse for loading
        DateTime.TryParse(PlayerPrefs.GetString("LastOnline", DateTime.Now.ToString()),out d);
        Debug.Log((DateTime.Now - d).ToString() + " since last run");

        saveData = ScriptableObject.CreateInstance<SaveData>();
        //Example using the filesystem manually, which allows for larger
        //file sizes and a greater level of control
        if(File.Exists(Application.persistentDataPath+"/CurScore.dat"))
        {
            Debug.Log("Loading from " + Application.persistentDataPath+"/CurScore.dat");
            JsonUtility.FromJsonOverwrite(File.ReadAllText(Application.persistentDataPath+"/CurScore.dat"),saveData);
            clicks = saveData.clicks;
        }
    
        clicksText.text = clicks.ToString();
        main = Camera.main;
        height = main.orthographicSize;
        width = height * main.aspect;
    }

    public void Increment()
    {
        clicks += increment_amt;
        clicksText.text = clicks.ToString();
        var p = Instantiate(prefab, new Vector3(UnityEngine.Random.Range(-width, width),
            UnityEngine.Random.Range(-height,height), 0),Quaternion.identity);
        var sr = p.GetComponent<SpriteRenderer>();
        sr.sprite = spriteObject.sprite;
        if(randomColor)
            sr.color = UnityEngine.Random.ColorHSV();
        else
            sr.color = spriteObject.color;
        p.GetComponent<ClickParticle>().maxScale = UnityEngine.Random.Range(spriteObject.scale*0.5f, spriteObject.scale*2f);
    }

    //Note: There are ways to exit the application that don't trigger lifecycle functions
    //so there is a good reason to attempt to save critical data during gameplay
    void OnDestroy()
    {
        ShuttingDown();
    }
    public void ShuttingDown()
    {
        PlayerPrefs.SetString("LastOnline",DateTime.Now.ToString());
        saveData.clicks = clicks;
        File.WriteAllText(Application.persistentDataPath+"/CurScore.dat", JsonUtility.ToJson(saveData));

    }
}
