using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPositionUI : MonoBehaviour
{
    [SerializeField] private GameObject player = null;
    private MeshFilter enemyMeshFilter = null;
    [SerializeField] private GameObject enemyWarningUI = null;
    private GameObject spawnedWarningUI = null;
    private RectTransform canvasTransform = null;
    private RectTransform uIRectTransform = null;
    private float rectPositionY = -100f;
    private Camera cameraObj;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PLAYER_Character");
        enemyMeshFilter = this.GetComponent<MeshFilter>();
        cameraObj = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Start()
    {
        canvasTransform = GameObject.FindGameObjectWithTag("[UI]_EnemyWarning").GetComponent<RectTransform>();
        spawnedWarningUI =
            Instantiate(enemyWarningUI, new Vector2(0, rectPositionY), Quaternion.identity);
        spawnedWarningUI.SetActive(false);
        spawnedWarningUI.transform.SetParent(GameObject.FindGameObjectWithTag("[UI]_EnemyWarning")
            .transform, false);
        uIRectTransform = spawnedWarningUI.GetComponent<RectTransform>();
        uIRectTransform.LeanSetLocalPosY(rectPositionY);
    }

    private void Update()
    {
        if(IsVisibleToCamera(cameraObj))
        {
            spawnedWarningUI.SetActive(false);
        }
        else
        {
            Vector2 canvasPos = ConvertWorldPosToCanvas();
            spawnedWarningUI.SetActive(true);
            if (PlayerIsBelowTheEnemy())
            {
                uIRectTransform.anchoredPosition = new Vector2(canvasPos.x, rectPositionY);
                uIRectTransform.anchorMax = new Vector2(0.5f, 1f);
                uIRectTransform.anchorMin = new Vector2(0.5f, 1f);
                uIRectTransform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                uIRectTransform.anchoredPosition = new Vector2(canvasPos.x, -rectPositionY);
                uIRectTransform.anchorMax = new Vector2(0.5f, 0f);
                uIRectTransform.anchorMin = new Vector2(0.5f, 0f);
                uIRectTransform.rotation = Quaternion.Euler(0, 0, 180);
            }
        }
    }

    private bool PlayerIsBelowTheEnemy()
    {
        return player.transform.position.y < this.transform.position.y;
    }

    private Vector2 ConvertWorldPosToCanvas()
    {
        Vector2 viewportPosition = cameraObj.WorldToViewportPoint(this.transform.position);
        Vector2 worldObject_ScreenPosition = new Vector2(
        ((viewportPosition.x * canvasTransform.sizeDelta.x) - (canvasTransform.sizeDelta.x * 0.5f)),
        ((viewportPosition.y * canvasTransform.sizeDelta.y) - (canvasTransform.sizeDelta.y * 0.5f)));
        return worldObject_ScreenPosition;
    }

    private bool IsVisibleToCamera(Camera cam)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

        foreach (Plane plane in planes)
        {
            foreach (Vector2 vertice in enemyMeshFilter.mesh.vertices)
            {
                if (plane.GetDistanceToPoint(transform.TransformPoint(vertice)) < 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void OnDestroy()
    {
        if(spawnedWarningUI != null)
            Destroy(spawnedWarningUI.gameObject);
    }
}
