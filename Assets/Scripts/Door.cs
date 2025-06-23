using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class Door : MonoBehaviour
{
    public enum DoorState { Closed, PermanentlyLocked, Open }
    public enum InteractionResult { Opened, PermanentlyLocked, AlreadyOpen }

    [Header("Настройки")]
    [SerializeField] private bool startLocked = true;
    [SerializeField] private bool neverOpen = false;

    [Header("Материалы")]
    [SerializeField] private Material lockedMat;
    [SerializeField] private Material unlockedMat;

    private DoorState _currentState;
    private MeshRenderer _renderer;
    private BoxCollider _collider;
    private bool _wasInteractedThisEpisode;

    public DoorState CurrentState => _currentState;
    public bool WasInteracted => _wasInteractedThisEpisode;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<BoxCollider>();
        ResetState();
    }

    public void ResetState()
    {
        _wasInteractedThisEpisode = false;

        if (neverOpen)
        {
            _currentState = DoorState.PermanentlyLocked;
        }
        else
        {
            _currentState = startLocked ? DoorState.Closed : DoorState.Open;
        }

        UpdateComponents();
    }

    public InteractionResult Interact()
    {
        // Фиксируем попытку взаимодействия
        bool isFirstInteraction = !_wasInteractedThisEpisode;
        _wasInteractedThisEpisode = true;

        if (neverOpen)
        {
            return isFirstInteraction ?
                InteractionResult.PermanentlyLocked :
                InteractionResult.AlreadyOpen;
        }

        switch (_currentState)
        {
            case DoorState.Closed:
                _currentState = DoorState.Open;
                UpdateComponents();
                return InteractionResult.Opened;

            case DoorState.Open:
                return InteractionResult.AlreadyOpen;

            default:
                return InteractionResult.PermanentlyLocked;
        }
    }

    private void UpdateComponents()
    {
        _collider.enabled = _currentState != DoorState.Open;
        _renderer.material = _currentState == DoorState.Open ? unlockedMat : lockedMat;
        gameObject.tag = _currentState == DoorState.Open ? "OpenDoor" : "CloseDoor";
    }
}