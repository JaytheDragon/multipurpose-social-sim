using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AgentGenerator : MonoBehaviour
{
    // UI elements to assign in the Inspector
    public InputField agentCountInput;
    public Button generateButton;
    public Text outputText;

    // List of unique USA-based names
    private List<string> availableNames = new List<string> {
        "James", "John", "Robert", "Michael", "William", "David", "Richard", "Joseph", "Thomas", "Charles",
        "Christopher", "Daniel", "Matthew", "Anthony", "Mark", "Donald", "Steven", "Paul", "Andrew", "Joshua",
        "Karen", "Nancy", "Lisa", "Betty", "Sandra", "Ashley", "Dorothy", "Kimberly", "Emily", "Donna",
        "Michelle", "Carol", "Amanda", "Melissa", "Deborah", "Stephanie", "Rebecca", "Sharon", "Laura"
    };

    void Start()
    {
        if (generateButton != null)
            generateButton.onClick.AddListener(GenerateAgents);
    }

    void GenerateAgents()
    {
        int agentCount = 30; // default value
        if (!int.TryParse(agentCountInput.text, out agentCount))
        {
            agentCount = 30;
        }
        
        List<Agent> agents = new List<Agent>();

        // Create agents with randomized motive values
        for (int i = 0; i < agentCount; i++)
        {
            Agent agent = new Agent();
            agent.Name = GetUniqueName(i);
            agent.Group = (i < agentCount / 2) ? "A" : "B";
            // Initialize four motives with random values between -1 and 1.
            agent.Motives = new Dictionary<string, float>()
            {
                { "Disability & Medical Needs", Random.Range(-1f, 1f) },
                { "65+", Random.Range(-1f, 1f) },
                { "Inaccessible", Random.Range(-1f, 1f) },
                { "Non-Native Speaker", Random.Range(-1f, 1f) }
            };
            agents.Add(agent);
        }

        // Create a list of all indices from 0 to agentCount - 1
        List<int> indices = Enumerable.Range(0, agentCount).ToList();
        System.Random rnd = new System.Random();

        // Override random agents with specific values according to spec:
        // 2 agents get -0.25 for "Disability & Medical Needs"
        var disabilityIndices = indices.OrderBy(x => rnd.Next()).Take(Mathf.Min(2, agentCount)).ToList();
        foreach (var idx in disabilityIndices)
        {
            agents[idx].Motives["Disability & Medical Needs"] = -0.25f;
        }

        // 3 agents get -0.1 for "65+"
        var ageIndices = indices.OrderBy(x => rnd.Next()).Take(Mathf.Min(3, agentCount)).ToList();
        foreach (var idx in ageIndices)
        {
            agents[idx].Motives["65+"] = -0.1f;
        }

        // 4 agents get -0.3 for "Inaccessible"
        var inaccessibleIndices = indices.OrderBy(x => rnd.Next()).Take(Mathf.Min(4, agentCount)).ToList();
        foreach (var idx in inaccessibleIndices)
        {
            agents[idx].Motives["Inaccessible"] = -0.3f;
        }

        // 3 agents get -0.1 for "Non-Native Speaker"
        var nonNativeIndices = indices.OrderBy(x => rnd.Next()).Take(Mathf.Min(3, agentCount)).ToList();
        foreach (var idx in nonNativeIndices)
        {
            agents[idx].Motives["Non-Native Speaker"] = -0.1f;
        }

        // Convert agents list to JSON and output to UI/Text
        string jsonOutput = JsonHelper.ToJson(agents.ToArray(), true);
        if (outputText != null)
        {
            outputText.text = jsonOutput;
        }
        Debug.Log(jsonOutput);
    }

    // Returns a unique name from availableNames or appends a number if needed.
    string GetUniqueName(int index)
    {
        if (index < availableNames.Count)
        {
            return availableNames[index];
        }
        else
        {
            return availableNames[index % availableNames.Count] + (index / availableNames.Count + 1).ToString();
        }
    }
}

// Serializable class to hold agent data
[System.Serializable]
public class Agent
{
    public string Name;
    public string Group;
    public Dictionary<string, float> Motives;
}

// Helper class for converting arrays to/from JSON using Unity's JsonUtility
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
