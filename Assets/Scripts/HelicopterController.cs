using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    [SerializeField] private float _flyingSpeed = 3;
    [SerializeField] private float _liftingSpeed = 3;
    [SerializeField] private float _rotationSpeed = 5;
    
    private readonly Vector2 _controlButtonsStartPosr = new Vector2(Screen.width / 2, Screen.height / 2 + 50);
    private const float BackOrDown = -1;
    private const float ForwardOrUp = 1;

    void FixedUpdate()
    {
        float verticalMovement = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            verticalMovement = ForwardOrUp;
        }

        if (Input.GetKey(KeyCode.E))
        {
            verticalMovement = BackOrDown;
        }

        var rotation = Input.GetAxis("Horizontal");
        var horizontalMovement = Input.GetAxis("Vertical");
        MoveHelicopter(verticalMovement, horizontalMovement);
        TurnHelicopter(rotation);
    }

    void OnGUI()
    {
        if (!GameManager.GameStarted) return;

        CheckHelicopterGuiControls();
    }

    private void TurnHelicopter(float value)
    {
        transform.Rotate(Vector3.up, value * Time.deltaTime * _rotationSpeed);
    }

    private void MoveHelicopter(float horizontalValue, float verticalValue)
    {
        transform.position = transform.position + transform.forward * verticalValue * _flyingSpeed + Vector3.up * Time.deltaTime * _liftingSpeed * horizontalValue;
    }

    private void CheckHelicopterGuiControls()
    {
        if (GUI.RepeatButton(new Rect(_controlButtonsStartPosr.x - 160, _controlButtonsStartPosr.y + 100, 100, 50), "Up(Q)"))
        {
            MoveHelicopter(ForwardOrUp, 0);
        }
        if (GUI.RepeatButton(new Rect(_controlButtonsStartPosr.x + 60, _controlButtonsStartPosr.y + 100, 100, 50), "Down(E)"))
        {
            MoveHelicopter(BackOrDown, 0);
        }

        if (GUI.RepeatButton(new Rect(_controlButtonsStartPosr.x - 50, _controlButtonsStartPosr.y + 100, 100, 50), "Forward(W)"))
        {
            MoveHelicopter(0, ForwardOrUp);
        }
        if (GUI.RepeatButton(new Rect(_controlButtonsStartPosr.x - 50, _controlButtonsStartPosr.y + 160, 100, 50), "Backward(S)"))
        {
            MoveHelicopter(0, BackOrDown);
        }
        if (GUI.RepeatButton(new Rect(_controlButtonsStartPosr.x - 160, _controlButtonsStartPosr.y + 160, 100, 50), "Left(A)"))
        {
            TurnHelicopter(BackOrDown);
        }
        if (GUI.RepeatButton(new Rect(_controlButtonsStartPosr.x + 60, _controlButtonsStartPosr.y + 160, 100, 50), "Right(D)"))
        {
            TurnHelicopter(ForwardOrUp);
        }
    }
}
