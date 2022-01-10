using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;

public class Write : MonoBehaviour
{
    

    public InputField email;
    public InputField userName;
    public Text buttonText;

    public void UpdateText()
    {
        string path = "Assets/message.txt";
        
        Debug.Log("Method Called");
        StreamWriter writer = new StreamWriter(path, true);

        string emailText = email.text;
        string userNameText = userName.text;

        writer.WriteLine("Hello we are connecting Unity to AWS s3");
        writer.WriteLine(emailText + "\n" + userNameText + "\n" + System.DateTime.UtcNow.ToString());
        writer.Close();

        FindObjectOfType<AWSManager>().UpdateText(path);
    }

    public void UploadPicture()
    {
        Debug.Log("Method Called");
        
        string picturePath = "Assets/301.png";
        FindObjectOfType<AWSManager>().UploadPicture(picturePath);
    }

    public void UploadFile()
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
            FindObjectOfType<AWSManager>().UploadFile(userInfoData, userName.text);
        }
    }

    private byte[] ObjectToByteArray(UserInfo obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, obj);

        return ms.ToArray();
    }
}
