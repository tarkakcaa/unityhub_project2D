namespace Platformer
{
    using UnityEngine;

    public class EnemyAI : MonoBehaviour
    {
        public float speed = 2f;                 // Patrol speed
        public float patrolDistance = 5f;       // Patrol range
        public float chaseSpeed = 4f;           // Speed while chasing the player
        public float detectionRange = 3f;       // Horizontal player detection range
        private Vector3 startPosition;          // Starting position for patrol
        private bool movingRight = true;        // Direction of patrol
        public Transform player;               // Reference to the player
        private bool isChasing = false;         // Flag for chasing state
        private float currentPatrolDistance;    // Track current distance traveled during patrol

        void Start()
        {
            startPosition = transform.position;
            player = GameObject.FindGameObjectWithTag("Player").transform; // Find player by tag
        }

        void Update()
        {
            if (player.gameObject.activeSelf)
            {
                // Check only the horizontal distance
                float playerHorizontalDistance = Mathf.Abs(player.position.x - transform.position.x);

                // Ensure the player is also at roughly the same vertical level
                float verticalDifference = Mathf.Abs(player.position.y - transform.position.y);

                if (playerHorizontalDistance <= detectionRange && verticalDifference <= 0.5f&&!player.gameObject.GetComponent<PlayerController>().b) // Tweak vertical tolerance if needed
                {
                    // Player is within the horizontal detection range and same vertical level
                    isChasing = true;
                    ChasePlayer();
                }
                else
                {
                    // Player is out of detection range, return to patrol
                    isChasing = false;
                    Patrol();
                }
            }
        }

        void Patrol()
        {
            if (movingRight)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                currentPatrolDistance += speed * Time.deltaTime;

                if (currentPatrolDistance >= patrolDistance)
                {
                    movingRight = false;
                    Flip();
                }
            }
            else
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime);
                currentPatrolDistance -= speed * Time.deltaTime;

                if (currentPatrolDistance <= 0f)
                {
                    movingRight = true;
                    Flip();
                }
            }
        }

        void ChasePlayer()
        {
            // Move towards the player's position, only horizontally
            Vector3 direction = new Vector3(player.position.x - transform.position.x, 0, 0).normalized;
            transform.Translate(direction * chaseSpeed * Time.deltaTime);

            if (player.position.x > transform.position.x && !movingRight)
            {
                movingRight = true;
                Flip();
            }
            else if (player.position.x < transform.position.x && movingRight)
            {
                movingRight = false;
                Flip();
            }
        }

        void Flip()
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        void OnDrawGizmos()
        {
            if (startPosition != Vector3.zero)
            {
                Gizmos.color = Color.green;
                // Draw patrol area line
                Gizmos.DrawLine(startPosition - Vector3.right * patrolDistance, startPosition + Vector3.right * patrolDistance);

                Gizmos.color = Color.red;
                // Draw horizontal detection range as a line
                Gizmos.DrawLine(transform.position - Vector3.right * detectionRange, transform.position + Vector3.right * detectionRange);
            }
        }
    }
}
