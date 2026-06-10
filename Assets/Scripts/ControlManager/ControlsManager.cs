using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager instance { get; private set; }

    public Player player { get; private set; }
    public PlayerControls inputActions;

    public Vector2 MoveInput { get; private set; }
    public InputDevice CurrentDevice { get; private set; }

    private void Awake()
    {
        instance = this;
        inputActions = new PlayerControls();
    }

    private void Start()
    {
        AssignInputEvents();
    }

    public void Init(Player owner)
    {
        player = owner;
    }

    private void AssignInputEvents()
    {
        inputActions.Player.Move.performed += ctx =>
        {
            MoveInput = ctx.ReadValue<Vector2>();
            CurrentDevice = ctx.control.device;
        };

        inputActions.Player.Move.canceled += ctx =>
        {
            MoveInput = Vector2.zero;
            CurrentDevice = ctx.control.device;
        };

        inputActions.UI.SwitchToInGame.performed += ctx =>
        {
            CurrentDevice = ctx.control.device;

            if (UI.instance.IsSkillBoardUI())
                return;

            if (UI.instance.IsInGameUI())
            {
                UI.instance.OpenSettings();
                return;
            }

            UI.instance.SwitchToIngameUI();
        };
    }

    public bool PressedAttack()
    {
        bool pressed = inputActions.Player.Attack.WasPressedThisFrame();

        if (pressed)
            CurrentDevice = inputActions.Player.Attack.activeControl?.device;

        return pressed;
    }

    public bool UsingGamepad() => CurrentDevice is Gamepad;
    public bool UsingKeyboardMouse() => CurrentDevice is Keyboard || CurrentDevice is Mouse;
    public bool UsingTouch() => CurrentDevice is Touchscreen;

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}