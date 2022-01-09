using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Amazon.CognitoIdentity;
using System;

public class AWSManager : MonoBehaviour
{
    public string IdentityPoolID = "ap-northeast-2:c4e9edd9-92a0-4389-82d6-494b7c056f1c";
    public string Region = RegionEndpoint.APNortheast2.SystemName;

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

    //public string identityId;

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
                string identityId = result.Response;
                Debug.Log(identityId);
            });

        }
    }


    public void Upload(string path)
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
            if(responseObj.Exception == null)
            {
                Debug.Log("Upload Success");
            }
            else
            {
                Debug.Log("Error " + responseObj.Exception);
            }
        });
    }
}
