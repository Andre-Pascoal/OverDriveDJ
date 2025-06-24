using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    public int min;
    public int max;
    public int current;
    public Image fill;

    private int lastCurrent = -1;


    // Update is called once per frame
    void Update()
    {
        // Para não estar sempre a atualizar
        if (current != lastCurrent)
        {
            getCurrentFill();
            lastCurrent = current;
        }

    }

    void getCurrentFill()
    {
        float currentOffset = current - min;
        float maxOffset = max - min;
        float fillAmount = currentOffset / maxOffset;
        fill.fillAmount = fillAmount;
    }
}
