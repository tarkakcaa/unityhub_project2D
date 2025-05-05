using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Platformer
{
    public class GameManager : MonoBehaviour
    {
        public int coinsCounter = 0;

        public GameObject playerGameObject;
        private PlayerController player;
        public GameObject deathPlayerPrefab;
        public Text coinText;
        public GameObject gamerestart, next;
        public int target = 10;
        void Start()
        {
            Time.timeScale = 1;

            player = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        void Update()
        {
            coinText.text = coinsCounter.ToString()+"/"+target;
            if(player.deathState == true)
            {
                playerGameObject.SetActive(false);
                GameObject deathPlayer = (GameObject)Instantiate(deathPlayerPrefab, playerGameObject.transform.position, playerGameObject.transform.rotation);
                deathPlayer.transform.localScale = new Vector3(playerGameObject.transform.localScale.x, playerGameObject.transform.localScale.y, playerGameObject.transform.localScale.z);
                player.deathState = false;
                Invoke("ReloadLevel", 3);
            }
            if(player.door == true)
            {
                if (coinsCounter >= target)
                {
                    next.SetActive(true);
                    Time.timeScale = 0;

                }
            }
        }

        private void ReloadLevel()
        {
            gamerestart.SetActive(true);

            
        }
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void RestartGame()
        {
            SceneManager.LoadScene(0);
        }
        public void Next()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
    }
}
