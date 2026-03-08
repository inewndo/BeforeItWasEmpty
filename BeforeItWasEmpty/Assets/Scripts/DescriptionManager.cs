using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("UI")]
    public GameObject textPannel;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI lineText;
    public Transform cameraTransform;
    private Interactable currrentInteractable;

    private int lineIndex;
    private bool isActive;
    private ObjectData currentNode;
    

    private CCPlayer player;

    [Header("Puzzle")]
    private int PuzzlePiece = 0;
    public TextMeshProUGUI PiecesCollectedText;
    public GameObject box;


    private void Awake()
    {
        if (textPannel != null) textPannel.SetActive(false);
        player = FindFirstObjectByType<CCPlayer>();
    }

    private void OnEnable()
    {
        CCPlayer.OnDescriptionRequested += StartDescription;
    }
    private void OnDisable()
    {
        CCPlayer.OnDescriptionRequested -= StartDescription;
    }

    private void Update()
    {
        if (!isActive) return;
        if(Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Advanced();
        }
    }
    void StartDescription(ObjectData objectData)
    {
        player.DisableInput();
        if(objectData == null)
        {
            Debug.Log("Data is null");
            return;
        }
        currentNode = objectData;
        lineIndex = 0;
        isActive = true;
        if (textPannel != null) textPannel.SetActive(true);
        ShowLine();
       
    }
    void ShowLine()
    {
        if(currentNode == null)
        {
            EndDescription();
            return;
        }
        if (displayName != null) displayName.text = currentNode.displayName;
        if (currentNode.lines == null || currentNode.lines.Length == 0)
        {
            FinishNode();
            return;
        }
        lineIndex = Mathf.Clamp(lineIndex, 0, currentNode.lines.Length - 1);
        if (lineText != null) lineText.text = currentNode.lines[lineIndex];
    }
    void FinishNode()
    {
        if(currentNode.nextNode != null)
        {
            currentNode = currentNode.nextNode;
            lineIndex = 0;
            return;
        }
        EndDescription();
    }

    void Advanced()
    {
        if(currentNode == null)
        {
            EndDescription();
            
            return;
        }
        lineIndex++;
        if (currentNode.lines != null && lineIndex < currentNode.lines.Length)
        {
            if(lineText != null)
            {
                lineText.text = currentNode.lines[lineIndex];
                return;
            }
        }
        FinishNode();
    }
    void EndDescription()
    {
        textPannel.SetActive(false);
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            if(hit.collider.tag == "Interactable")
            {
                Destroy(hit.collider.gameObject);

            }
        }
        isActive = false;
        currentNode = null;
        lineIndex = 0;

        PuzzlePiece++;
        PiecesCollectedText.text = "Collected: " + PuzzlePiece.ToString();
        if (PuzzlePiece == 6)
        {
            PiecesCollectedText.text = "Find the puzzle box and complete the Puzzle!";
            box.SetActive(true);
        }
        player.EnableInput();
    }
  
}
