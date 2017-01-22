using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerMove : MonoBehaviour {

    [Header("Move Settings")]
    [Range(0f,10f)]
    public float moveSpeed;
    [Range(0f, 10f)]
    public float rotationSpeed;

    private Vector2 movement;
    private Rigidbody2D rb;
    private float rotationTimer;

	//key binding variables
	private string upButton, downButton, leftButton, rightButton;

    private float rotation;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

		CheckInput ();

		//movement.x = Input.GetAxis("Horizontal") * moveSpeed;
        //movement.y = Input.GetAxis("Vertical")   * moveSpeed;
	

        // if there is no input - then don't update the rotation
        if (movement.x == 0 && movement.y == 0)
        {
            return;
        }

        var temp = Vector2.Angle(Vector2.up, movement);
        if (movement.x > 0)
        {
            temp *= -1;
        }
        
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





	private void CheckInput() {
		 
		if (!Input.anyKey) {
			movement = Vector2.zero;
		}

		if (Input.GetKey (upButton)) {
			movement.y = moveSpeed;
		} 
		if (Input.GetKey(downButton)) {
			movement.y = -moveSpeed;
		} 
		if (Input.GetKey(leftButton)) {
			movement.x = -moveSpeed;
		} 
		if (Input.GetKey(rightButton)) {
			movement.x = moveSpeed;
		}

		if (movement.x != 0 && movement.y != 0) {
			movement = Vector2.zero;
		}
					
	}


	void Awake() {

		//initialize player key bindings
		if (PlayerPrefs.HasKey("Up")) {
			upButton = PlayerPrefs.GetString ("Up");

		}
		else {
			upButton = "w";
		}

		if (PlayerPrefs.HasKey("Down")) {
			downButton = PlayerPrefs.GetString ("Down");
		}
		else {
			downButton = "s";
		}

		if (PlayerPrefs.HasKey("Left")) {
			leftButton = PlayerPrefs.GetString ("Left");
		}
		else {
			leftButton = "a";
		}

		if (PlayerPrefs.HasKey("Right")) {
			rightButton = PlayerPrefs.GetString ("Right");
		}
		else {
			rightButton = "d";
		}
			
			
	}

}
