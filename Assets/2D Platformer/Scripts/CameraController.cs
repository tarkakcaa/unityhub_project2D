using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class CameraController : MonoBehaviour
    {
        public float damping = 1.5f; // hareket h�z�
        public Vector2 offset = new Vector2(0f, 0f); // karakterin ekran�n ortas�nda olmamas�n� istiyoruz onun i�in
        public bool faceLeft; //  y eksenine yans�ma yapmaya yarar
        private Transform player;
        private int lastX;
        void Start () {
            offset = new Vector2(Mathf.Abs(offset.x), offset.y);
            FindPlayer(faceLeft);
        }
        public void FindPlayer(bool playerFaceLeft)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            lastX = Mathf.RoundToInt(player.position.x);
            if (playerFaceLeft)
            {
                transform.position = new Vector3(player.position.x - offset.x, player.position.y + offset.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
            }
        }
        void Update () {
            if (player)
            {
                int currentX = Mathf.RoundToInt(player.position.x);
                if (currentX > lastX) faceLeft = false; else if (currentX < lastX) faceLeft = true;
                lastX = Mathf.RoundToInt(player.position.x);

                Vector3 target;
                if (faceLeft)
                {
                    target = new Vector3(player.position.x - offset.x, player.position.y + offset.y, transform.position.z);
                }
                else
                {
                    target = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
                }
                Vector3 currentPosition = Vector3.Lerp(transform.position, target, damping * Time.deltaTime);
                transform.position = currentPosition;
            }
        }
    }
}

