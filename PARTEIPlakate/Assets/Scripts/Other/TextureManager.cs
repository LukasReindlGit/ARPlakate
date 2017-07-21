using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureManager : MonoBehaviour {

    public static TextureManager Instance;

    [SerializeField]
    Texture2D[] fallbackTextures;

    List<Texture2D> allTextures= new List<Texture2D>();

    [SerializeField]
    Texture2D[] allTexturesArr;

    private void Awake()
    {
        Instance = this;

        // Init textures
        foreach(var tex in fallbackTextures)
        {
            allTextures.Add(tex);
        }
    }

    public static Texture2D GetRandomTexture()
    {
        return Instance.GetRandomTextureFromList();
    }

    private Texture2D GetRandomTextureFromList()
    {
        return allTextures[UnityEngine.Random.Range(0, allTextures.Count)];
    }

    internal void AddTexture(Texture2D tex)
    {
        allTextures.Add(tex);
        allTexturesArr = allTextures.ToArray();
    }
}
