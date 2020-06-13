using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    /*
     Euler angle is 3 floats that represent the xyz axis in degrees 
     example (90,0,0) 90 degrees rotation on the x axis 
    
     Quaternions has 4. why use them?for example": when u look at 90 degrees in X axis you can no longer rotate at Z axis properly!(this happens for all 3 axis)" 
     this is unexpected behaviour and it is unaceptable! Quaternions do NOT suffer from this!

     //unity code https://docs.unity3d.com/Manual/QuaternionAndEulerRotationsInUnity.html
     Implications for scripting
    When dealing with handling rotations in your scripts,
    you should use the Quaternion class and its functions to create and modify rotational values. 
    There are some situations where it is valid to use Euler angles, but you should bear in mind:
        - You should use the Quaternion Class functions that deal with Euler angles 
        - Retrieving, modifying, and re-applying Euler values from a rotation can cause unintentional side-effects    


         */


    public Transform camTranform;

    private float movementSpeed;
    public float movementTime;
    public float fastSpeed ;
    public float normalSpeed  ;
    public float rotationAmmount;
    public Vector3 zoomAmmount;

    public Vector3 targetPos;
    public Quaternion targetRotation;
    public Vector3 targetZoom;

    public Vector3 dragStartPos;
    public Vector3 dragCurrentPos;
    public Vector3 rotateStartPos;
    public Vector3 rotateCurrPos;

    // Start is called before the first frame update
    void Start()
    {

        //init with current 
        targetPos = transform.position;
        targetRotation = transform.rotation;
        targetZoom = camTranform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        handleMouseInput();
        handleMovementInput();
    }
    void handleMovementInput() {

        //handle norma and fast speed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
        }
        else {
            movementSpeed = normalSpeed;
        }

        //set target position
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            targetPos += transform.forward * movementSpeed ;
           
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            targetPos += transform.forward * -movementSpeed ;
       
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            targetPos += transform.right * movementSpeed ;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            targetPos += transform.right * -movementSpeed ;
        }

        //rotate
        if (Input.GetKey(KeyCode.Q)) {
            targetRotation *= Quaternion.Euler(Vector3.up * -rotationAmmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            targetRotation *= Quaternion.Euler(Vector3.up * rotationAmmount);
        }

        //zooming !!
        if (Input.GetKey(KeyCode.Z))
        {
            targetZoom += zoomAmmount;
        }
        if (Input.GetKey(KeyCode.X))
        {
            targetZoom -= zoomAmmount;
        }

        //do the transormation using lerp so its smooth 
        //limites should be implemented here so the camera stays within a certain border
        transform.position =  Vector3.Lerp(transform.position, targetPos,Time.deltaTime* movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * movementTime);
        camTranform.localPosition = Vector3.Lerp(camTranform.localPosition, targetZoom, Time.deltaTime * movementTime);

    }

    void handleMouseInput() {
        //with arrays dragging is more stable
        if (Input.GetMouseButtonDown(1)) {

            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float point;
            if (plane.Raycast(ray, out point)) {
                dragStartPos = ray.GetPoint(point);
            }
        }

        if (Input.GetMouseButton(1))
        {

            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float point;
            if (plane.Raycast(ray, out point))
            {
                dragCurrentPos = ray.GetPoint(point);

                targetPos = transform.position - dragCurrentPos + dragStartPos;
            }
        }

        //zoom with mouse wheel
        if (Input.mouseScrollDelta.y != 0) {
            targetZoom -= Input.mouseScrollDelta.y * zoomAmmount;
        }
        
        
        //rotate with right click
        if (Input.GetMouseButtonDown(2)) {
            rotateStartPos = Input.mousePosition;
        
        }
        //if player is dragging ....
        if (Input.GetMouseButton(2)) {

            rotateCurrPos = Input.mousePosition;

            Vector3 diff = rotateStartPos - rotateCurrPos;

            rotateStartPos = rotateCurrPos;

            targetRotation *= Quaternion.Euler(Vector3.up * (-diff.x / 5));
        }


    }
}
