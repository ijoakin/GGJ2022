using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeoTest : MonoBehaviour
{
    public Bar Bar;
    public Button Button;
    public TextMeshProUGUI Text;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        //Button.GetComponent<Button>().onClick.AddListener(DoClick);
    }

    // Update is called once per frame
    void Update()
    {
        Text.text = player.currentStateName;
        /*
        // Text of Slider Value
        float sliderValue = GameObject.Find("Slider").GetComponent<Slider>().value;
        GameObject.Find("SliderValue").GetComponent<TextMeshProUGUI>().text = "" + sliderValue;

        // Automatic Bar Value from Slider
        Bar.CurrentCharge = (int)sliderValue;
        GameObject.Find("BarChargeValue").GetComponent<TextMeshProUGUI>().text = "" + Bar.CurrentCharge;
        */
    }
    void DoClick()
    {
    }
}
