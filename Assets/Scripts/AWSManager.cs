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

public class AWSManager : MonoBehaviour
{
    public void UsedOnlyForAOTCOdeGeneration()
    {
        AndroidJavaObject jo = new AndroidJavaObject("android.os.Message");
        int valueString = jo.Get<int>("what");
    }

    public Button UploadButton;
    public Text UploadButtonText;

    public string IdentityPoolID = "ap-northeast-2:c4e9edd9-92a0-4389-82d6-494b7c056f1c";
    public string Region = RegionEndpoint.APNortheast2.SystemName;
    public string identityId;
    private static AWSManager _instance;
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

    

    public string S3Region = RegionEndpoint.APNortheast2.SystemName;
    private RegionEndpoint _S3Region
    {
        get
        {
            return RegionEndpoint.GetBySystemName(S3Region);
        }
    }

    private AmazonS3Client _S3Client;

    public AmazonS3Client S3Client
    {
        get
        {
            if(_S3Client==null)
            {
                _S3Client = new AmazonS3Client(new CognitoAWSCredentials(IdentityPoolID, RegionEndpoint.APNortheast2), _S3Region);
            }
            return _S3Client;
        }
    }

    private void Start()
    {
#if UNITY_ANDROID
        // Toggles the dimmed out state (where status/navigation content is darker)
        //ApplicationChrome.dimmed = !ApplicationChrome.dimmed;

        // Set the status/navigation background color (set to 0xff000000 to disable)
        //pplicationChrome.statusBarColor = ApplicationChrome.navigationBarColor = 0xffff3300;

        // Makes the status bar and navigation bar visible (default)
        ApplicationChrome.statusBarState = ApplicationChrome.States.Visible;
        ApplicationChrome.navigationBarState = ApplicationChrome.States.Hidden;

        // Makes the status bar and navigation bar visible over the content (different content resize method) 
        //ApplicationChrome.statusBarState = ApplicationChrome.navigationBarState = ApplicationChrome.States.VisibleOverContent;

        // Makes the status bar and navigation bar visible over the content, but a bit transparent
        //ApplicationChrome.statusBarState = ApplicationChrome.navigationBarState = ApplicationChrome.States.TranslucentOverContent;

        // Makes the status bar and navigation bar invisible (animated)
        //ApplicationChrome.statusBarState = ApplicationChrome.navigationBarState = ApplicationChrome.States.Hidden;
#endif
        {
            Debug.Log("Start!");

            _instance = this;

            UnityInitializer.AttachToGameObject(this.gameObject);
            try
            {
                AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
            }
            catch(Exception e)
            {
                Debug.Log(3);
            }

            Credentials.GetIdentityIdAsync(delegate (AmazonCognitoIdentityResult<string> result)
            {
                if(result.Exception != null)
                {
                    Debug.Log(result.Exception);
                }
                identityId = result.Response;
                Debug.Log(identityId);
            });

        }
    }

    public void UpdateText(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open);

        PostObjectRequest request = new PostObjectRequest()
        {
            Bucket = "unitys3course9-ryoko",
            Key = "message.txt",
            InputStream = fs,
            CannedACL = S3CannedACL.Private,
            Region = _S3Region
        };

        S3Client.PostObjectAsync(request, (responseObj) =>
        {
            if (responseObj.Exception == null)
            {
                Debug.Log("Upload Success");
                UploadButton.GetComponent<Image>().color = Color.green;
                UploadButtonText.text = "Success!";
            }
            else
            {
                Debug.Log("Error " + responseObj.Exception);
                UploadButton.GetComponent<Image>().color = Color.red;
                UploadButtonText.text = "Error!";
            }
        });
    }

    //?????? ??????
    public void UploadPicture(string path, string _key)
    {
        FileStream fs = new FileStream(path, FileMode.Open);

        PostObjectRequest request = new PostObjectRequest()
        {
            Bucket = "unitys3course9-ryoko",
            Key = _key,
            InputStream = fs,
            CannedACL = S3CannedACL.Private,
            Region = _S3Region
        };

        S3Client.PostObjectAsync(request, (responseObj) =>
        {
            if(responseObj.Exception == null)
            {
                Debug.Log("Upload Success");
                UploadButton.GetComponent<Image>().color = Color.green;
                UploadButtonText.text = "Success!";
            }
            else
            {
                Debug.Log("Error " + responseObj.Exception);
                UploadButton.GetComponent<Image>().color = Color.red;
                UploadButtonText.text = "Error!";
            }
        });
    }

    //data?? ???? filename???? ??????
    public void UploadFile(byte[] data, string filename)
    {
        MemoryStream ms = new MemoryStream(data);

        PostObjectRequest request = new PostObjectRequest()
        {
            Bucket = "unitys3course9-ryoko",
            Key = filename,
            InputStream = ms,
            CannedACL = S3CannedACL.Private,
            Region = _S3Region
        };

        S3Client.PostObjectAsync(request, (responseObj) =>
        {
            if (responseObj.Exception == null)
            {
                Debug.Log("Upload Success");
                UploadButton.GetComponent<Image>().color = Color.green;
                UploadButtonText.text = "Success!";
            }
            else
            {
                Debug.Log("Error " + responseObj.Exception);
                UploadButton.GetComponent<Image>().color = Color.red;
                UploadButtonText.text = "Error!";
            }
        });
    }

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
                BucketName = "unitys3course9-ryoko",
            };

            S3Client.ListObjectsAsync(request, (responseObject) => 
            {
                S3Client.GetObjectAsync("unitys3course9-ryoko", itemToDownload, (responseObject) => 
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

                        using(MemoryStream memory = new MemoryStream(data))
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            downloadedUserInfo = (UserInfo)bf.Deserialize(memory);
                            UserName.text = downloadedUserInfo.userName;
                            Email.text = downloadedUserInfo.email;
                            downloadedImageData = downloadedUserInfo.picture;
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
    }
}
