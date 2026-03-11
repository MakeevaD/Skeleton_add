using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    private Camera mainCamera;

    public BoneSelectable currentSelection { get; private set; }

    [SerializeField] private Color selectionColor = Color.red;
    [SerializeField] private Color hoverColor = Color.yellow;

    private BoneSelectable currentHover;

    private void Start()
    {
        mainCamera = Camera.main;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelStart += HandleLevelStart;
            GameManager.Instance.OnGameEnd += DeselectCurrent;
        }
    }

    private void Update()
    {
        UpdateHover();

        if (!Input.GetMouseButtonDown(0))
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        HandleClick();
    }

    private void UpdateHover()
    {
        // наведение не работаем, если курсор над UI или курсор выключен
        if ((EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) || Cursor.visible == false)
        {
            ClearHover();
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            var hover = hit.collider.GetComponent<BoneSelectable>();
            if (hover == null)
            {
                ClearHover();
                return;
            }

            // если наведены на ту же самую – ничего не меняем
            if (hover == currentHover)
                return;

            // сброс старого hover
            ClearHover();

            // если это не выбранная кость – подсвечиваем hover‑цветом
            if (hover != currentSelection)
            {
                currentHover = hover;
                currentHover.Select(hoverColor);
            }
        }
        else
        {
            ClearHover();
        }
    }

    private void ClearHover()
    {
        if (currentHover == null)
            return;

        // если кость также выбрана – вернуть цвет выбора
        if (currentHover == currentSelection)
            currentHover.Select(selectionColor);
        else
            currentHover.Deselect();

        currentHover = null;
    }

    private void HandleClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 1000f))
            return;

        var selectable = hit.collider.GetComponent<BoneSelectable>();
        if (selectable == null)
            return;

        // если кликаем по уже выбранной – ничего не делаем
        if (selectable == currentSelection)
            return;

        DeselectCurrent();

        currentSelection = selectable;
        currentSelection.Select(selectionColor);

        // если на эту же кость был hover – обновим ссылку
        if (currentHover == currentSelection)
            currentHover = currentSelection;
    }

    private void DeselectCurrent()
    {
        if (currentSelection != null)
        {
            currentSelection.Deselect();
            currentSelection = null;
        }

        // при смене уровня/конце игры сбрасываем hover
        ClearHover();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelStart -= HandleLevelStart;
            GameManager.Instance.OnGameEnd -= DeselectCurrent;
        }
    }

    private void HandleLevelStart(BoneData _, int __)
    {
        DeselectCurrent();
    }
}
