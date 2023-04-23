using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterController2D : MonoBehaviour
{
    ImageCapture ic;
    RawImageCapture raw;
    ModeSwitcher switcher;
    CharacterController3D cc3d;

    public Vector2Int playerPosition;
    public Vector2 playerPositionFloat;
    public Vector2 moveDirection;
    Vector2 pointerPosition;

    public Vector3 respawnPosition;
    public Vector3 worldPosition;
    public Vector3 checkPointPosition;
    public Vector3 initialSpawnPoint;

    public GameObject player2D;

    public RectTransform playerRectTransform;
    public RectTransform pointer;
    public Transform playerTransform;
    public Image playerImage;
    public Image animatedPlayerImage;
    public Image pointerImage;

    public int colliderRadius;
    int collisionResolution = 60;

    float playerSpd = 600;
    float playerGravity = 10;
    float JumpHeight = 2.5f;

    List<Vector2> collisionPoints;

    public bool isInCollider;
    public bool isGrounded;
    public bool isInAir;
    public bool isCeiling;
    public bool isOnScreen;
    public bool jumping;

    bool isMovementBlocked = false;
    bool killFromOuchie;

    Camera visualCamera;

    public UnityEvent onDie;
    public UnityEvent onRespawnCheckpoint;

    float inAirDistanceThreshold = 25;
    Vector2 lastGroundedPos;

    public void _Start()
    {
        ic = UpdateDriver.ud.GetComponent<ImageCapture>();
        raw = UpdateDriver.ud.GetComponent<ImageCapture>().raw;
        switcher = UpdateDriver.ud.GetComponent<ModeSwitcher>();
        cc3d = UpdateDriver.ud.GetComponent<CharacterController3D>();

        visualCamera = GameObject.FindGameObjectWithTag("Visual Camera").GetComponent<Camera>();

        pointer = GameObject.FindGameObjectWithTag("Pointer 2D").GetComponent<RectTransform>();
        pointerImage = pointer.gameObject.GetComponent<Image>();

        player2D = GameObject.FindGameObjectWithTag("Player 2D");
        playerRectTransform = player2D.GetComponent<RectTransform>();
        playerTransform = player2D.transform;
        playerImage = player2D.GetComponent<Image>();//TEMPORARY
        animatedPlayerImage = GameObject.FindGameObjectWithTag("AnimatedPlayer").GetComponent<Image>();

        //sets Constant size for player in different resolutions. 
        playerRectTransform.sizeDelta = new Vector2(Screen.width/64, Screen.height/18);
        animatedPlayerImage.rectTransform.sizeDelta = new Vector2(playerRectTransform.sizeDelta.y*1.5f, playerRectTransform.sizeDelta.y * 1.5f);

        playerPosition = CharacterController2D.Vector2IntFromVector3(playerRectTransform.anchoredPosition);
        playerPositionFloat = playerPosition;
        colliderRadius = Mathf.RoundToInt(playerRectTransform.sizeDelta.x/2f);

        collisionPoints = new List<Vector2>();
        cast3DPoint();

        if (initialSpawnPoint != new Vector3(-1, -1, -1)) { worldPosition = initialSpawnPoint;  }
        respawnPosition = worldPosition;

        onDie = new UnityEvent();
        onRespawnCheckpoint = new UnityEvent();
    }

    public void _Update()
    {
        isOnScreen = playerRectTransform.anchoredPosition3D.z > 0;

        if (UpdateDriver.ud.GetComponent<ModeSwitcher>().firstPerson) { inactiveMode(); return; }

        if (ic.texture == null) { return; }

        if (!isMovementBlocked)
        {
            movePlayer();

            playerPosition = CharacterController2D.Vector2IntFromVector3(playerPositionFloat);
            playerRectTransform.anchoredPosition = playerPosition;
            raw.capturePos = new Vector2Int(playerPosition.x - (raw.captureDimensions.x / 2), playerPosition.y - (raw.captureDimensions.y / 2));

            //set 3d position in space
            cast3DPoint();

            killConditions();
        }

        Destroy(ic.texture);//SUPER SUPER IMPORTANT FOR PERFORMANCE
    }

    void killConditions()
    {
        bool offscreen = playerPositionFloat.x < 0 ||
            playerPositionFloat.x > Screen.width ||
            playerPositionFloat.y < 0 ||
            playerPositionFloat.y > Screen.height ||
            !isOnScreen;

        if (offscreen)
        {
            DIE("out of screen");
        }

        if (killFromOuchie)
        {
            DIE("touched an ouchie");
            killFromOuchie = false;
        }
    }

    void cast3DPoint()
    {
        Physics.Raycast(visualCamera.ScreenPointToRay(playerRectTransform.anchoredPosition), out RaycastHit hit, Mathf.Infinity, UpdateDriver.layerMask);// does not discriminate on the layer
        worldPosition = hit.point;
    }

    void inactiveMode()
    {
        moveDirection = Vector2.zero;
        
        playerRectTransform.anchoredPosition3D = visualCamera.WorldToScreenPoint(worldPosition);

        playerPositionFloat = playerRectTransform.anchoredPosition;
        playerPosition = Vector2IntFromVector3(playerPositionFloat);
        collisionPoints.Clear();      
        //offscreenPointer();
    }

    public bool checkForColliderBetween()
    {
        Physics.Linecast(UpdateDriver.ud.GetComponent<CharacterController3D>().head.position, worldPosition, out RaycastHit hit, UpdateDriver.layerMask);
        return hit.collider != null && (
            hit.distance < Vector3.Distance(UpdateDriver.ud.GetComponent<CharacterController3D>().head.position, worldPosition)-.1f
            );
    }

    void offscreenPointer()
    {
        bool offscreen = playerPositionFloat.x < 0 ||
            playerPositionFloat.x > Screen.width ||
            playerPositionFloat.y < 0 ||
            playerPositionFloat.y > Screen.height ||
            !isOnScreen;

        pointerImage.enabled = offscreen;
        
        Vector2 screenOffset = new Vector2(Screen.width, Screen.height);
        screenOffset /= 2;
        
        Vector2 normalizedScreenPos = playerPositionFloat - screenOffset;
        normalizedScreenPos /= screenOffset;


        print(normalizedScreenPos);

        if (offscreen)
        {
            pointerPosition = playerPositionFloat;
            
            //pointerPosition.x = Mathf.Clamp(pointerPosition.x, 25, Screen.width - 25);
            //pointerPosition.y = Mathf.Clamp(pointerPosition.y, 25, Screen.height - 25);

            pointer.anchoredPosition = pointerPosition;
        }
    }

    void movePlayer()
    {
        moveDirection.x = Input.GetAxis("Horizontal_2D");
        moveDirection.y -= playerGravity * Time.deltaTime * (1-System.Convert.ToInt32(isGrounded));

        Vector3.ClampMagnitude(moveDirection,3f);

        playerPositionFloat += moveDirection * playerSpd * Time.deltaTime;

        calculateCollisions();

        if (isGrounded) 
        { 
            moveDirection.y = 0;
            lastGroundedPos = playerPositionFloat;
        }

        if (isCeiling)
        {
            moveDirection.y = 0;
            isCeiling = false;
        }

        foreach (Vector2 v in collisionPoints)
        {
            playerPositionFloat += v;
        }

        
        isInAir = !isGrounded && Vector2.Distance(playerPositionFloat,lastGroundedPos)>inAirDistanceThreshold;


        if (isGrounded)
        {
            if (Input.GetButton("Jump_2D"))
            {
                moveDirection.y = JumpHeight;
            }
        }

    }

    bool isZeroVector(Vector2 v) { return v.magnitude == 0; }
    bool vectorHasDownwardVelocity(Vector2 v) { return v.y < 0; }
    void calculateCollisions()
    {
        collisionPoints.Clear();

        //----------------------------------
        Vector2Int outOfBoundsDelta = new Vector2Int(0, 0);

        if (raw.capturePos.x < 0) { outOfBoundsDelta.x = raw.capturePos.x; }
        else if (raw.capturePos.x + raw.captureDimensions.x > Screen.width) { outOfBoundsDelta.x = (raw.capturePos.x + raw.captureDimensions.x) - Screen.width; }

        if (raw.capturePos.y < 0) { outOfBoundsDelta.y = raw.capturePos.y; }
        else if (raw.capturePos.y + raw.captureDimensions.y > Screen.height) { outOfBoundsDelta.y = (raw.capturePos.y + raw.captureDimensions.y) - Screen.height; }
        //----------------------------------

        int radiusHalf = colliderRadius / 2;
        float collisionResolutionDegree = collisionResolution / (Mathf.PI*2);
        isInCollider = false;
        isGrounded = false;

        //Find the normal of the collision points

        bool midCollision = false;
        List<int> normalIndx = new List<int>();

        for (int i = 0; i < collisionResolution+1; i++)
        {
            //define the current point of the collision check.
            Vector2 checkPoint = getCollisionCheckPoint(i, collisionResolutionDegree, outOfBoundsDelta);            

            //Convert to Vector2Int because we're working with pixels. 
            Vector2Int checkPointInt = Vector2IntFromVector3(checkPoint);

            //center the point proerply
            checkPointInt += raw.captureDimensions / 2;

            //get the color of the pixel where the collision is being checked. 
            Color detectedColor = ic.texture.GetPixel(checkPointInt.x, checkPointInt.y);

            bool isCutout = detectedColor == ColorInfo.cutoutColor_2D;
            bool isPlatform = detectedColor == ColorInfo.platformColor_2D;
            bool isDamage = detectedColor == ColorInfo.damageColor_2D;

            bool topcolliders = i < (collisionResolution / 2);
            bool _isGrounded = isPlatform && !(topcolliders);
            bool _isInCollider = isPlatform || isDamage;
            bool _killFromOuchie = isDamage;

            if (_isInCollider && !midCollision) { normalIndx.Add(i); }
            if (!_isInCollider && midCollision) { normalIndx.Add(i-1); }
            midCollision = _isInCollider;

            if (_isGrounded) { isGrounded = true; }
            if (_isInCollider && _isGrounded) { isCeiling = true; }
            if (_killFromOuchie) { killFromOuchie = true; }
            
            bool centerColliders = ((i == 0) || i == (collisionResolution / 2) || i >= collisionResolution);
            if (centerColliders && _isInCollider) { collisionPoints.Add(-checkPoint); }
        }

        //gets normals for each collision pair
        for (int i = 0; i < normalIndx.Count; i+=2)
        {
            int index1 = i;
            int index2 = i+(1-(System.Convert.ToInt16(i + 1 >= normalIndx.Count)));
            Vector2 scaledNormal = getNormalOfTwoCollisionPoints(normalIndx[index1],normalIndx[index2],collisionResolutionDegree,outOfBoundsDelta);
            collisionPoints.Add(scaledNormal);
        }

    }

    Vector3 getNormalOfTwoCollisionPoints(float index1, float index2, float resolutionDeg, Vector2 oobDelta)
    {
        float avgIndex = (index1 + index2) / 2;
        
        Vector2 entryPt = getCollisionCheckPoint(index1,   resolutionDeg, oobDelta);
        Vector2 midPt   = getCollisionCheckPoint(avgIndex, resolutionDeg, oobDelta);
        Vector2 exitPt  = getCollisionCheckPoint(index2,   resolutionDeg, oobDelta);

        Vector2 midLinear = Vector2.Lerp(entryPt,exitPt,.5f);

        return midLinear - midPt;
    }

    Vector3 getCollisionCheckPoint(float index, float resolutionDeg, Vector2 oobDelta)
    {
        //define the current point of the collision check.
        Vector2 checkPoint = new Vector2(Mathf.Cos(index / (resolutionDeg)), Mathf.Sin(index / (resolutionDeg))) * colliderRadius;
        checkPoint += oobDelta;

        //to achieve a capsule shape, we offset the top and bottom colliders.
        bool topcolliders = index < (collisionResolution / 2);
        int topColliderMod = System.Convert.ToInt16((topcolliders)); // 1 if top colliders, zero if bottom colliders
        topColliderMod *= 2;
        topColliderMod -= 1; //1 if top colliders, -1 if bottom


        bool midColliders = ((index == 0) || index == (collisionResolution / 2)||index>=collisionResolution);//if the collider points should be at the center.
        int midColliderMod = System.Convert.ToInt16(midColliders);// 1 if mid collider
        midColliderMod = 1 - midColliderMod;// 0 if mid collider.


        //offset depending on top, bottom or middle            
        checkPoint += new Vector2(0, colliderRadius) * topColliderMod * midColliderMod;

        return checkPoint;
    }

    //respawns player at checkpoint
    public void respawnAtCheckPoint()
    {
        respawnPosition = checkPointPosition;
        worldPosition = respawnPosition;
        switcher.toggleMode(true);
        onRespawnCheckpoint.Invoke();
    }

    //this method returns the raycast information of whatever collider is behind the 2d player's canvas location. 
    //useful for interactable objects in scene. 
    public RaycastHit colliderAtPlayerPosition()
    {
        Physics.Raycast(visualCamera.ScreenPointToRay(playerRectTransform.anchoredPosition), out RaycastHit hit, Mathf.Infinity, UpdateDriver.layerMask);
        return hit;
    }

    //the death method also invokes an publically accessible event. 
    //possible uses: 
    //- trigger dialogue on death
    //- play sound 
    public void DIE(string str = "Died")
    {
        worldPosition = respawnPosition;

        StartCoroutine(DeathAnimation());
        
        onDie.Invoke();
        print($"2D Character died from '{str}'.");
    }

    void switchModeto3D()
    {
        if (switcher.force2DMode)
        {
            inactiveMode();
        }
        else
        {
            switcher.toggleMode(true);
            
        }
    }

    IEnumerator DeathAnimation()
    {
        isMovementBlocked = true;
        cameraShake(.1f,.25f);
        yield return new WaitForSeconds(.25f);
        switchModeto3D();
        isMovementBlocked = false;
    }

    public void cameraShake(float duration = .1f, float magnitude = .1f)
    {
        StartCoroutine(cameraShakeEnum(duration, magnitude));
    }

    public void cameraShake(string parameters = ".1,.1")
    {
        float duration = float.Parse(parameters.Split(',')[0]);
        float magnitude = float.Parse(parameters.Split(',')[1]);

        StartCoroutine(cameraShakeEnum(duration, magnitude));
    }

    IEnumerator cameraShakeEnum(float duration = .1f, float magnitude = .1f)
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime / duration;
            visualCamera.transform.localPosition = new Vector3(Random.Range(-magnitude, magnitude), Random.Range(-magnitude, magnitude), Random.Range(-magnitude, magnitude));
            timer = Mathf.Clamp(timer, 0, 1);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        visualCamera.transform.localPosition = new Vector3(0, 0, 0);
    }

    //converts vector3 and vector 2 into vector2 int
    public static Vector3 Vector3FromVector2Int(Vector2Int v)
    {
        return new Vector3(v.x,v.y,0);
    }
    //converts vector2int into vector3
    public static Vector2Int Vector2IntFromVector3(Vector3 v)
    {
        return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    public static Vector2 roundVector(Vector2 v, float decimalPlace)
    {
        Vector2 v1 = v;
        
        v1.x *= decimalPlace;
        v1.y *= decimalPlace;

        v1.x = Mathf.Round(v1.x);
        v1.y = Mathf.Round(v1.y);

        v1.x /= decimalPlace;
        v1.y /= decimalPlace;

        return v1;
    }
}
