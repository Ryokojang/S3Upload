using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine;

public class FilePicker : MonoBehaviour
{
    byte[] newPictureData;

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
}
