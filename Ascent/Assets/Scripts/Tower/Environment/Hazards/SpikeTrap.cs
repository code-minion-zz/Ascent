using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpikeTrap : MonoBehaviour 
{
    enum ESpikeTrapState
    {
        Idle,
        Activated,
        Peeked,
        Rearming
    }

    public float spikeDamage;
    public float rearmSpeed = 1.5f;
    public float spikeLength;
    public int stubsWide;
    public int stubsLong;
    public float spikeHeightMax = 1.0f;
    public float spikeRiseSpeed = 10.0f;

    private List<Spike> listSpikes = new List<Spike>();
    private SpikeTrapPlate plate;
    private ESpikeTrapState state = ESpikeTrapState.Idle;

    private float waitMax = 1.0f;
    private float waited = 0.0f;

  

    void Start()
    {
        Initialise();
    }

    void Initialise()
    {
        plate = transform.FindChild("Plate").GetComponent<SpikeTrapPlate>();
        Transform spikeParent = transform.FindChild("Spikes");

        foreach (Transform t in plate.GetComponentsInChildren<Transform>())
        {
            if (t.name == "Stub")
            {
                GameObject newSpike = GameObject.Instantiate(Resources.Load("Prefabs/Hazards/Spike")) as GameObject;
                Spike spike = newSpike.GetComponent<Spike>();
                newSpike.transform.parent = t;
                newSpike.transform.localPosition = new Vector3(0.0f, -9.5f, 0.0f);
                spike.Initialise(spikeDamage);
                listSpikes.Add(spike);
            }
        }
    }

    void Update()
    {
        switch (state)
        {
            case ESpikeTrapState.Idle:
                {
                    // Check if trap has been stepped on and activated it if it has
                    if(plate.IsStepped())
                    {
                        state = ESpikeTrapState.Activated;
                        Debug.Log(state);

                        foreach (Spike spike in listSpikes)
                        {
                            spike.startTime = Time.time;
                            spike.originalPos = spike.transform.position;
                            spike.distance = Vector3.Distance(spike.originalPos, new Vector3(spike.originalPos.x, spikeHeightMax, spike.originalPos.z));
                        }
                    }
                }
                break;

            case ESpikeTrapState.Activated:
                {
                    // Make the trap rise quickly to it's peek then retract/rearm it
                    foreach (Spike spike in listSpikes)
                    {
                        float distCovered = (Time.time - spike.startTime) * spikeRiseSpeed;
                        float fracJourney = distCovered / spike.distance;
                        spike.transform.position = Vector3.Lerp(spike.originalPos, new Vector3(spike.originalPos.x, spikeHeightMax, spike.originalPos.z), fracJourney);
                    }

                    if (listSpikes[0].transform.position.y == spikeHeightMax)
                    {
                        state = ESpikeTrapState.Peeked;
                        Debug.Log(state);

                        foreach (Spike spike in listSpikes)
                        {
                            spike.startTime = Time.time;
                            spike.originalPos = spike.transform.position;
                            spike.distance = Vector3.Distance(spike.originalPos, new Vector3(spike.originalPos.x, spikeHeightMax, spike.originalPos.z));
                        }
                    }
                }
                break;
            case ESpikeTrapState.Peeked:
                {
                    waited += Time.deltaTime;
                    if(waited > waitMax)
                    {
                        waited = 0.0f;

                        state = ESpikeTrapState.Rearming;
                    }
                }
                break;
            case ESpikeTrapState.Rearming:
                {
                    // Retract the trap
                    foreach (Spike spike in listSpikes)
                    {
                        float distCovered = (Time.time - spike.startTime) * rearmSpeed;
                        float fracJourney = distCovered / spike.distance;
                        spike.transform.position = Vector3.Lerp(spike.originalPos, new Vector3(spike.originalPos.x, -spikeHeightMax, spike.originalPos.z), fracJourney);
                    }

                    if (listSpikes[0].transform.position.y == -spikeHeightMax)
                    {
                        state = ESpikeTrapState.Idle;
                        Debug.Log(state);

                        foreach (Spike spike in listSpikes)
                        {
                            spike.originalPos = spike.transform.position;
                        }
                    }
                }
                break;
            default:
                {
                    Debug.LogError("Invalid case");
                }
                break;
        }
    }
}
