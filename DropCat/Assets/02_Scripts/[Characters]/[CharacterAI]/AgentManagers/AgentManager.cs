using UnityEngine;
using CharacterAI;

public class AgentManager : MonoBehaviour
{
    protected float maxSpeed = 10;
    protected float maxAccel = 3;
    protected float orientation;
    protected float rotation;
    protected Vector2 velocity;
    protected Steering steering;

    public float MaxAccel { get { return maxAccel; } set { maxAccel = value; } }

    public virtual void SetSteering(Steering steering)
    {
        return;
    }

    public virtual void Update()
    {
        Vector2 displacement = velocity * Time.deltaTime;
        orientation += rotation * Time.deltaTime;
        if (orientation < 0.0f)
        {
            orientation += 360.0f;
        }
        else if (orientation > 360.0f)
        {
            orientation -= 360.0f;
        }
        transform.Translate(displacement, Space.World);
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.forward, orientation);
    }

    public virtual void LateUpdate()
    {
        if(steering != null)
        {
            velocity += steering.GetLinear * Time.deltaTime;
            rotation += steering.GetAngular * Time.deltaTime;
            if (velocity.magnitude > maxSpeed)
            {
                velocity.Normalize();
                velocity = velocity * maxSpeed;
            }
            if (steering.GetAngular == 0.0f)
            {
                rotation = 0.0f;
            }
            if (steering.GetLinear.sqrMagnitude == 0.0f)
            {
                velocity = Vector2.zero;
            }
        }
        steering = new Steering();
    }
}
