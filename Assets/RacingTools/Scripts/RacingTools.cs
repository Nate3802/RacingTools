using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class RacingTools : MonoBehaviour
{
    public List<Object> trackPrefabs;

    List<GameObject> _trackPieces;
    List<Vector3> _trackLocations;

    bool _obstructed;


    // Start is called before the first frame update
    void Start()
    {
        _trackPieces = new List<GameObject>();
        _trackLocations = new List<Vector3>();

        foreach(Transform t in transform.GetChild(0))
        {
            _trackPieces.Add(t.gameObject);
            _trackLocations.Add(t.position + t.right);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPiece(int pieceNum)
    {
        if (!_obstructed)
        {
            Vector3 newPos = transform.position;
            Quaternion newRot = Quaternion.identity;
            if (_trackPieces.Count > 0)
            {
                newPos = _trackPieces[_trackPieces.Count - 1].transform.GetChild(3).position;
                newRot = _trackPieces[_trackPieces.Count - 1].transform.GetChild(3).rotation;
            }
            _trackPieces.Add((GameObject) PrefabUtility.InstantiatePrefab(trackPrefabs[pieceNum]));
            _trackPieces[_trackPieces.Count - 1].transform.position = newPos;
            _trackPieces[_trackPieces.Count - 1].transform.rotation = newRot;
            _trackPieces[_trackPieces.Count - 1].transform.parent = transform.GetChild(0);

            _trackLocations.Add(RoundVector(_trackPieces[_trackPieces.Count - 1].transform.position + _trackPieces[_trackPieces.Count - 1].transform.right));

            _obstructed = TestObstruction();
        }
        else
        {
            Debug.LogWarning("New piece blocked by existing pieces");
        }
    }

    public void DeleteLastPiece()
    {
        if (_trackPieces.Count > 0)
        {
            GameObject lastPiece = _trackPieces[_trackPieces.Count - 1];
            DestroyImmediate(lastPiece);
            _trackPieces.Remove(_trackPieces[_trackPieces.Count - 1]);
            _trackLocations.RemoveAt(_trackLocations.Count - 1);
        }
        else
        {
            Debug.LogWarning("No track pieces to delete!");
        }

        if (_trackPieces.Count > 0)
        {
            _obstructed = TestObstruction();
        }
    }

    public bool TestObstruction()
    {
        if (_trackPieces.Count > 0)
        {
            Vector3 testLocation = RoundVector(_trackPieces[_trackPieces.Count - 1].transform.GetChild(3).position +
                _trackPieces[_trackPieces.Count - 1].transform.GetChild(3).right);

            if (_trackLocations.Contains(testLocation))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    Vector3 RoundVector(Vector3 original)
    {
        Vector3 newVector = new Vector3(Mathf.RoundToInt(original.x), Mathf.RoundToInt(original.y), Mathf.RoundToInt(original.z));
        return newVector;
    }

    public bool CheckCircuit()
    {
        if (_trackPieces.Count > 0)
        {
            if (RoundVector(_trackPieces[_trackPieces.Count - 1].transform.GetChild(3).position) == transform.position)
            {
                return true;
            }
        }

        return false;
    }

    public string ConvertName(string original)
    {
        string newName = "";

        for(var i = 0; i<original.Length; i++)
        {
            if(i == 0)
            {
                if (char.IsLetter(original[i]))
                {
                    newName += original[i].ToString().ToUpper();
                }
            }
            else if (char.IsUpper(original[i]) && newName[newName.Length -1] != ' ')
            {
                newName += (" " + original[i]);
            }
            else if (original[i] == '_')
            {
                newName += " ";
            }
            else if(newName[newName.Length - 1] == ' ' || newName.Length == 0)
            {
                newName += original[i].ToString().ToUpper();
            }
            else
            {
                newName += original[i];
            }
        }

        return newName;
    }

    public int TrackPiecesNum()
    {
        return _trackPieces.Count;
    }
}
