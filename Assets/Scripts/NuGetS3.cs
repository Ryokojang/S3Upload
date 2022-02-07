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

public class NuGetS3 : MonoBehaviour
{
    private const string bucketName = "unitys3course9-ryoko";
    //private const string keyName = "";
    //private const string filePath = "";
    private const string accessKeyID = "AKIAX5EHWC7R6T3ZRPDE";
    private const string accessSecretKey = "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";
    private static readonly RegionEndpoint buckertRegion = RegionEndpoint.APNortheast2;

    public  async Task UploadFileAsync(string filePath, string keyName)
    {
        try
        {
            Debug.Log("UploadFileAsync");
            var fileTransferUtility = new TransferUtility(accessKeyID, accessSecretKey, buckertRegion);

            using(var fileToUpload = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                await fileTransferUtility.UploadAsync(fileToUpload, bucketName, keyName);
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void UploadFileAsync2(string filePath, string keyName)
    {
        try
        {
            Debug.Log("UploadFileAsync");
            var fileTransferUtility = new TransferUtility(accessKeyID, accessSecretKey, buckertRegion);

            using (var fileToUpload = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                fileTransferUtility.UploadAsync(fileToUpload, bucketName, keyName);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

}
