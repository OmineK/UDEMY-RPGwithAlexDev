using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;

    [Header("Lost currency")]
    [SerializeField] GameObject lostCurrencyPref;
    [SerializeField] float lostCurrencyX;
    [SerializeField] float lostCurrencyY;
    public int lostCurrencyAmount;
    [Space]

    [SerializeField] Checkpoint[] checkpoints;
    Transform player;

    string closestCheckpointID;

    void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        checkpoints = FindObjectsOfType<Checkpoint>();
    }

    void Start()
    {
        player = PlayerManager.instance.player.transform;
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data) => StartCoroutine(LoadWithDelay(_data));

    IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(0.1f);

        LoadLostCurrency(_data);
        LoadCheckpoints(_data);
        LoadClosestCheckpoint(_data);
    }

    void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;

        if (lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPref, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyControler>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }

    private void LoadCheckpoints(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if ((checkpoint.checkpointID == pair.Key) && (pair.Value == true))
                    checkpoint.ActivateCheckpoint();
            }
        }
    }

    void LoadClosestCheckpoint(GameData _data)
    {
        if (_data.closestCheckpointID == null) { return; }

        closestCheckpointID = _data.closestCheckpointID;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (closestCheckpointID == checkpoint.checkpointID)
                player.position = checkpoint.transform.position;
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = player.position.y;

        if (FindClosestCheckpoint() != null)
            _data.closestCheckpointID = FindClosestCheckpoint().checkpointID;

        _data.checkpoints.Clear();

        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.checkpointID, checkpoint.activationStatus);
        }
    }

    private Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach (var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(player.position, checkpoint.transform.position);

            if ((distanceToCheckpoint < closestDistance) && (checkpoint.activationStatus == true))
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }

    public void PauseGame(bool _pause)
    {
        if (_pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
