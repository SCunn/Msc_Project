using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagers {
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [SerializeField] private UIManager uiManager;

        // public bool isGameStarted = false;

        // private bool isGameOver = false;

        private bool isPaused = false;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }

        public void TogglePause()
        {
            isPaused = !isPaused;
            if(isPaused)
            {
                
                Time.timeScale = 0;
                // AudioManager.Instance.PlaySoundEffect("Pause");
                Debug.Log("Paused");
            }
            else
            {
                Time.timeScale = 1;
                // AudioManager.Instance.PlaySoundEffect("Unpause");
                Debug.Log("Unpaused");
            }
            uiManager.TogglePauseMenu(isPaused);
        }
    }
}
