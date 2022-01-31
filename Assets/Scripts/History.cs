using UnityEngine;
using UnityEngine.SceneManagement;

public class History : MonoBehaviour
{
    public static History Instance;

    private const float TRANSITION_TIME = 0.5f; // Seconds
    private const float TRANSITION_THRESHOLD = 0.01f; // Seconds

    public Sprite[] Slides;

    private int currentSlide;
    private float currentX;
    private bool isFadeOut;
    private bool isFading;
    private int nextSlide;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentSlide = 0;
        spriteRenderer.sprite = Slides[currentSlide];
        spriteRenderer.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFading)
        {
            Color color;
            if (isFadeOut)
            {
                currentX -= Time.deltaTime;
                if (currentX <= TRANSITION_THRESHOLD)
                {
                    isFadeOut = false;
                    currentSlide = nextSlide;
                    spriteRenderer.sprite = Slides[currentSlide];

                    color = spriteRenderer.color;
                    color.a = 0.0f;
                    spriteRenderer.color = color;

                    currentX = 0;
                }
                else
                {
                    color = spriteRenderer.color;
                    color.a = currentX / TRANSITION_TIME;
                    spriteRenderer.color = color;
                }
            }
            else
            {
                currentX += Time.deltaTime;
                if (currentX >= TRANSITION_TIME)
                {
                    isFading = false;
                    color = spriteRenderer.color;
                    color.a = 1.0f;
                    spriteRenderer.color = color;
                }
                else
                {
                    color = spriteRenderer.color;
                    color.a = currentX / TRANSITION_TIME;
                    spriteRenderer.color = color;
                }
            }
        }
    }

    public void Next()
    {
        nextSlide = currentSlide + 1;
        if (nextSlide >= Slides.Length)
        {
            --nextSlide;
        }
        else
        {
            transitionSprite();
        }
    }

    public void Prev()
    {
        nextSlide = currentSlide - 1;
        if (nextSlide < 0)
        {
            nextSlide = 0;
        }
        else
        {
            transitionSprite();
        }
    }

    public void Skip()
    {
        SceneManager.LoadScene("FinalLevel");
    }

    private void transitionSprite()
    {
        isFadeOut = true;
        isFading = true;
        currentX = TRANSITION_TIME;
    }
}
