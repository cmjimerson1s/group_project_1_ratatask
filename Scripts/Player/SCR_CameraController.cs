using UnityEngine;
using UnityEngine.Splines;

public class SCR_CameraController : MonoBehaviour
{
    [Header("References")]
    public float cameraRotationSpeed = 3;
    public float cameraPositionSpeed;
    SCR_Player pS;
    public GameObject camObj;

    bool fixedCam = false;
    [SerializeField] float mainCamResetSpeed;
    [HideInInspector] public Vector3 camDefaultStartPos;
    [HideInInspector] public Quaternion camDefaultStartRot;

    private void Start()
    {
        pS = SCR_SceneManager.instance.pS;
    }

    public void SaveCamValues()
    {
        camDefaultStartPos = camObj.transform.localPosition;
        camDefaultStartRot = camObj.transform.localRotation;
        //Debug.Log($"{camDefaultStartPos} | {camDefaultStartRot.eulerAngles} Start");

        camObj.transform.localPosition = Vector3.zero;
        camObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public void UpdateCamera()
    {
        if (!fixedCam)
        {
            //Debug.Log($"{camDefaultStartPos} | {camDefaultStartRot.eulerAngles}");
            camObj.transform.localPosition = Vector3.MoveTowards(
                camObj.transform.localPosition, camDefaultStartPos, mainCamResetSpeed * Time.deltaTime);
            camObj.transform.localRotation = Quaternion.RotateTowards(
                camObj.transform.localRotation,camDefaultStartRot, mainCamResetSpeed * Time.deltaTime);

            bool posIsThere = (Mathf.Approximately(camObj.transform.localPosition.x, camDefaultStartPos.x) && Mathf.Approximately(camObj.transform.localPosition.y, camDefaultStartPos.y) && Mathf.Approximately(camObj.transform.localPosition.z, camDefaultStartPos.z));
            bool rotIsThere = (Mathf.Approximately(camObj.transform.localRotation.eulerAngles.x, camDefaultStartRot.eulerAngles.x) && Mathf.Approximately(camObj.transform.localRotation.eulerAngles.y, camDefaultStartRot.eulerAngles.y) && Mathf.Approximately(camObj.transform.localRotation.eulerAngles.z, camDefaultStartRot.eulerAngles.z));
            
            if (posIsThere &&  rotIsThere)
            {
                fixedCam = true;
                camObj.transform.localPosition = camDefaultStartPos;
                camObj.transform.localRotation = camDefaultStartRot;
            }
        }


        float posSpeed = cameraPositionSpeed;// * (pS.movementScript.defaultPlayerSpeed / 10);
        if (pS.playerPath == null || pS.movementScript == null) return;
        float playerProgress = pS.movementScript.progress;
        Vector3 position = pS.playerPath.EvaluatePosition(playerProgress);
        Vector3 tangent = pS.playerPath.EvaluateTangent(playerProgress);
        Quaternion targetRotation = Quaternion.LookRotation(tangent);
        Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * cameraRotationSpeed);


        transform.position = new Vector3(
            Mathf.Lerp(transform.position.x, position.x, Time.deltaTime * posSpeed),
            Mathf.Lerp(transform.position.y, pS.transform.position.y, Time.deltaTime * posSpeed),
            Mathf.Lerp(transform.position.z, position.z, Time.deltaTime * posSpeed));

        transform.rotation = smoothedRotation;

    }
}
