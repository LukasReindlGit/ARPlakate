using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class DriveFileHandler : MonoBehaviour
{
    public static DriveFileHandler Instance;

    string configURL = "https://docs.google.com/spreadsheets/d/1WObjBVu-j-bQ6QI_GjZjYSDWZIWI0qIhCMh6bsUXeF0/edit?usp=sharing";
    
    [SerializeField]
    ConfigElement[] configs;
    
    // Use this for initialization
    void Awake()
    {        
        Init();
    }

    private void Init()
    {
        Instance = this;
        StartCoroutine(LoadConfigFromURL(configURL));
    }

    IEnumerator LoadConfigFromURL(string url)
    {
        //Parse the drive document to extract the poster urls

        WWW www = new WWW(url);
        yield return www;

        // Split on Line endings
        string[] tmpArr = Regex.Split(www.text, "\n|\r|\r\n");
        List<ConfigElement> urlList = new List<ConfigElement>();

        // Run through http text.
        bool started = false;
        for (int i = 0; i < tmpArr.Length; i++)
        {
            if (!started)
            {
                // Start writing data after "Zeitstempel" was found
                if (tmpArr[i].Contains("Zeitstempel"))
                {
                    started = true;
                }
                continue;
            }

            // Stop after
            if (tmpArr[i].Contains("<meta name="))
            {
                started = false;
                break;
            }

            // Add to list
            if (!string.IsNullOrEmpty(tmpArr[i]))
            {
                urlList.Add(new ConfigElement(tmpArr[i]));
            }
            
        }
        configs = urlList.ToArray();
        
    }


    public string GetRandomURLFromConfig(int ttl = 100)
    {
        if (ttl <= 0)
            return "";

        string url = configs[UnityEngine.Random.Range(0, configs.Length)].url;

        if (url.Length < 5 || !url.Contains("http"))
            url = GetRandomURLFromConfig(ttl - 1);

        return url;
    }

}

[Serializable]
public class ConfigElement
{
    public string zeitstempel;
    public string url;
    public string[] tags;

    public ConfigElement(string s)
    {
        string[] tmp = s.Split(',');
        zeitstempel = tmp[0];
        url = tmp[1].Replace(" ", "");
        if (tmp.Length > 2)
        {
            tags = tmp[2].Split(' ');
        }
    }
}
    
