# 3D Platformer
Fundamentals of Game Development (FGCT4015)

Bradley Curtis

2309516

## Research
### Relevant sources and references

- As I have worked on 3D games in Unity in the past I wanted to challlenge myself to see if i would be able to implement a system I havent used before and I decided on a grappling hook. I wanted to specifically also use tools within unity that I havent done before so the grappling hook and using Spring Joints to make this work was my main goal within the task. 

- I wanted to the grapple to feel less like a traditional grappling hook where the player is just pulled to the hook location but rather to be similar to a swing connected to both the player and wherever the hook lands. When the grapple swing was working, the grapple gun looked very stiff within the players hands so i also wanted to learn how to make it look towards where the spring joint had been created so I also learnt how to use Quaternion.Lerp (Technologies, n.d.)

 - My main help with the implementation of Spring Joints and using Lerp was the official Unreal Documentation (Technologies, n.d.)


## Implementation
- Initially, I planned to make a simple 3D Platformer in terms of basically just jumping from object to object while avoiding traps but after a while of working on that I found myself getting distraced and trying to make a swinging mechanic which was very off topic but I followed through with it anyway.

```csharp
    private void Awake()
    {
        rope = GetComponent<LineRenderer>();
        scoreText = GameObject.Find("score").GetComponent<TMP_Text>();

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Grapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }

        if (springJoint != null && Input.GetKey(KeyCode.E))
        {
            springJoint.maxDistance = Mathf.Max(0, springJoint.maxDistance - 0.5f);
        }
    }
    private void LateUpdate()
    {
        Rope();
    }
    private void Grapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.position, Camera.forward, out hit, maxDistance))
        {
            if (hit.collider.CompareTag("ScoreOBJ"))
            {
                score++;
                scoreText.text = "Score: " + score.ToString();
                tempPoint = hit.collider.gameObject;
            }
            grapplePoint = hit.point;
            springJoint = Player.AddComponent<SpringJoint>();
            springJoint.autoConfigureConnectedAnchor = false;
            springJoint.connectedAnchor = grapplePoint;
            float playerDistance = Vector3.Distance(Player.position, grapplePoint);
            // springJoint.maxDistance = playerDistance * 0.8f;
            springJoint.maxDistance = playerDistance * sj_maxDistance;
            springJoint.spring = sj_spring;
            springJoint.damper = sj_damper;
            springJoint.massScale = sj_massScale;
            rope.positionCount = sj_postionCount;
        }
    }
    private void StopGrapple()
    {
        rope.positionCount = 0;
        Destroy(springJoint);

        if (tempPoint == null) return;
        Destroy(tempPoint);

    }
    public bool isGrappling()
    {
        return springJoint != null;
    }
    public Vector3 GrapplePoint()
    {
        return grapplePoint;
    }
    private void Rope()
    {
        if (!springJoint)
        {
            return;
        }
        rope.SetPosition(0, lineRendererPoint.position);
        rope.SetPosition(1, grapplePoint);
    }
}
```

*Implementation of the Grapple Gun*
- I also added rotation to the grapple gun as it looked stiff and weird when the rope created by the line renderer would move around but the gun would always face forward so i added rotation using Quaternions.

