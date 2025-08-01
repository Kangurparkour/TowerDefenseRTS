using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{

    public float cameraSpeed;
    public float zoomSpeed;
    public float groundHeight;
    public Vector2 cameraHightMinMax;
    public Vector2 cameraRotateMinMax;

    [Range(0, 1)] public float zoomLerp;
    [Range(0, 0.2f)] public float cursorThreshold;

    RectTransform selectionBox;
    new Camera camera;
    
    Vector2 mousePosition;
    Vector2 mousePosScreen;
    Vector2 keyboardInput;
    Vector2 mouseScroll;

    bool isInScreen;
    Rect selectionRect, boxRect;


    private void Awake()
    {
        selectionBox = GetComponentInChildren<Image>(true).transform as RectTransform;
        camera = GetComponentInChildren<Camera>();
        selectionBox.gameObject.SetActive(false);

    }


    private void Update()
    {
        UpdateMovement();
        UpdateClicks();
        UpdateZoom();
    }



    void UpdateMovement()
    {
        keyboardInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        mousePosition = Input.mousePosition;
        mousePosScreen = camera.ScreenToViewportPoint(mousePosition);
        isInScreen = mousePosScreen.x >= 0 && mousePosScreen.x <= 1 
            && mousePosScreen.y >= 0 && mousePosScreen.y <= 1;

        Vector2 movementDirection = keyboardInput; 
        
        if(isInScreen)
        {
            if (mousePosScreen.x < cursorThreshold) movementDirection.x -= 1 - mousePosScreen.x/cursorThreshold;
            if (mousePosScreen.x > 1-cursorThreshold) movementDirection.x += 1- (1-mousePosScreen.x) / (cursorThreshold);
            if (mousePosScreen.y < cursorThreshold) movementDirection.y -= 1 - mousePosScreen.y / cursorThreshold;
            if (mousePosScreen.y > 1 - cursorThreshold) movementDirection.y += 1 - (1 - mousePosScreen.y) / (cursorThreshold);
        }

        var deltaPosition = new Vector3(movementDirection.x, 0, movementDirection.y);
        deltaPosition *= cameraSpeed * Time.deltaTime;
        transform.position += deltaPosition;

    }
    void UpdateZoom()
    {
        mouseScroll = Input.mouseScrollDelta;
        float zoomDelta = mouseScroll.y * (-zoomSpeed) * Time.deltaTime;
        zoomLerp = Mathf.Clamp01(zoomLerp + zoomDelta);

        var position = transform.localPosition;
        position.y = Mathf.Lerp(cameraHightMinMax.x, cameraHightMinMax.y, zoomLerp); //+ groundHeight;
        transform.position = position;
        
        var rotation = transform.localEulerAngles;
        rotation.x = Mathf.Lerp(cameraRotateMinMax.x, cameraRotateMinMax.y, zoomLerp);
        transform.localEulerAngles = rotation;
        
    }
    void UpdateClicks()
    {

        if (Input.GetMouseButtonDown(0))
        {
            selectionBox.gameObject.SetActive(true);
            selectionRect.position = mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            selectionBox.gameObject.SetActive(false);
        }
        if(Input.GetMouseButton(0))
        {
            selectionRect.size = mousePosition - selectionRect.position;
            boxRect = AbsRect(selectionRect);
            selectionBox.anchoredPosition = boxRect.position;
            selectionBox.sizeDelta = boxRect.size;
        }

    }

    Rect AbsRect(Rect rect)
    {

        if(rect.width < 0)
        {
            rect.x += rect.width;
            rect.width *= -1;
        }
        if(rect.height <0)
        {
            rect.y += rect.height;
            rect.height *= -1;
        }

        return rect;
    }



}
