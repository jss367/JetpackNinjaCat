using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneratorScript : MonoBehaviour {
    public GameObject[] availableMountains;
    public List<GameObject> currentMountains;
    private float screenWidthInPoints;

	// Use this for initialization
	void Start () {
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void AddMountains(float farthestMountainEndX)
    {
        int randomIndex = Random.Range(0, availableMountains.Length);
        GameObject mt = (GameObject)Instantiate(availableMountains[randomIndex]);
        float mtWidth = mt.transform.FindChild("Floor").localScale.x;
        float mtCenter = farthestMountainEndX + mtWidth * 0.275f;
        mt.transform.position = new Vector3(mtCenter, 0, 0);
        currentMountains.Add(mt);
    }

    void GenerateMountainIfRequired()
    {
        List<GameObject> mountainsToRemove = new List<GameObject>();
        bool addMountains = true;
        float playerX = transform.position.x;
        float removeMountainX = playerX - screenWidthInPoints;
        float addMountainX = playerX + screenWidthInPoints;
        float farthestMountainEndX = 0;

        foreach (var mt in currentMountains)
        {
            float mtWidth = mt.transform.FindChild("Floor").localScale.x;
            float mtStartX = mt.transform.position.x - (mtWidth * 0.5f);
            float mtEndX = mtStartX + mtWidth;

            if (mtStartX > addMountainX)
                addMountains = false;

            if (mtEndX < removeMountainX)
                mountainsToRemove.Add(mt);

            farthestMountainEndX = Mathf.Max(farthestMountainEndX, mtEndX);
        }

        foreach (var mt in mountainsToRemove)
        {
            currentMountains.Remove(mt);
            Destroy(mt);
        }

        if (addMountains)
            AddMountains(farthestMountainEndX);
    }

    void FixedUpdate()
    {
        GenerateMountainIfRequired();
    }
}
