using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [Header("Move Settings")]
    [Range(0f, 10f)]
    public float moveSpeed;
    [Range(0f, 10f)]
    public float rotationSpeed;

	public bool hasControl= true;
	private string lastKey;

	public Vector3 movement;
    private Rigidbody rb;
    private float rotationTimer;
	private float rotation;



    //key binding variables
    private string upButton, downButton, leftButton, rightButton;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

		if (hasControl) {

			movement = Vector2.zero;

			CheckInput ();

			//use last key pressed to determine movement
			if (lastKey == upButton) 
			{
				movement.z = moveSpeed;
			}
			if (lastKey == downButton) 
			{
				movement.z = -moveSpeed;
			}
			if (lastKey == leftButton)
			{
				movement.x = -moveSpeed;
			}
			if (lastKey == rightButton)
			{
				movement.x = moveSpeed;
			}
		}

        //movement.x = Input.GetAxis("Horizontal") * moveSpeed;
        //movement.z = Input.GetAxis("Vertical")   * moveSpeed;

        // if there is no input - then don't update the rotation
        //if (movement.x == 0 && movement.z == 0)
        //{
        //    return;
        //}
        //
        //var temp = Vector2.Angle(Vector2.up, movement);
        //if (movement.x > 0)
        //{
        //    temp *= -1;
        //}
        //
        //if (temp != rotation)
        //{
        //    rotation = temp;
        //    rotationTimer = 0;
        //}

    }

    void FixedUpdate()
    {
        // move character x/y position
        rb.MovePosition(transform.position + (movement * Time.fixedDeltaTime));

        // rotate character towards moving direction
        //if (rotationTimer < rotationSpeed)
        //{
        //    // smallest distance to angle
        //
        //    var wow = new Vector2(rb.rotation.eulerAngles.x, rb.rotation.eulerAngles.z);
        //    var dif = Mathf.DeltaAngle(wow, rotation);
        //
        //    // lerp between these angles 
        //    var rot = Mathf.Lerp(rb.rotation, rb.rotation + dif, rotationTimer);
        //    rb.MoveRotation(rot);
        //
        //    //reduce rotation timer
        //    rotationTimer += Time.fixedDeltaTime;
        //}
    }

    private void CheckInput()
    {


       	//store last key pressed
		if (Input.GetKeyDown(upButton))
        {
			lastKey = upButton;
		}
        if (Input.GetKeyDown(downButton))
        {
			lastKey= downButton;
        }
        if (Input.GetKeyDown(leftButton))
        {
			lastKey= leftButton;
		}
        if (Input.GetKeyDown(rightButton))
        {
			lastKey= rightButton;
		}

		//cancel last key if key is released
		if (Input.GetKeyUp(upButton) && lastKey == upButton)
		{
			lastKey = null;
		}
		if (Input.GetKeyUp(downButton) && lastKey == downButton)
		{
			lastKey = null;
		}
		if (Input.GetKeyUp(leftButton) && lastKey == leftButton)
		{
			lastKey = null;
		}
		if (Input.GetKeyUp(rightButton) && lastKey == rightButton)
		{
			lastKey = null;
		}
    }


    void Awake()
    {

        //initialize player key bindings
        if (PlayerPrefs.HasKey("Up"))
        {
            upButton = PlayerPrefs.GetString("Up");

        }
        else
        {
            upButton = "w";
        }

        if (PlayerPrefs.HasKey("Down"))
        {
            downButton = PlayerPrefs.GetString("Down");
        }
        else
        {
            downButton = "s";
        }

        if (PlayerPrefs.HasKey("Left"))
        {
            leftButton = PlayerPrefs.GetString("Left");
        }
        else
        {
            leftButton = "a";
        }

        if (PlayerPrefs.HasKey("Right"))
        {
            rightButton = PlayerPrefs.GetString("Right");
        }
        else
        {
            rightButton = "d";
        }


    }

}