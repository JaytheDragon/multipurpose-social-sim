using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class Turn
{
    public string speaker;  // Example: "Eleanor"
    public string dialog;   // Example: "Hello, how are you?"
}

[Serializable]
public class Conversation
{
    public string type;       // Example: "communication"
    public string withAgent;  // Example: "Thomas"
    public string topic;      // Example: "Evacuation Plans"
    public List<Turn> turns;
    public string timestamp;  // Example: "2025-03-25 14:05:00"
}

[Serializable]
public class AgentJournal
{
    public List<Conversation> conversations = new List<Conversation>();
}

/// <summary>
/// Manages logging agent conversations to a JSON file (AgentJournal.json).
/// </summary>
public class AgentJournalManager : MonoBehaviour
{
    // Name of the main JSON file to store the global journal.
    // (This is the original journal file.)
    private string journalFileName = "AgentJournal.json";
    // Full path to the global journal file.
    private string journalFilePath;
    // In-memory copy of the global journal.
    private AgentJournal agentJournal;

    private void Awake()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        // Save the global journal in the "agentJournals" folder.
        string directoryPath = Path.Combine(Application.dataPath, "Scripts", "SimManager", "Data", "Survey", "agentJournals");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        journalFilePath = Path.Combine(directoryPath, journalFileName);
        Debug.Log("Global journal will be saved at: " + journalFilePath);
        
        // Alternatively, use persistentDataPath:
        // journalFilePath = Path.Combine(Application.persistentDataPath, journalFileName);

=======
        // Use persistentDataPath so it works in builds (this works in Editor too)
        journalFilePath = Path.Combine(Application.persistentDataPath, journalFileName);
>>>>>>> parent of d6c20e5 ([implementation] keeps another journal)
=======
        // Use persistentDataPath so it works in builds (this works in Editor too)
        journalFilePath = Path.Combine(Application.persistentDataPath, journalFileName);
>>>>>>> parent of d6c20e5 ([implementation] keeps another journal)
        LoadJournal();
    }

    /// <summary>
    /// Loads the journal from JSON if it exists; otherwise creates a new one.
    /// </summary>
    private void LoadJournal()
    {
        if (File.Exists(journalFilePath))
        {
            string json = File.ReadAllText(journalFilePath);
            agentJournal = JsonUtility.FromJson<AgentJournal>(json);
            if (agentJournal == null)
            {
                agentJournal = new AgentJournal();
            }
        }
        else
        {
            agentJournal = new AgentJournal();
        }
    }

    /// <summary>
    /// Saves the current global journal to JSON.
    /// </summary>
    private void SaveJournal()
    {
        string json = JsonUtility.ToJson(agentJournal, true);
        File.WriteAllText(journalFilePath, json);
    }

    /// <summary>
    /// Adds a conversation record to the global journal and saves it.
    /// </summary>
    /// <param name="type">Type of conversation (e.g., communication).</param>
    /// <param name="withAgent">The other agent's name.</param>
    /// <param name="topic">Topic of the conversation.</param>
    /// <param name="turns">List of dialog turns.</param>
    /// <param name="timeStamp">Time of the conversation.</param>
    public void AddConversation(string type, string withAgent, string topic, List<Turn> turns, DateTime timeStamp)
    {
        Conversation conversation = new Conversation
        {
            type = type,
            withAgent = withAgent,
            topic = topic,
            turns = turns,
            timestamp = timeStamp.ToString("yyyy-MM-dd HH:mm:ss")
        };

        agentJournal.conversations.Add(conversation);
        SaveJournal();
    }

    /// <summary>
<<<<<<< HEAD
<<<<<<< HEAD
    /// Debug utility: prints the entire global journal to the Console.
=======
    /// Debug utility: prints the journal to the Console.
>>>>>>> parent of d6c20e5 ([implementation] keeps another journal)
=======
    /// Debug utility: prints the journal to the Console.
>>>>>>> parent of d6c20e5 ([implementation] keeps another journal)
    /// </summary>
    public void PrintJournalToConsole()
    {
        foreach (var convo in agentJournal.conversations)
        {
            Debug.Log($"Type: {convo.type}, With: {convo.withAgent}, Topic: {convo.topic}, Timestamp: {convo.timestamp}");
            if (convo.turns != null)
            {
                foreach (var turn in convo.turns)
                {
                    Debug.Log($"  {turn.speaker}: {turn.dialog}");
                }
            }
        }
    }

    // ============================================================
    // NEW: Per-Agent Journal Saving Functionality
    // ============================================================

    /// <summary>
    /// Saves the given agent's personal journal to a unique JSON file.
    /// Each agent's journal is stored in:
    /// Assets/Scripts/SimManager/Data/Survey/agentJournals/[AgentName]/[AgentName]_Journal.json
    /// Journals reset every run.
    /// </summary>
    /// <param name="agentName">The agent's name.</param>
    /// <param name="journal">The agent's personal AgentJournal object.</param>
    public void SavePersonalJournalForAgent(string agentName, AgentJournal journal)
    {
        // Define the base folder for personal agent journals.
        string baseFolder = Path.Combine(Application.dataPath, "Scripts", "SimManager", "Data", "Survey", "agentJournals");
        if (!Directory.Exists(baseFolder))
        {
            Directory.CreateDirectory(baseFolder);
        }

        // Create a subfolder for the agent.
        string agentFolder = Path.Combine(baseFolder, agentName);
        if (!Directory.Exists(agentFolder))
        {
            Directory.CreateDirectory(agentFolder);
        }

        // Build the file path: e.g., Assets/Scripts/SimManager/Data/Survey/agentJournals/Eleanor/Eleanor_Journal.json
        string filePath = Path.Combine(agentFolder, $"{agentName}_Journal.json");

        try
        {
            string json = JsonUtility.ToJson(journal, true);
            File.WriteAllText(filePath, json);
            Debug.Log($"Journal for {agentName} saved at: {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save journal for {agentName}: {e.Message}");
        }
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}
