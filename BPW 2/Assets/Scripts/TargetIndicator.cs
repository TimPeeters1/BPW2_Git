//Dit script is afkomstig van http://forum.brackeys.com/thread/offscreen-target-indicator/ 
//en niet door mijzelf geschreven. Het is wel lichtelijk uitgebreid en aangepast voor dit spel.

using UnityEngine;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour
{
    private Camera mainCamera;
    [HideInInspector] public RectTransform m_icon;
    private Image m_iconImage;
    private Canvas mainCanvas;

    private Vector3 m_cameraOffsetUp;
    private Vector3 m_cameraOffsetRight;
    private Vector3 m_cameraOffsetForward;

    public Sprite m_targetIconOnScreen;
    public Sprite m_targetIconOffScreen;

    [Space]

    [Range(0, 100)]
    public float m_edgeBuffer;

    [Range(0, 1)]
    public float m_targetIconScale;

    [Space]

    public bool ShowDebugLines;



    void Start()
    {
        mainCamera = Camera.main;

        mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        Debug.Assert((mainCanvas != null), "There needs to be a Canvas object in the scene for the OTI to display");

        InstainateTargetIcon();
    }



    void Update()
    {
        if (ShowDebugLines)
            DrawDebugLines();

        UpdateTargetIconPosition();
    }



    private void InstainateTargetIcon()
    {
        m_icon = new GameObject().AddComponent<RectTransform>();
        m_icon.transform.SetParent(mainCanvas.transform);
        m_icon.localScale = new Vector3(m_targetIconScale, m_targetIconScale, 0);

        m_icon.name = name + ": OTI icon";

        m_iconImage = m_icon.gameObject.AddComponent<Image>();
        m_iconImage.sprite = m_targetIconOffScreen;
    }



    private void UpdateTargetIconPosition()
    {
        Vector3 newPos = transform.position;

        newPos = mainCamera.WorldToViewportPoint(newPos);

        if (newPos.x > 1 || newPos.x < 0 || newPos.y > 1 || newPos.y < 0)
        {
            m_iconImage.enabled = true;

            if (m_iconImage.sprite != m_targetIconOffScreen)
            {
                m_iconImage.sprite = m_targetIconOffScreen; 
            }
        }
        else
        {
           m_iconImage.enabled = false;
        }


        if (newPos.z < 0)
        {
            newPos.x = 1f - newPos.x;
            newPos.y = 1f - newPos.y;
            newPos.z = 0;

            newPos = Vector3Maxamize(newPos);
            
        }

        newPos = mainCamera.ViewportToScreenPoint(newPos);

        newPos.x = Mathf.Clamp(newPos.x, m_edgeBuffer, Screen.width - m_edgeBuffer);
        newPos.y = Mathf.Clamp(newPos.y, m_edgeBuffer, Screen.height - m_edgeBuffer);


        m_icon.rotation = Quaternion.LookRotation(mainCamera.transform.position, transform.position - mainCamera.transform.position);
        m_icon.transform.position = newPos;
    }





    public void DrawDebugLines()
    {
        Vector3 directionFromCamera = transform.position - mainCamera.transform.position;

        Vector3 cameraForwad = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        Vector3 cameraUp = mainCamera.transform.up;

        cameraForwad *= Vector3.Dot(cameraForwad, directionFromCamera);
        cameraRight *= Vector3.Dot(cameraRight, directionFromCamera);
        cameraUp *= Vector3.Dot(cameraUp, directionFromCamera);

        Debug.DrawRay(mainCamera.transform.position, directionFromCamera, Color.magenta);

        Vector3 forwardPlaneCenter = mainCamera.transform.position + cameraForwad;

        Debug.DrawLine(mainCamera.transform.position, forwardPlaneCenter, Color.blue);
        Debug.DrawLine(forwardPlaneCenter, forwardPlaneCenter + cameraUp, Color.green);
        Debug.DrawLine(forwardPlaneCenter, forwardPlaneCenter + cameraRight, Color.red);
    }



    public Vector3 Vector3Maxamize(Vector3 vector)
    {
        Vector3 returnVector = vector;

        float max = Mathf.Max(vector.x, vector.y, 0f);
        if (max != 0f)
        {
            returnVector /= max;
        }

        return returnVector;
    }
}