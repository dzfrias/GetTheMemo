using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinterUI : MonoBehaviour, IStationUI<PrinterData>
{
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject bottom;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;

    private PrinterData data;
    private float timer;

    private void OnEnable()
    {
        GameInput.Instance.OnPrinterLeft += GameInput_OnPrinterLeft;
        GameInput.Instance.OnPrinterRight += GameInput_OnPrinterRight;
        GameInput.Instance.OnPrinterTop += GameInput_OnPrinterTop;
        GameInput.Instance.OnPrinterBottom += GameInput_OnPrinterBottom;
        timer = 0;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnPrinterLeft -= GameInput_OnPrinterLeft;
        GameInput.Instance.OnPrinterRight -= GameInput_OnPrinterRight;
        GameInput.Instance.OnPrinterTop -= GameInput_OnPrinterTop;
        GameInput.Instance.OnPrinterBottom -= GameInput_OnPrinterBottom;
        data.Shutdown();
    }

    private void GameInput_OnPrinterTop()
    {
        Hit(PrinterHitLocation.Top);
    }

    private void GameInput_OnPrinterBottom()
    {
        Hit(PrinterHitLocation.Bottom);
    }

    private void GameInput_OnPrinterLeft()
    {
        Hit(PrinterHitLocation.Left);
    }

    private void GameInput_OnPrinterRight()
    {
        Hit(PrinterHitLocation.Right);
    }

    private void Hit(PrinterHitLocation loc)
    {
        bool goodHit = data.Hit(new PrinterHit(loc, timer));
        if (goodHit)
        {
            Debug.Log("good hit!");
        }
        else
        {
            Debug.Log("bad hit!");
        }
    }

    public void Startup(PrinterData data)
    {
        this.data = data;
        foreach (PrinterHit hit in data.GetLocations())
        {
            StartCoroutine(SchedulePopup(hit));
        }
    }

    public ActionMap PreferredActionMap()
    {
        return ActionMap.Printer;
    }

    public void Update()
    {
        timer += Time.deltaTime;
    }

    private IEnumerator SchedulePopup(PrinterHit hit)
    {
        yield return new WaitForSeconds(hit.time - 0.5f);

        switch (hit.loc)
        {
            case PrinterHitLocation.Left:
            {
                left.SetActive(true);
                break;
            }
            case PrinterHitLocation.Right:
            {
                right.SetActive(true);
                break;
            }
            case PrinterHitLocation.Top:
            {
                top.SetActive(true);
                break;
            }
            case PrinterHitLocation.Bottom:
            {
                bottom.SetActive(true);
                break;
            }
        }
    }
}
