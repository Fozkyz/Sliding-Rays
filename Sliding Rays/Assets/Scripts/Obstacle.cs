using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameManager manager;

    public int grid_size;
    public float speed;
    public LineRenderer line1, line2;

    // Start is called before the first frame update
    void Start()
    {
        CreateObstacle();
        manager = gameObject.GetComponentInParent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.is_paused)
		{
            MoveObstacle();
		}
    }

	void CreateObstacle()
	{
        int hole = Random.Range(-grid_size / 2 + 1, grid_size / 2 - 1);
        line1.SetPosition(1, new Vector3(hole - 1, 0, 0));
        AdaptCollider(line1);
        line2.SetPosition(1, new Vector3(hole + 1, 0, 0));
        AdaptCollider(line2);
    }

    void MoveObstacle()
	{
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (Mathf.Abs(transform.position.x) > (float) grid_size / 2 || Mathf.Abs(transform.position.y) > (float) grid_size / 2)
		{
            manager.PlayerScored();
            Destroy(gameObject);
		}
	}
    
    void AdaptCollider(LineRenderer line)
	{
        BoxCollider2D col = line.gameObject.GetComponent<BoxCollider2D>();
        if (col != null)
		{
            float distX = Mathf.Abs(line.GetPosition(0).x - line.GetPosition(1).x);
            float distY = Mathf.Abs(line.GetPosition(0).y - line.GetPosition(1).y);
            if (distX < distY)
			{
                float coordY = line.GetPosition(0).y + line.GetPosition(1).y;
                col.size = new Vector2(col.size.x, distY);
                col.offset = new Vector2(col.offset.x, coordY / 2);
			}
			else
			{
                float coordX = line.GetPosition(0).x + line.GetPosition(1).x;
                col.size = new Vector2(distX, col.size.y);
                col.offset = new Vector2(coordX / 2, col.offset.y);
            }
		}
	}
}
