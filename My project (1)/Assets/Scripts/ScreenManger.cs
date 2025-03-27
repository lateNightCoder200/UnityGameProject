using UnityEngine;
using UnityEngine.SceneManagement;
public class ScreenManger : MonoBehaviour
{
     

    public void GoToSignUpPage()
    {
        SceneManager.LoadScene("SignUpScene");
    }

    public void GoToLoginPage()
    {
        SceneManager.LoadScene("LoginScene");
    }
    

    public void GoToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void GoToHomeScene()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void GoToMyProfileScene()
    {
        SceneManager.LoadScene("MyProfileScene");
    }

    public void GoToUpdatePatientInfoScene()
    {
        SceneManager.LoadScene("UpdatePatientInfoScene");
    }

  
    public void GoToUpdateUserNameScene()
    {
        SceneManager.LoadScene("UpdateUserNameScene");
    }

    
}
