using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 30f;

    Rigidbody rigidBody;
    AudioSource thruster;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    bool thruster_play;
    bool thruster_ToggleChange;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        thruster = GetComponent<AudioSource>();
        thruster_play = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        } 
	}

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag) 
        {

            case "Friendly":
                //Do nothing
                print("cabbage");
                Invoke("PrintMore" , 1.5f);
                print("apple");
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextScene" , 1f);
                break;
            default:
                print("Dead");
                state = State.Dying;
                Invoke("LoadFirstLevel", 1f);
                break;
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
        {
            float thrustPower = mainThrust * Time.deltaTime;
            rigidBody.AddRelativeForce(Vector3.up * thrustPower);
            if (!thruster.isPlaying)
            {
                thruster.Play();
            }

        }
        else
        {
            thruster.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; //take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //resume physics control of rotation
    }
}
