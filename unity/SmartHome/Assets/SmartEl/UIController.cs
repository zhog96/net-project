using UnityEngine;
using UnityEngine.UI;

namespace SmartEl
{
    public class UIController : MonoBehaviour
    {
        public Text Input;

        void Start () {
            // Button btn = yourButton.GetComponent<Button>();
            // btn.onClick.AddListener(TaskOnClick);
        }

        void TaskOnClick(){
            print ("You have clicked the button!");
        }

        public void OnClick()
        {
            print ("You have clicked the button + " + Input.text + "!");
        }
        
        public void QuitGame()
        {
            Application.Quit();;
        }
    }
}