using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class Write : MonoBehaviour
{
    

    public InputField email;
    public InputField userName;
    public Text buttonText;

    public Image avatar;
    public Texture2D copy;
    

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
            copy = duplicateTexture((Texture2D)avatar.mainTexture);
            byte[] userInfoData = copy.EncodeToJPG();
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
}
