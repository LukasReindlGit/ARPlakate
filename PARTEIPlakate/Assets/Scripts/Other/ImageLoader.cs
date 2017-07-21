using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class ImageLoader : MonoBehaviour
{
    /** Pipeline:
     * 
     * Load config
     * 
     * Foreach url:
     *      - Create Hashed name
     *      - Save name to namelist
     * 
     *      - Does File exist?
     *          No:
     *              - Load Texture
     *              - Save file in cache
     * 
     *      - Load Texture from cache
     * 
     */

    [SerializeField]
    TextAsset configFile;
    [SerializeField]
    string[] config;

    [SerializeField]
    bool loadFromInternet = true;

    private void Awake()
    {
        InitTexture();
    }

    private void InitTexture()
    {
        if (loadFromInternet)
        {
            LoadConfig();

            CheckIfFilesExist();            
        }
    }

    private void CheckIfFilesExist()
    {
        foreach (string s in config)
        {
            CheckIfFileExists(s);
        }
    }

    private void CheckIfFileExists(string s)
    {
        string path = GetLocalPathFromURL(s);

        if (!File.Exists(path))
        {
            Debug.Log("File does not exist. loading INTO cache: " + path);
            StartCoroutine(LoadTextureFromURL(s));
        }
        else
        {            
            StartCoroutine(LoadTextureFromCache(path));
        }
    }
    
    /// <summary>
    /// URL is too long, convert it to short path
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private string GetLocalPathFromURL(string s)
    {
        return Application.persistentDataPath + "/" + s.GetHashCode() + ".jpg";
    }
        
    IEnumerator LoadTextureFromURL(string url)
    {
        Texture2D tex;
        tex = new Texture2D(4, 4, TextureFormat.RGB24, false);
        WWW www = new WWW(url);
        yield return www;
        www.LoadImageIntoTexture(tex);
        if (tex != null)
        {
            TextureManager.Instance.AddTexture(tex);
            SaveTextureToCache(tex, url);
        }
    }

    /// <summary>
    /// Loads texture from local cache and adds it to a list.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private IEnumerator LoadTextureFromCache(string path)
    {
        if (!File.Exists(path))
        {
            yield return null;
        }

        Texture2D tex;
        tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
        tex.name = Path.GetFileName(path);

        WWW www = new WWW("File://"+path);
        yield return www;
        www.LoadImageIntoTexture(tex);
        // If no texture was loaded, tex.width is still 4.
        if (tex.width>4)
        {            
            TextureManager.Instance.AddTexture(tex);
        }

    }

    /// <summary>
    /// Save texture in persistant data path
    /// </summary>
    /// <param name="tex"></param>
    /// <param name="url"></param>
    private void SaveTextureToCache(Texture2D tex, string url)
    {
        string path = GetLocalPathFromURL(url);

        if (File.Exists(path))
        {
            throw new System.Exception("File already exists! " + path);
        }

        byte[] bytes = tex.EncodeToJPG();
        File.WriteAllBytes(path, bytes);
    }

    private void LoadConfig()
    {
        string fs = configFile.text;
       
        List<string> temp = new List<string>();

        foreach (string s in Regex.Split(fs, "\n|\r|\r\n"))
        {
            if(!string.IsNullOrEmpty(s))
            {
                temp.Add(s);
            }
        }
        config = temp.ToArray();
    }
}
