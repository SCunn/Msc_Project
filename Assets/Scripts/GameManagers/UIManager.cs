using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GameManagers {
    public class UIManager : MonoBehaviour
    {
        public GameObject pauseMenu;
        

        public void TogglePauseMenu(bool isPaused)
        {
            pauseMenu.SetActive(isPaused);
        }
    }
}