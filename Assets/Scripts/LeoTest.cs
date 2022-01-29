using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeoTest : MonoBehaviour
{
    public Bar Bar;
    public Button Button;

    // Start is called before the first frame update
    void Start()
    {
        Button.GetComponent<Button>().onClick.AddListener(DoClick);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("Charge").GetComponent<TextMeshProUGUI>().text = ""
            + GameObject.Find("Slider").GetComponent<Slider>().value;

        GameObject.Find("Value").GetComponent<TextMeshProUGUI>().text = "" + Bar.CurrentCharge;
    }
    void DoClick()
    {
        int v = (int)GameObject.Find("Slider").GetComponent<Slider>().value;
        Bar.Charge(v);
    }
}
