using UnityEngine;

public class SkyCameraTracker : MonoBehaviour
{
    private Transform target;
    public Transform Target { get { return target; } set { target = value; } }
    [SerializeField] private float SkyObjHeightThreshold = 32f;
    private float initX;

    private void Awake()
    {
        target = GameObject.Find("[Virtual Camera]").transform;
        initX = this.transform.position.x;
    }

    private void Update()
    {
        SkyTracksCamera();
    }

    private bool PlayerIsNearGround()
    {
        return this.transform.position.y <= SkyObjHeightThreshold;
    }

    private bool PlayerIsAtUpperLevel()
    {
        return SkyObjHeightThreshold < this.transform.position.y;
    }

    private void SkyTracksCamera()
        //Based on the player's position
    {
        this.transform.position = new Vector2(initX, target.transform.position.y);
        if (PlayerIsNearGround())
        {
            this.transform.position = new Vector2(initX, SkyObjHeightThreshold);
        }
    }
}
