using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class ImageLoader : MonoBehaviour
{

    [SerializeField]
    TextAsset configFile;
    string[] config;

    [SerializeField]
    Renderer visual;

    // Use this for initialization
    void Start()
    {
        if (visual == null)
        {
            visual = GetComponent<Renderer>();
        }

        InitTexture();
    }

    private void InitTexture()
    {
        LoadConfig();
        StartCoroutine(LoadTextureFromURL(GetRandomURLFromConfig()));
    }

    private string GetRandomURLFromConfig(int ttl = 100)
    {
        if (ttl <= 0)
            return "";

        string url = config[UnityEngine.Random.Range(0, config.Length)];

        if (url.Length < 5)
            url = GetRandomURLFromConfig(ttl - 1);

        return url;
    }


    IEnumerator LoadTextureFromURL(string url)
    {
        Debug.Log("Trying: URL: " + url);

        Texture2D tex;
        tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
        WWW www = new WWW(url);
        yield return www;
        www.LoadImageIntoTexture(tex);
        if (tex != null)
        {
            visual.material.mainTexture = tex;
        }

    }

    private void LoadConfig()
    {
        string fs = configFile.text;
        config = Regex.Split(fs, "\n|\r|\r\n");
    }
}
