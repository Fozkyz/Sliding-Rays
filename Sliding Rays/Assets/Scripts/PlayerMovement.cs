using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameManager manager;
    public ParticleSystem particle_system;

    public int grid_size;
    public float speed;

    [SerializeField] Vector2 direction;
    Vector2 destination;

	// Update is called once per frame
	void Update()
    {
        direction = GetDirection();
        destination = (Vector2) transform.position + direction;
        destination.x = Mathf.Clamp(destination.x, -grid_size / 2, grid_size / 2);
        destination.y = Mathf.Clamp(destination.y, -grid_size / 2, grid_size / 2);
        transform.position = destination;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Obstacle"))
		{
            particle_system.Play();
            manager.PlayerLost();
		}
	}

	Vector2 GetDirection()
	{
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(horizontal, vertical);
        if (dir.magnitude > 1)
            dir = dir.normalized;
        return dir * Time.deltaTime * speed;
	}
}
