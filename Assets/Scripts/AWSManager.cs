using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.S3;
using Amazon.S3.Model;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;

public class AWSManager : MonoBehaviour
{

    public void UsedOnlyForAOTCOdeGeneration()
    {
        AndroidJavaObject jo = new AndroidJavaObject("android.os.Message");
        int valueString = jo.Get<int>("what");
    }

    //string IdentityPoolID = "ap-northeast-2:e33ea593-2e47-4aca-bd90-f797983beb0b";
    //string bucketname = "lambda-test-bucket-resized/origin";

    public string identityId;
    private static AWSManager _instance;

    public string IdentityPoolID = "ap-northeast-2:c4e9edd9-92a0-4389-82d6-494b7c056f1c";
    string bucketname = "unitys3course9-ryoko";


    public static AWSManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("NULL");
            }
            return _instance;
        }
    }

    private CognitoAWSCredentials _credentials;

    public CognitoAWSCredentials Credentials
    {
        get
        {
            if (_credentials == null)
            {
                _credentials = new CognitoAWSCredentials(IdentityPoolID, RegionEndpoint.APNortheast2);
            }
            return _credentials;
        }
    }



    /*private string S3Region = RegionEndpoint.APNortheast2.SystemName;
     private RegionEndpoint _S3Region
     {
         get
         {
             return RegionEndpoint.GetBySystemName(S3Region);
         }
     }/**/

    private AmazonS3Client _S3Client;

    public AmazonS3Client S3Client
    {
        get
        {
            if (_S3Client == null)
            {
                //_S3Client = new AmazonS3Client(new CognitoAWSCredentials(IdentityPoolID, RegionEndpoint.APNortheast2), _S3Region);
                //_S3Client = new AmazonS3Client("AKIAX5EHWC7R6T3ZRPDE", "8FoOlZX3b5pHFKs1lFnsIjFFljmhxQfq9OjpHJzs", _S3Region);
                _S3Client = new AmazonS3Client("AKIAX5EHWC7R6T3ZRPDE", "8FoOlZX3b5pHFKs1lFnsIjFFljmhxQfq9OjpHJzs", RegionEndpoint.APNortheast2);
            }
            return _S3Client;
        }
    }

    void LoadAssemble()
    {
        List<string> assemblies = new List<String>() {
                "AWSSDK.S3.dll",
                "AWSSDK.Core.dll",
                "AWSSDK.CognitoIdentity.dll",
                "AWSSDK.SecurityToken.dll"
            };
        string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        foreach (string assemblyName in assemblies)
        {
            string assemblyPath = Path.Combine(folderPath, assemblyName);
            if (!File.Exists(assemblyPath))
                continue;
            Assembly assembly = Assembly.LoadFrom(assemblyPath);

        }
    }

    private void Start()
    {
        //LoadAssemble();
        Debug.Log("Start!");

        _instance = this;


        /*UnityInitializer.AttachToGameObject(this.gameObject);
        try
        {
            AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        }
        catch(Exception e)
        {
            Debug.Log(3);
        }/**/

        /*Credentials.GetIdentityIdAsync(delegate (AmazonCognitoIdentityResult<string> result)
        {
            if(result.Exception != null)
            {
                Debug.Log(result.Exception);
            }
            identityId = result.Response;
            Debug.Log(identityId);
        });/**/

        //string Result = await Credentials.GetIdentityIdAsync();
        //InitAWS;
        //Task.Run(()=>InitAWS());
    }

    async Task InitAWS()
    {
        string Result = await Credentials.GetIdentityIdAsync();
        Debug.Log("InitAWS : " + Result);
    }

    //data를 써서 filename으로 업로드
    public void UploadFile(byte[] data, string filename)
    {
        Debug.Log("UploadFile start");

        MemoryStream ms = new MemoryStream(data);

        Debug.Log("UploadFile 2");
        PutObjectRequest request = new PutObjectRequest()
        {
            BucketName = bucketname,
            Key = filename,
            InputStream = ms,
            CannedACL = S3CannedACL.Private,
        };

        Debug.Log("UploadFile 3");
        if (_S3Client == null)
        {
            //_S3Client = new AmazonS3Client(new CognitoAWSCredentials(IdentityPoolID, RegionEndpoint.APNortheast2), _S3Region);
            //_S3Client = new AmazonS3Client("AKIAX5EHWC7R6T3ZRPDE", "8FoOlZX3b5pHFKs1lFnsIjFFljmhxQfq9OjpHJzs", _S3Region);
            _S3Client = new AmazonS3Client("AKIAX5EHWC7R6T3ZRPDE", "8FoOlZX3b5pHFKs1lFnsIjFFljmhxQfq9OjpHJzs", RegionEndpoint.APNortheast2);
        }

        Debug.Log("UploadFile 4");
        PutObjectResponse response = S3Client.PutObject(request);
        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
        {
            Debug.Log("Success");
        }
    }

    public async Task<PutObjectResponse> UploadFileAsync(byte[] data, string filename)
    {
        MemoryStream ms = new MemoryStream(data);

        PutObjectRequest request = new PutObjectRequest()
        {
            BucketName = bucketname,
            Key = filename,
            InputStream = ms,
            CannedACL = S3CannedACL.Private,
        };
        return await S3Client.PutObjectAsync(request);
    }

    /*
    public InputField userNameSearch;
    public Text Email;
    public Text UserName;
    public Button SearchButton;
    public Text SearchButtonText;
    public Image DownLoadImage;
    byte[] downloadedImageData;
    public void Download()
    {
        if(userNameSearch.text != "")
        {
            UserInfo downloadedUserInfo = new UserInfo();
            string itemToDownload = userNameSearch.text;

            var request = new ListObjectsRequest()
            {
                BucketName = Bucketname,
            };

            S3Client.ListObjectsAsync(request, (responseObject) => 
            {
                S3Client.GetObjectAsync(Bucketname, itemToDownload, (responseObject) => 
                {
                    if(responseObject.Exception == null)
                    {
                        byte[] data = null;
                        using (StreamReader sr = new StreamReader(responseObject.Response.ResponseStream))
                        {
                            using(MemoryStream ms = new MemoryStream())
                            {
                                var buffer = new byte[50000];
                                var bytesRead = default(int);
                                while((bytesRead = sr.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    ms.Write(buffer, 0, bytesRead);
                                }
                                data = ms.ToArray();
                            }
                        }

                        ManageDownloadedPicture(data);
                        /*using(MemoryStream memory = new MemoryStream(data))
                        {
                            BinaryFormatter bf = new BinaryFormatter();

                            //downloadedUserInfo = (UserInfo)bf.Deserialize(memory);
                            //UserName.text = downloadedUserInfo.userName;
                            //Email.text = downloadedUserInfo.email;
                            //downloadedImageData = downloadedUserInfo.picture;

                            downloadedImageData = bf.Deserialize(memory).;
                        }

                        ManageDownloadedPicture(downloadedImageData);
                    }
                    else
                    {
                        UserName.text = "Not Found";
                        Email.text = "Error";
                        SearchButton.GetComponent<Image>().color = Color.red;
                    }
                });
            });
        }
        else
        {
            userNameSearch.text = "Fill in Info Please";
        }
    }

    public void ManageDownloadedPicture(byte[] newImageData)
    {
        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(newImageData);
        tex.Apply();
        Sprite newImage = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        DownLoadImage.sprite = newImage;
    }/**/
}
