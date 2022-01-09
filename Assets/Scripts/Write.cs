using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Write : MonoBehaviour
{
    public string path = "Assets/message.txt";

    public InputField email;
    public InputField username; 

    public void Button()
    {
        Debug.Log("Method Called");
        StreamWriter writer = new StreamWriter(path, true);
        
        string emailText = email.text;
        string userNameText = username.text;

        //writer.WriteLine("Hello we are connecting Unity to AWS s3");
        writer.WriteLine(emailText + "\n" + userNameText + "\n" + System.DateTime.UtcNow.ToString());

        writer.Close();

        FindObjectOfType<AWSManager>().Upload(path);
    }
}
