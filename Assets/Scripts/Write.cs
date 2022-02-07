using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
//using Amazon.CognitoIdentity;
using Amazon.S3;
using Amazon.S3.Model;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using Amazon.S3.Transfer;


public class Write : MonoBehaviour
{
    

    public InputField email;
    public InputField userName;
    public Text buttonText;

    public Image avatar;
    public Texture2D copy;

    public string Path;
    public void Button()
    {
        /*if (email.text == "" || userName.text == "")
        {
            buttonText.text = "Fill in Info Please";
        }
        else/**/
        {
            //copy = duplicateTexture((Texture2D)avatar.mainTexture);
            //UserInfo userInfo = new UserInfo()
            //{
            //    //email = email.text,
            //    //userName = userName.text,
            //    //picture = copy.EncodeToPNG()
            //    picture = copy.EncodeToJPG()
            //};
            
            //copy = duplicateTexture((Texture2D)avatar.mainTexture);
            //byte[] userInfoData = copy.EncodeToJPG();
            //FindObjectOfType<AWSManager>().UploadFile(userInfoData, userName.text);
        }

        //Task.Run(() => FindObjectOfType<NuGetS3>().UploadFileAsync(Path, userNam//e.text));
        //FindObjectOfType<NuGetS3>().UploadFileAsync(Path, userName.text);
        //FindObjectOfType<NuGetS3>().UploadFileAsync2(Path, userName.text);

        //UploadFileAsync(Path, userName.text);
        Task.Run(() => UploadFileAsync(Path, userName.text));
    }

    private byte[] ObjectToByteArray(UserInfo obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, obj);

        return ms.ToArray();
    }

    public Texture2D duplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
            source.width,
            source.height,
            0, RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear
            );

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableTex = new Texture2D(source.width, source.height);
        readableTex.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableTex.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableTex;
    }

    public void ManageNewPicture(byte[] newImageData)
    {
        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(newImageData);
        tex.Apply();
        Sprite newImage = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        avatar.sprite = newImage;
    }

    private const string bucketName = "unitys3course9-ryoko";
    //private const string keyName = "";
    //private const string filePath = "";
    private const string accessKeyID = "AKIAX5EHWC7R6T3ZRPDE";
    private const string accessSecretKey = "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";
    private static readonly RegionEndpoint buckertRegion = RegionEndpoint.APNortheast2;

    public async Task UploadFileAsync(string filePath, string keyName)
    {
        try
        {
            Debug.Log("UploadFileAsync");
            var fileTransferUtility = new TransferUtility(accessKeyID, accessSecretKey, buckertRegion);

            Debug.Log("UploadFileAsync2");
            await fileTransferUtility.UploadAsync(filePath, bucketName, keyName);

            /*
            Debug.Log("UploadFileAsync2");
            using (var fileToUpload = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                await fileTransferUtility.UploadAsync(fileToUpload, bucketName, keyName);
            }
            Debug.Log("UploadFileAsync3");/**/
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
