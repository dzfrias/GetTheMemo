using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum PrinterHitLocation
{
    Left,
    Right,
    Top,
    Bottom,
}

public struct PrinterHit
{
    private const float THRESHOLD = 0.1f;

    public PrinterHitLocation loc;
    public float time;

    public PrinterHit(PrinterHitLocation loc, float time)
    {
        this.loc = loc;
        this.time = time;
    }

    public bool Matches(PrinterHit other)
    {
        return this.loc == other.loc && TimeMatches(other.time);
    }

    public bool TimeMatches(float time)
    {
        return time <= this.time + THRESHOLD && time >= this.time - THRESHOLD;
    }
}

public class PrinterData
{
    public event Action OnShutdown;

    private List<PrinterHit> locations;

    public PrinterData(List<PrinterHit> locations)
    {
        this.locations = locations;
    }

    public bool Hit(PrinterHit hit)
    {
        if (locations.Count == 0)
        {
            return false;
        }

        // TODO: we can speed this up if necessary. Could just do a binary
        // search for an element that matches, then a linear search forwards
        // and backwards for each element that also matches
        bool found = false;
        foreach (PrinterHit expectHit in locations)
        {
            if (!expectHit.Matches(hit))
                continue;

            found = true;
        }

        return found;
    }

    public List<PrinterHit> GetLocations()
    {
        return locations;
    }

    public void Shutdown()
    {
        OnShutdown?.Invoke();
    }
}

public class Printer : MonoBehaviour, IInteractable
{
    [SerializeField]
    private CinemachineVirtualCamera cam;

    private PrinterData data;

    private void Start()
    {
        List<PrinterHit> locations = new List<PrinterHit>();
        locations.Add(new PrinterHit(PrinterHitLocation.Left, 1f));
        locations.Add(new PrinterHit(PrinterHitLocation.Top, 3f));
        locations.Add(new PrinterHit(PrinterHitLocation.Bottom, 3.6f));
        locations.Add(new PrinterHit(PrinterHitLocation.Right, 3.6f));
        data = new PrinterData(locations);
        data.OnShutdown += Shutdown;
    }

    public void Interact()
    {
        cam.gameObject.SetActive(true);
        StationUIManager.Instance.Startup(data);
    }

    private void Shutdown()
    {
        cam.gameObject.SetActive(false);
    }
}
