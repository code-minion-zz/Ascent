using UnityEngine;
using System.Collections;

public class SpikeTrap : MonoBehaviour 
{
    struct TSpike
    {
        public GameObject go;
        public Spike script;
        public Vector3 originalPos;
        public float startTime;
        public float distance;
    }

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

    private int spikeCount = 25;
    private TSpike[] spikes;
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

        spikes = new TSpike[spikeCount];

        Transform spikeParent = transform.FindChild("Spikes");

        Vector3 startPos = new Vector3(-0.6f, -0.2f, 0.6f);
        float offsetX = 0.3f;
        float offsetZ = -0.3f;
        //Vector3 offset = new Vector3(0.2f, 0.0f, 0.2f);

        for (int i = 0; i < 5; ++i)
        {
            for (int j = 0; j < 5; ++j)
            {
                int current = (i * 5) + j;
                GameObject newSpike = GameObject.Instantiate(Resources.Load("Prefabs/Spike")) as GameObject;
                spikes[current].go = newSpike;
                spikes[current].script = newSpike.GetComponent<Spike>();
                spikes[current].script.Initialise(spikeDamage);

                Vector3 offset = new Vector3(offsetX * i, -1.0f, offsetZ * j);

                newSpike.transform.position = transform.position +  startPos + offset;

                newSpike.transform.parent = spikeParent;
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

                        for (int i = 0; i < spikeCount; ++i)
                        {
                            spikes[i].startTime = Time.time;
                            spikes[i].originalPos = spikes[i].go.transform.position;
                            spikes[i].distance = Vector3.Distance(spikes[i].originalPos, new Vector3(spikes[i].originalPos.x, spikeHeightMax, spikes[i].originalPos.z));
                        }
                    }
                }
                break;
            case ESpikeTrapState.Activated:
                {
                    // Make the trap rise quickly to it's peek then retract/rearm it
                    for (int i = 0; i < spikeCount; ++i )
                    {
                        float distCovered = (Time.time - spikes[i].startTime) * spikeRiseSpeed;
                        float fracJourney = distCovered / spikes[i].distance;
                        spikes[i].go.transform.position = Vector3.Lerp(spikes[i].originalPos, new Vector3(spikes[i].originalPos.x, spikeHeightMax, spikes[i].originalPos.z), fracJourney);
                    }
                    if (spikes[0].go.transform.position.y == spikeHeightMax)
                    {
                        state = ESpikeTrapState.Peeked;
                        Debug.Log(state);

                        for (int i = 0; i < spikeCount; ++i)
                        {
                            spikes[i].startTime = Time.time;
                            spikes[i].originalPos = spikes[i].go.transform.position;
                            spikes[i].distance = Vector3.Distance(spikes[i].originalPos, new Vector3(spikes[i].originalPos.x, spikeHeightMax, spikes[i].originalPos.z));
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
                    for (int i = 0; i < spikeCount; ++i)
                    {
                        float distCovered = (Time.time - spikes[i].startTime) * rearmSpeed;
                        float fracJourney = distCovered / spikes[i].distance;
                        spikes[i].go.transform.position = Vector3.Lerp(spikes[i].originalPos, new Vector3(spikes[i].originalPos.x, -spikeHeightMax, spikes[i].originalPos.z), fracJourney);
                    }
                    if (spikes[0].go.transform.position.y == -spikeHeightMax)
                    {
                        state = ESpikeTrapState.Idle;
                        Debug.Log(state);

                        for (int i = 0; i < spikeCount; ++i)
                        {
                            spikes[i].originalPos = spikes[i].go.transform.position;
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