```csharp

public class GrappleRotation : MonoBehaviour
{
   public GrappleGunFix grapple;
    private Quaternion rotationAngle;
    private float rotateSpeed = 5f;

    private void Update()   
    {
        if (!grapple.isGrappling())
        {
            rotationAngle = transform.parent.rotation;
        }
        else
        {
            rotationAngle = Quaternion.LookRotation(grapple.GrapplePoint() - transform.position);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationAngle, Time.deltaTime * rotateSpeed);
    }
}
```
![Grapple Hook](https://raw.githubusercontent.com/2309516/3D-Platformer/refs/heads/main/GrappleHookImage.png)
- After the initial prototype of the grapple, I asked some of my peers to play the game and a common problem came up, which was that the movement was way to fast and the grapple was unintuitive. 

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Range(0f,1000f)]public float playerSpeed = 15f;
    public float jumpForce = 30f, groundDrag = 5f, airControlFactor = 0.5f, maxVelocity = 10f;

    private Rigidbody _rb;
    private bool _isGrounded;
    private Camera _mainCamera;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _isGrounded = false;
        }
        Debug.Log("Player Velocity: " + _rb.velocity.magnitude);
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitToMenu();
        }
    }

    private void FixedUpdate()
    {
        Vector3 cameraForward = Vector3.Scale(_mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = (cameraForward * Input.GetAxis("Vertical") + _mainCamera.transform.right * Input.GetAxis("Horizontal")).normalized;
        
        _rb.AddForce(movement * playerSpeed * (_isGrounded ? 1 : airControlFactor), ForceMode.Force);

        if (_rb.velocity.magnitude > maxVelocity)
        {
            _rb.velocity = _rb.velocity.normalized * maxVelocity;
        }

        _isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, 2f, ~0);
        _rb.drag = _isGrounded ? groundDrag : 0f;
    }

    private void ExitToMenu()
    {
        SceneManager.LoadScene(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Restart()
    {
        SceneManager.LoadScene(1);
    }
}
```
*Updated movement script to add drag on the player when they're on the ground and limit the players velocity to a set speed*

## Outcome
![GameEngineImage](https://raw.githubusercontent.com/2309516/3D-Platformer/refs/heads/main/TetheredTrialsGameEngineIMG.png)

[Link to gameplay video demonstration](https://youtu.be/yAdk6yU182E)

[Link to the game on Itch.io](https://popegames.itch.io/tethered-trials)

[Link to github repository](https://github.com/2309516/3D-Platformer)

## Critical Reflection  
 
- One of the biggest problems I had in creating the game was actually etting the grapple hook to work as for the longest time it kept on creating the 2nd Spring joint at 0,0,0 rather than at where the raycast was hitting. This was due to the Spring Joint being created in local space rather than world space. 

- I spent a lot of my time tring to build a world which was unneccesary and if I didnt focus so much on that I couldve refined the player movement a little bit more and make the player feel less floaty after letting go of the grapple.

- Next time I will spend a bit more time thinking of a gameplay direction rather then just jumping in and trying to implement whatever comes into my head as this wasted a lot of my time on the project. I find myelf jumping around from script to script and from ideas when I dont have a clear direction of what I am trying to do. In the future I will plan more in detail of what I want to do next and have a better schedule in place.

- I am very happy with how the grappling hook turned out and that i was able to get it to work properly in the end. 

## Bibliography

Technologies, U. (n.d.). Unity - Manual: Spring Joint component reference. [online] docs.unity3d.com. Available at: https://docs.unity3d.com/Manual/class-SpringJoint.html.

Technologies, U. (n.d.). Unity - Scripting API: Quaternion.Lerp. [online] docs.unity3d.com. Available at: https://docs.unity3d.com/ScriptReference/Quaternion.Lerp.html.

‌Unity Technologies (2016). How do I import a custom image and put it on the canvas/pause menu? [online] Unity Discussions. Available at: https://discussions.unity.com/t/how-do-i-import-a-custom-image-and-put-it-on-the-canvas-pause-menu/172499 [Accessed 12 Nov. 2024].

‌

## Declared Assets

Kenney (2018). Crosshair Pack · Kenney. [online] Kenney.nl. Available at: https://kenney.nl/assets/crosshair-pack [Accessed 4 Oct. 2024].

assetstore.unity.com. (n.d.). SimplePoly City - Low Poly Assets | 3D Environments | Unity Asset Store. [online] Available at: https://assetstore.unity.com/packages/3d/environments/simplepoly-city-low-poly-assets-58899.

‌Cyberwave-Orchestra (2024). Upbeat Mission – Fun and Quirky Adventure | Royalty-free Music. [online] Pixabay.com. Available at: https://pixabay.com/music/bloopers-upbeat-mission-fun-and-quirky-adventure-248802/ [Accessed 12 Nov. 2024].

‌
‌
The following assets were created or modified with the use of GPT 4o:
- Grapple Gun Fix.cs
- Player Controller.cs
- Grapple Rotation.cs
‌


