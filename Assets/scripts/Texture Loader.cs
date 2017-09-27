﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextureLoader : MonoBehaviour {

    Texture[] textures;
    string[] paths = {
        "Super Konky Fighter\\Assets\\textures\\attacktorb.png",
        "Super Konky Fighter\\Assets\\textures\\soldier.png"
    };

	// Use this for initialization
	void Start () {
        int numTextures = paths.Length;
        textures = new Texture[numTextures];
        for(int i=0; i < numTextures; ++i)
        {
            textures[i] = LoadPNG(paths[i]);
        }
	}
	
	void Update () {}

    public static Texture2D LoadPNG(string filePath)
    {
        Texture2D texture = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            texture = new Texture2D(2,2);
            texture.LoadImage(fileData);
        }
        return texture;
    }
}

