using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;


public class CameraManager : MonoBehaviour
{

    private Coroutine _panCameraCoroutine;
    private Vector2 _startingTrackedOffset;




    public static CameraManager instance;

    [SerializeField] public CinemachineVirtualCamera[] _allVirtualCameras;

    [Header("Controls for lerping the Y Damping during player Jump/Fall")]
    [SerializeField] private float _fallPanAmount = 0.9f;
    [SerializeField] private float _fallYPanTime = 0.1f;
    public float _fallSpeedDampingChangeThreshold = -15f;



    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }


    private Coroutine _lerpYPanCoroutine;


    private CinemachineVirtualCamera _currentCamera;

    private CinemachineFramingTransposer _framingTransposer;

    private float _normYPanAmount;


    private void Awake()
    {
        if (instance == null)
        {
            
            instance = this;
        }
        //else
        //{
        //    Debug.LogWarning("Multiple instances of CameraManager found. Only one should exist.");
        //    Destroy(gameObject);
        //}


        _currentCamera = _allVirtualCameras[0];
        _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        #region alter Ansatz
        //for (int i = 0; i < _allVirtualCameras.Length; i++)
        //{
        //    //set the current active camera
        //    _currentCamera = _allVirtualCameras[i];

        //    //set the framing transposer
        //    _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();


        //}
        #endregion

        //set the YDamping amount so it is based on the inspector value
        _normYPanAmount = _framingTransposer.m_YDamping;

        // set the starting position of the tracked object offset
        _startingTrackedOffset = _framingTransposer.m_TrackedObjectOffset;


    }

    public void DisableAllCams()
    {
        for(int i = 0; i < _allVirtualCameras.Length; i++)
        {
            _allVirtualCameras[i].enabled = false;
        }
    }

    public void ResetCameraToCamera0()
    {
        _allVirtualCameras[0].enabled = true;
        _currentCamera = _allVirtualCameras[0];
    }




    #region Lerp the YDamping

    public void LerpYDamping(bool isPlayerFalling)
    {
        _lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;

        //grab the starting damping amount
        float startDampAmount = _framingTransposer.m_YDamping;
        float endDampAmount = 0f;

        //determine the end damping amount
        if (isPlayerFalling)
        {
            endDampAmount = _fallPanAmount;
            LerpedFromPlayerFalling = true;


        }
        else
        {
            endDampAmount = _normYPanAmount;

        }

        //lerp the pan amount
        float elapsedTime = 0f;
        while (elapsedTime < _fallYPanTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / _fallYPanTime));
            _framingTransposer.m_YDamping = lerpedPanAmount;

            yield return null;
        }

        IsLerpingYDamping = false;
    }






    #endregion

    #region Pan Camera
    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        _panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }

    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;

        //handle pan from trigger
        if (!panToStartingPos)
        {
            //set the direction and distances
            switch (panDirection)
            {
                case PanDirection.Up:
                    endPos = Vector2.up; break;
                case PanDirection.Down:
                    endPos = Vector2.down; break;
                case PanDirection.Left:
                    endPos = Vector2.left; break;
                case PanDirection.Right:
                    endPos = Vector2.right; break;
                default: break;

            }

            endPos *= panDistance;

            startingPos = _startingTrackedOffset;

            endPos += startingPos;
        }

        //handle the pan back to starting position

        else
        {
            startingPos = _framingTransposer.m_TrackedObjectOffset;
            endPos = _startingTrackedOffset;
        }

        //handle the actual panning of the camera
        float elapsedTime = 0f;
        while (elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;

            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime / panTime));
            _framingTransposer.m_TrackedObjectOffset = panLerp;

            yield return null;
        }
    }


    #endregion

    #region Swap Cameras 
    // Auch möglich vertikal zu machen!

    public void SwapCameras(CinemachineVirtualCamera cameraFromLeft, CinemachineVirtualCamera cameraFromRight, Vector2 triggerExitDirection)
    {
        // if the current camera is the camera on the left and our trigger exit direction is on the right
        if (_currentCamera == cameraFromLeft && triggerExitDirection.x > 0f)
        {
            //activate new camera
            cameraFromRight.enabled = true;
            //deactivate the old camera
            cameraFromLeft.enabled = false;
            //set the new camera as the current camera
            _currentCamera = cameraFromRight;
            // update composer variable
            _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        // if the current camera is the camera on the right and our trigger exit direction is on the left
        if (_currentCamera == cameraFromRight && triggerExitDirection.x < 0f)
        {
            //activate new camera
            cameraFromLeft.enabled = true;
            //deactivate the old camera
            cameraFromRight.enabled = false;
            //set the new camera as the current camera
            _currentCamera = cameraFromLeft;
            // update composer variable
            _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }


    #endregion
}
