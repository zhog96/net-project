using UnityEngine;
using UnityEngine.UI;

namespace SmartEl
{
    public class UIController : MonoBehaviour
    {
        public Text IP;
        public Text Password;
        public Button ButtonToRequestHost;
        public Button ButtonToRequestGuest;
        public GameObject Interface;
        public bool stateInterface = false;

        void Start()
        {
            // Button btn = yourButton.GetComponent<Button>();
            // btn.onClick.AddListener(TaskOnClick);
        }

        public void ShowAndHideMenu()
        {
            print("Escape was pressed");
            stateInterface = !stateInterface;
            Interface.SetActive(stateInterface);
        }

        void TaskOnClick()
        {
            print("You have clicked the button!");
        }

        public void OnClick()
        {
            print("You have clicked the button + " + IP.text + "!");
        }

        public void QuitGame()
        {
            Application.Quit();
            ;
        }
    }
}