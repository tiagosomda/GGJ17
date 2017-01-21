using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerMove : MonoBehaviour {

    [Header("Move Settings")]
    [Range(0f,10f)]
    public float moveSpeed;
    [Range(0f, 1f)]
    public float rotationSpeed;

    private Vector2 movement;
    private Rigidbody2D rb;
    private float rotationTimer;

    private float rotation;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        movement.x = Input.GetAxis("Horizontal") * moveSpeed;
        movement.y = Input.GetAxis("Vertical")   * moveSpeed;


        Vector3.Angle(movement, Vector3.forward);

        var temp = ((movement.x > 0) ? 270 : (movement.x < 0) ? 90f : (movement.y > 0) ? 0f : (movement.y < 0) ? 180 : rotation);
        if(temp != rotation)
        {
            rotation = temp;
            rotationTimer = 0;
        }
    }

    void FixedUpdate()
    {
        // move character x/y position
        rb.MovePosition((Vector2)transform.position + (movement * Time.fixedDeltaTime));

        // rotate character towards moving direction
        if (rotationTimer < rotationSpeed)
        {
            // smallest distance to angle
            var dif = Mathf.DeltaAngle(rb.rotation, rotation);

            // lerp between these angles 
            var rot = Mathf.Lerp(rb.rotation, rb.rotation + dif, rotationTimer);
            rb.MoveRotation(rot);

            //reduce rotation timer
            rotationTimer += Time.fixedDeltaTime;
        }
    }
}
