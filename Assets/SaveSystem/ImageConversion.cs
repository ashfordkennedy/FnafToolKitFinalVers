using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class ImageConversion
{
   
    public static string textureToString(Texture2D Texture)
    {
        byte[] textByte = Texture.EncodeToPNG();
        string Base64Tex = System.Convert.ToBase64String(textByte);
       

        return Base64Tex;

    }


    public static Sprite StringToSprite(string TextureString)
    {

        byte[] textByte = System.Convert.FromBase64String(TextureString);
        Texture2D ConvertedTexture = new Texture2D(256, 256);
        ConvertedTexture.LoadImage(textByte, false);
        ConvertedTexture.Apply();
        Sprite gensprite = Sprite.Create(ConvertedTexture, new Rect(0.0f, 0.0f, ConvertedTexture.width, ConvertedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        

        return gensprite;

    }

    public static Sprite FIlePathToSprite(string path)
    {


        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(path))
        {
            FileData = File.ReadAllBytes(path);
            Texture2D ConvertedTexture = new Texture2D(2, 2);           // Create new "empty" texture
            if (ConvertedTexture.LoadImage(FileData))
            {

                Sprite gensprite = Sprite.Create(ConvertedTexture, new Rect(0.0f, 0.0f, ConvertedTexture.width, ConvertedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                // Load the imagedata into the texture (size is set automatically)
                return gensprite;
            }
                // If data = readable -> return texture
        }

        return null;                     // Return null if load failed
    }

    public static string FIlePathToString(string path)
    {


        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(path))
        {
            FileData = File.ReadAllBytes(path);
            Texture2D ConvertedTexture = new Texture2D(2, 2);           // Create new "empty" texture
            if (ConvertedTexture.LoadImage(FileData))
            {
               return textureToString(ConvertedTexture);
            }
            // If data = readable -> return texture
        }

        return null;                     // Return null if load failed
    }

}

