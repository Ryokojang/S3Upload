using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;

public class Write : MonoBehaviour
{
    public string path = "Assets/message.txt";

    public InputField email;
    public InputField userName;
    public Text buttonText;

    public void UpdateText()
    {
        Debug.Log("Method Called");
        StreamWriter writer = new StreamWriter(path, true);

        string emailText = email.text;
        string userNameText = userName.text;

        writer.WriteLine("Hello we are connecting Unity to AWS s3");
        writer.WriteLine(emailText + "\n" + userNameText + "\n" + System.DateTime.UtcNow.ToString());
        FindObjectOfType<AWSManager>().Upload(path);

        writer.Close();
    }

    public void UploadPicture()
    {
        Debug.Log("Method Called");
        StreamWriter writer = new StreamWriter(path, true);
        
        string picturePath = "Assets/301.png";
        FindObjectOfType<AWSManager>().Upload(picturePath);
    }

    public void Button()
    {
        if (email.text == "" || userName.text == "")
        {
            buttonText.text = "Fill in Info Please";
        }
        else
        {
            UserInfo userInfo = new UserInfo()
            {
                email = email.text,
                userName = userName.text
            };

            byte[] userInfoData = ObjectToByteArray(userInfo);
            FindObjectOfType<AWSManager>().Upload(userInfoData, userName.text);
        }
    }

    private byte[] ObjectToByteArray(UserInfo obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, obj);

        return ms.ToArray();
    }

    /*
    public void Button()
    {
        if (email.text == "" || userName.text == "")
        {
            buttonText.text = "Please Insert Info";
        }
        else
        {
            UserInfo userInfo = new UserInfo()
            {
                email = email.text,
                userName = userName.text
            };

            byte[] userInfoData = ObjectTyByteArray(userInfo);
            FindObjectOfType<AWSManager>().Upload(userInfoData, userName.text);
        }
    }

    private byte[] ObjectTyByteArray(UserInfo obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, obj);

        return ms.ToArray();
    }/**/
}
