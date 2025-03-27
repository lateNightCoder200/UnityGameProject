using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEditor.PackageManager.Requests;
using static System.Net.WebRequestMethods;
using System.Security.Cryptography;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class RedirectManger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string email = PlayerPrefs.GetString("userEmail", "");

        string getUrl = "https://localhost:7223/CheckPatientInfo/" + email;
        StartCoroutine(FetchPatientInfo(getUrl));
    }

    public IEnumerator FetchPatientInfo(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            string token = PlayerPrefs.GetString("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);
            }

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                SceneManager.LoadScene("HomeScene");
            }
            else
            {
              
                SceneManager.LoadScene("SetPatientInfo");
            }
        }
    }
}
