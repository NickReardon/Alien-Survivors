using UnityEngine;
using UnityEngine.UI;

public class CreditsMenu : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool scrollenabled = false;
    [SerializeField] private bool debug = false;

    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private float scrollSpeedMultiplier = 1.5f; // Multiplier for increasing scroll speed

    [Header("Credits Positioning")]
    [SerializeField] private float beginOffsetYCredits = 50f;
    [SerializeField] private float endOffsetYCredits = 50f;

    [Header("Credits Elements")]
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject scrollingCredits;

    [SerializeField] private bool isScrollingActive = false;
    [SerializeField] private float startPositionY;
    [SerializeField] private float endPositionY;
    [SerializeField] private float startScrollSpeed;
    [SerializeField] private float startScrollSpeedMultiplier;

    private void Awake()
    {
        backButton.onClick.AddListener(OnBackButtonClicked);

        if (debug)
        {
            Debug.Log("Credits Menu Loaded");
        }

        startScrollSpeed = scrollSpeed;
        startScrollSpeedMultiplier = scrollSpeedMultiplier;
    }

    private void OnEnable()
    {
        if (ValidateCredits())
        {
            CalculatePositions();
            ResetCredits();
            ActivateCredits();
        }
    }

    void Update()
    {
        if (isScrollingActive)
        {
            scrollingCredits.transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);

            // Check if the scrolling credits have reached the end position
            if (scrollingCredits.transform.localPosition.y >= endPositionY)
            {
                isScrollingActive = false;
                if (debug)
                {
                    Debug.Log("Reached the end of the credits.");
                }
            }
        }
    }

    private bool ValidateCredits()
    {
        if (scrollingCredits == null && creditsPanel == null)
        {
            Debug.LogError("Both Scrolling Credits and Credits Panel are not assigned in the inspector");
            return false;
        }

        if (scrollingCredits == null)
        {
            scrollenabled = false;
            Debug.LogWarning("Scrolling Credits not assigned in the inspector, defaulting to Credits Panel");
        }

        if (creditsPanel == null)
        {
            scrollenabled = true;
            Debug.LogWarning("Credits Panel not assigned in the inspector, defaulting to Scrolling Credits");
        }

        return true;
    }

    private void ActivateCredits()
    {
        if (scrollenabled)
        {
            if (scrollingCredits != null)
            {
                scrollingCredits.SetActive(true);
                isScrollingActive = true;
            }
            else
            {
                Debug.LogWarning("Scrolling Credits is not assigned, cannot activate scrolling.");
                isScrollingActive = false;
            }

            if (creditsPanel != null)
            {
                creditsPanel.SetActive(false);
            }
        }
        else
        {
            if (creditsPanel != null)
            {
                creditsPanel.SetActive(true);
                isScrollingActive = false;
            }
            else
            {
                Debug.LogWarning("Credits Panel is not assigned, cannot activate panel.");
                isScrollingActive = false;
            }

            if (scrollingCredits != null)
            {
                scrollingCredits.SetActive(false);
            }
        }
    }

    private void CalculatePositions()
    {
        RectTransform scrollingCreditsRect = scrollingCredits.GetComponent<RectTransform>();
        RectTransform parentRect = scrollingCredits.transform.parent.GetComponent<RectTransform>();
        GridLayoutGroup gridLayoutGroup = scrollingCredits.GetComponent<GridLayoutGroup>();

        if (scrollingCreditsRect != null && parentRect != null && gridLayoutGroup != null)
        {
            float totalHeight = 0f;
            foreach (RectTransform child in scrollingCreditsRect)
            {
                totalHeight += gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y;
            }

            float parentHeight = parentRect.rect.height;
            startPositionY = -totalHeight / 2 + (parentHeight * (beginOffsetYCredits / 100f)); // Start off-screen at the bottom
            endPositionY = totalHeight / 2 + (parentHeight * (endOffsetYCredits / 100f)); // End off-screen at the top

            scrollingCreditsRect.localPosition = new Vector3(scrollingCreditsRect.localPosition.x, startPositionY, scrollingCreditsRect.localPosition.z);

            if (debug)
            {
                Debug.Log($"Calculated start position Y: {startPositionY}, end position Y: {endPositionY}");
            }
        }
        else
        {
            Debug.LogError("RectTransform or GridLayoutGroup components are missing.");
        }
    }

    private void OnBackButtonClicked()
    {
        MenuManager.Instance.ShowMainMenu();
        ResetCredits();
    }

    private void OnMouseDown()
    {
        if (isScrollingActive)
        {
            scrollSpeed *= scrollSpeedMultiplier;
            if (debug)
            {
                Debug.Log($"Scroll speed increased to: {scrollSpeed}");
            }
        }
    }

    private void ResetCredits()
    {
        scrollSpeed = startScrollSpeed;
        scrollSpeedMultiplier = startScrollSpeedMultiplier;

        RectTransform scrollingCreditsRect = scrollingCredits.GetComponent<RectTransform>();
        if (scrollingCreditsRect != null)
        {
            scrollingCreditsRect.localPosition = new Vector3(scrollingCreditsRect.localPosition.x, startPositionY, scrollingCreditsRect.localPosition.z);
            isScrollingActive = false;
            if (debug)
            {
                Debug.Log("Credits sequence reset.");
            }
        }
        else
        {
            Debug.LogError("RectTransform component is missing.");
        }
    }
}