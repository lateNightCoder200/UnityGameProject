using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using Newtonsoft.Json;
using TMPro;
using System.Linq;
using UnityEditor.PackageManager.Requests;

public class SetPatientnfoSceneManger : MonoBehaviour
{

    public TMP_InputField firstNameInput;
    public TMP_InputField lastNameInput;
    public TMP_InputField cityInput;
    public TMP_InputField birthDateInput;
    public TMP_InputField hospitalInput;

    public Button submmitButton;
    public TextMeshProUGUI errorText;

    private string apiUrl = "https://localhost:7223/PatientInfo";

    void Start()
    {
        submmitButton.onClick.AddListener(OnClicked);
    }

    void OnClicked()
    {
        string email = PlayerPrefs.GetString("userEmail");

        string firstName = firstNameInput.text;
        string lastName = lastNameInput.text;
        string birthDate = birthDateInput.text;
        string city = cityInput.text;
        string hospital = hospitalInput.text;

        if (string.IsNullOrEmpty(firstName))
            errorText.text = "First name is required!";

        if (string.IsNullOrEmpty(lastName))
            errorText.text = "Last name is required!";

        if (string.IsNullOrEmpty(birthDate))
            errorText.text = "Birth date is required!";

        if (string.IsNullOrEmpty(city))
            errorText.text = "Cityis is required!";

        if (string.IsNullOrEmpty(hospital))
            errorText.text = "Hospital is required!";

        else
        {
           
            StartCoroutine(SetPatientInfoRequest(email, firstName, lastName, birthDate, city, hospital));

        }
    }

    IEnumerator SetPatientInfoRequest(string Email, string firstName, string lastName, string birthDate, string city, string hospital )
    {

       
        PatientInfo requestData = new PatientInfo 
        {
            email = Email,
            firstName = firstName,
            lastName = lastName,
            birthDate = birthDate,
            city = city,
            hospital = hospital
        };

       

        string jsonData = JsonConvert.SerializeObject(requestData);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl  , "POST"))
        {

            string token = PlayerPrefs.GetString("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);
            }

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                SceneManager.LoadScene("SetUserName");
                
            }
            else
            {
                errorText.text = "Error: " + request.downloadHandler.text;

                
            }
        }
    }


}
 
class PatientInfo {

    public string email;
    public string firstName;
    public string lastName;
    public string birthDate;
    public string city;
    public string hospital;
};