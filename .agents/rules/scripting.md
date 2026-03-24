---
trigger: always_on
---

# Coding Guidelines — Unity 6 Scripting & Patterns

## C# Naming Conventions

| Construct | Convention | Example |
|---|---|---|
| Class / Struct | PascalCase | `PlayerController` |
| Method | PascalCase | `TakeDamage()` |
| Public field / Property | PascalCase | `MaxHealth` |
| Private field | `_camelCase` | `_currentHealth` |
| Constant | SCREAMING_SNAKE | `MAX_SPAWN_COUNT` |
| Interface | `I` prefix | `IDamageable` |
| ScriptableObject | `SO` suffix | `WeaponDataSO` |
| Event | `On` prefix | `OnPlayerDied` |

---

## MonoBehaviour Rules
- One MonoBehaviour per GameObject responsibility.
- Never use `Find`, `FindObjectOfType`, or `GetComponent` in `Update`. Cache in `Awake`.
- Keep `Update`, `FixedUpdate`, and `LateUpdate` thin — delegate to domain classes.
- Prefer `[SerializeField] private` over public fields for Inspector exposure.
- Use `RequireComponent` to enforce dependencies rather than null-checking at runtime.

```csharp
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

    private Rigidbody2D _rb;

    private void Awake() => _rb = GetComponent<Rigidbody2D>();
    private void FixedUpdate() => Move(ReadInput());

    private Vector2 ReadInput() =>
        new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

    private void Move(Vector2 direction) =>
        _rb.linearVelocity = direction.normalized * _moveSpeed;
}
```

---

## ScriptableObjects
Use SOs as **data containers and shared state** — not as runtime logic holders.

- Define all tunable game data (stats, configs, item definitions) as SOs.
- Always add `[CreateAssetMenu]` for designer-facing SOs.
- Never put scene-specific references inside an SO — breaks prefab portability.

```csharp
[CreateAssetMenu(menuName = "Game/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    public float MaxHealth;
    public float MoveSpeed;
    public float AttackDamage;
}
```

---

## Event Architecture
Use a **decoupled SO-based event bus** — not direct method calls between unrelated systems.

- Listeners register/deregister in `OnEnable` / `OnDisable`.
- Prefer events over singletons for cross-system communication.

```csharp
[CreateAssetMenu(menuName = "Events/Game Event")]
public class GameEventSO : ScriptableObject
{
    private readonly List<IGameEventListener> _listeners = new();

    public void Raise()
    {
        // Iterate in reverse — listeners may deregister during Raise()
        for (int i = _listeners.Count - 1; i >= 0; i--)
            _listeners[i].OnEventRaised();
    }

    public void Register(IGameEventListener listener) => _listeners.Add(listener);
    public void Deregister(IGameEventListener listener) => _listeners.Remove(listener);
}
```

---

## Input System
- Use the **New Input System** exclusively — never `Input.GetKey` / `Input.GetAxis`.
- Define one `.inputactions` asset per player/context.
- Bind actions in `OnEnable` / `OnDisable`.
- **Explicitly Banned**: Legacy `OnMouseDown`, `OnMouseUp`, `OnMouseEnter`, etc., are strictly prohibited. Use `IPointerDownHandler` and the Unity Event System (with a `Physics2DRaycaster` on the Camera) for interaction.

---

## Async
- Prefer `async/await` with Unity 6's `Awaitable` API over Coroutines for async flows.
- Use Coroutines only for simple timed sequences where `Awaitable` is overkill.
- Never fire-and-forget without handling exceptions.

---

## Dependency Injection
- Avoid singletons and static state. Inject dependencies via `[SerializeField]` or a lightweight DI container.
- If a service locator is needed, scope it to a scene — never globally.