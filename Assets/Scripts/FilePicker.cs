using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine;

public class FilePicker : MonoBehaviour
{
    byte[] newPictureData;
    public Texture2D texture;
    public Texture2D copy;
    /*
    //Only wroks for desktop
    public void ChoosePicture()
    {
        string path = EditorUtility.OpenFilePanel("Open File", "", "jpg");
        if(path.Length != 0)
        {
            var fileContent = File.ReadAllBytes(path);
            newPictureData = fileContent;

            FindObjectOfType<Write>().ManageNewPicture(newPictureData);
        }
    }
    /**/

    public void ChoosePicture()
    {
        NativeGallery.Permission permissionS3Course = NativeGallery.GetImageFromGallery((path) => 
        {
            if(path.Length != 0)
            {
                texture = NativeGallery.LoadImageAtPath(path, 5000);
                copy = FindObjectOfType<Write>().duplicateTexture(texture);
                newPictureData = copy.EncodeToJPG();
                FindObjectOfType<Write>().ManageNewPicture(newPictureData);
            }
        });
    }
}
