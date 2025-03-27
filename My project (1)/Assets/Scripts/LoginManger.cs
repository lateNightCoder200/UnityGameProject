using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using Newtonsoft.Json;
using TMPro;


public class LoginManger : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public TextMeshProUGUI errorText;

    private string apiUrl = "https://localhost:7223/account/login";

    void Start()
    {
        loginButton.onClick.AddListener(OnLoginClicked);
    }

    void OnLoginClicked()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            errorText.text = "Email and Password are required!";
            return;
        }

        StartCoroutine(LoginRequest(email, password));
    }

    IEnumerator LoginRequest(string email, string password)
    {

        var loginData = new { email = email, password = password };
        string jsonData = JsonConvert.SerializeObject(loginData);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {

                LoginResponse response = JsonConvert.DeserializeObject<LoginResponse>(request.downloadHandler.text);

                if (!string.IsNullOrEmpty(response.accessToken))
                {

                    PlayerPrefs.SetString("authToken", response.accessToken);
                    PlayerPrefs.SetString("refreshToken", response.refreshToken);
                    PlayerPrefs.SetString("tokenType", response.tokenType);
                    PlayerPrefs.SetInt("expiresIn", response.expiresIn);
                    PlayerPrefs.SetString("userEmail", email);
                    PlayerPrefs.Save();

                    errorText.text = "Login Successful!";
                    SceneManager.LoadScene("RedirectScene");

                }
                else
                {
                    errorText.text = "Invalid login";
                }
            }
            else
            {
                errorText.text = "Your email or password is not valid!";
            }
        }
    }
}

[System.Serializable]
public class LoginResponse
{
    public string tokenType;
    public string accessToken;
    public int expiresIn;
    public string refreshToken;
}