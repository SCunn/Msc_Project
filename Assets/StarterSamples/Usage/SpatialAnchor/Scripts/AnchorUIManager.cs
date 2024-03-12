/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


/// <summary>
/// Manages UI of anchor sample.
/// </summary>
[RequireComponent(typeof(SpatialAnchorLoader))]
public class AnchorUIManager : MonoBehaviour
{
    /// <summary>
    /// Anchor UI manager singleton instance
    /// </summary>
    public static AnchorUIManager Instance;

    /// <summary>
    /// Anchor Mode switches between create and select
    /// </summary>
    /// 
    /////////////////////////////////// Added for Saving Prefabs to Spatial Anchors ///////////////////////
    private Anchor _anchor;
    private GameObject _selectedPrefab;
    ///////////////////////////////////////////////////////////////////////////////////////////////
    public enum AnchorMode
    {
        Create,
        Select
    };

    [SerializeField, FormerlySerializedAs("createModeButton_")]
    private GameObject _createModeButton;

    [SerializeField, FormerlySerializedAs("selectModeButton_")]
    private GameObject _selectModeButton;

    [SerializeField, FormerlySerializedAs("trackedDevice_")]
    private Transform _trackedDevice;

    private Transform _raycastOrigin;

    private bool _drawRaycast = false;

    [SerializeField, FormerlySerializedAs("lineRenderer_")]
    private LineRenderer _lineRenderer;

    private Anchor _hoveredAnchor;
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // private Anchor _selectedAnchor;
    public Anchor _selectedAnchor;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private AnchorMode _mode = AnchorMode.Select;

    [SerializeField, FormerlySerializedAs("buttonList_")]
    private List<Button> _buttonList;

    private int _menuIndex = 0;

    private Button _selectedButton;

    [SerializeField]
    private Anchor _anchorPrefab;

    public Anchor AnchorPrefab => _anchorPrefab;

    [SerializeField, FormerlySerializedAs("placementPreview_")]
    private GameObject _placementPreview;

    [SerializeField, FormerlySerializedAs("anchorPlacementTransform_")]
    private Transform _anchorPlacementTransform;

    private delegate void PrimaryPressDelegate();

    private PrimaryPressDelegate _primaryPressDelegate;

    private bool _isFocused = true;

    #region Monobehaviour Methods

    /////////////////////////////////// Added for Switching Between objects ///////////////////////
    [SerializeField] private Transform selectablePrefabsTransform;
    private int selectedPrefabIndex;
    [SerializeField] private List<GameObject> selectablePrefabs;
    // private int selectedPrefabIndex = 0;
    
    ///////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {   
        /////////////////////////////////// Added for Saving Prefabs to Spatial Anchors ///////////////////////
        // _anchor = GetComponent<Anchor>();
        // if (_anchor == null)
        // {
        //     Debug.LogError("Anchor script not found!");
        //     return;
        // }

        ///////////////////////////////////////////////////////////////////////////////////////////////

        _raycastOrigin = _trackedDevice;

        // Start in select mode
        _mode = AnchorMode.Select;
        StartSelectMode();

        _menuIndex = 0;
        _selectedButton = _buttonList[0];
        _selectedButton.OnSelect(null);

        _lineRenderer.startWidth = 0.005f;
        _lineRenderer.endWidth = 0.005f;
    /////////////////////////////////// Added for Switching Between objects ///////////////////////
        // Get all the prefabs from the selectable prefabs transform
        selectablePrefabs = new List<GameObject>(); 
        // Loop through all the children of the selectable prefabs transform
        foreach (Transform child in selectablePrefabsTransform)
        {   // Add the child to the list of selectable prefabs
            selectablePrefabs.Add(child.gameObject);
        }
    //////////////////////////////////////////////////////////
    }

    private void Update()
    {
        if (_drawRaycast)
        {
            ControllerRaycast();
        }

        if (_selectedAnchor == null)
        {
            // Refocus menu
            _selectedButton.OnSelect(null);
            _isFocused = true;
        }

        HandleMenuNavigation();

        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            _primaryPressDelegate?.Invoke();
        }
    }

    #endregion // Monobehaviour Methods


    #region Menu UI Callbacks

    /// <summary>
    /// Create mode button pressed UI callback. Referenced by the Create button in the menu.
    /// </summary>
    public void OnCreateModeButtonPressed()
    {
        ToggleCreateMode();
        _createModeButton.SetActive(!_createModeButton.activeSelf);
        _selectModeButton.SetActive(!_selectModeButton.activeSelf);
    }

    /// <summary>
    /// Load anchors button pressed UI callback. Referenced by the Load Anchors button in the menu.
    /// </summary>
    public void OnLoadAnchorsButtonPressed()
    {
        GetComponent<SpatialAnchorLoader>().LoadAnchorsByUuid();
    }

    #endregion // Menu UI Callbacks

    #region Mode Handling

    private void ToggleCreateMode()
    {
        if (_mode == AnchorMode.Select)
        {
            _mode = AnchorMode.Create;
            EndSelectMode();
            StartPlacementMode();
        }
        else
        {
            _mode = AnchorMode.Select;
            EndPlacementMode();
            StartSelectMode();
        }
    }

    private void StartPlacementMode()
    {
        ShowAnchorPreview();
        _primaryPressDelegate = PlaceAnchor;
    }

    private void EndPlacementMode()
    {
        HideAnchorPreview();
        _primaryPressDelegate = null;
    }

    private void StartSelectMode()
    {
        ShowRaycastLine();
        _primaryPressDelegate = SelectAnchor;
    }

    private void EndSelectMode()
    {
        HideRaycastLine();
        _primaryPressDelegate = null;
    }

    #endregion // Mode Handling


    #region Private Methods

    private void HandleMenuNavigation()
    {
        if (!_isFocused)
        {
            return;
        }

        if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickUp))
        {
            NavigateToIndexInMenu(false);
        }

        if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickDown))
        {
            NavigateToIndexInMenu(true);
        }

        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            _selectedButton.OnSubmit(null);
        }
    /////////////////////////////////// Added for Switching Between objects ///////////////////////
        // Switch between prefabs using the X and Y buttons
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            SelectPreviousPrefab();
        }
        else if (OVRInput.GetDown(OVRInput.RawButton.Y))
        {
            SelectNextPrefab();
        }
    //////////////////////////////////////////////////////////
    }

    private void NavigateToIndexInMenu(bool moveNext)
    {
        if (moveNext)
        {
            _menuIndex++;
            if (_menuIndex > _buttonList.Count - 1)
            {
                _menuIndex = 0;
            }
        }
        else
        {
            _menuIndex--;
            if (_menuIndex < 0)
            {
                _menuIndex = _buttonList.Count - 1;
            }
        }

        _selectedButton.OnDeselect(null);
        _selectedButton = _buttonList[_menuIndex];
        _selectedButton.OnSelect(null);
    }

    private void ShowAnchorPreview()
    {
        _placementPreview.SetActive(true);
    }

    private void HideAnchorPreview()
    {
        _placementPreview.SetActive(false);
    }

    private void PlaceAnchor()
    {
        Instantiate(_anchorPrefab, _anchorPlacementTransform.position, _anchorPlacementTransform.rotation);
    }

    private void ShowRaycastLine()
    {
        _drawRaycast = true;
        _lineRenderer.gameObject.SetActive(true);
    }

    private void HideRaycastLine()
    {
        _drawRaycast = false;
        _lineRenderer.gameObject.SetActive(false);
    }

    private void ControllerRaycast()
    {
        Ray ray = new Ray(_raycastOrigin.position, _raycastOrigin.TransformDirection(Vector3.forward));
        _lineRenderer.SetPosition(0, _raycastOrigin.position);
        _lineRenderer.SetPosition(1,
            _raycastOrigin.position + _raycastOrigin.TransformDirection(Vector3.forward) * 10f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Anchor anchorObject = hit.collider.GetComponent<Anchor>();
            if (anchorObject != null)
            {
                _lineRenderer.SetPosition(1, hit.point);

                HoverAnchor(anchorObject);
                return;
            }
        }

        UnhoverAnchor();
    }

    private void HoverAnchor(Anchor anchor)
    {
        UnhoverAnchor();
        _hoveredAnchor = anchor;
        _hoveredAnchor.OnHoverStart();
    }

    private void UnhoverAnchor()
    {
        if (_hoveredAnchor == null)
        {
            return;
        }

        _hoveredAnchor.OnHoverEnd();
        _hoveredAnchor = null;
    }

    private void SelectAnchor()
    {
        if (_hoveredAnchor != null)
        {
            if (_selectedAnchor != null)
            {
                // Deselect previous Anchor
                _selectedAnchor.OnSelect();
                _selectedAnchor = null;
            }

            // Select new Anchor
            _selectedAnchor = _hoveredAnchor;
            _selectedAnchor.OnSelect();

            // Defocus menu
            _selectedButton.OnDeselect(null);
            _isFocused = false;
        }
        else
        {
            if (_selectedAnchor != null)
            {
                // Deselect previous Anchor
                _selectedAnchor.OnSelect();
                _selectedAnchor = null;

                // Refocus menu
                _selectedButton.OnSelect(null);
                _isFocused = true;
            }
        }
    }

    #endregion // Private Methods

    /////////////////////////////////// Added for Switching Between objects ///////////////////////
    
    // Selects the previous prefab in the list
    private void SelectPreviousPrefab()
    {
        selectedPrefabIndex--;  // Decrement the selected prefab index
        if (selectedPrefabIndex < 0)    // If the selected prefab index is less than 0
        {
            selectedPrefabIndex = selectablePrefabs.Count - 1;  // Set the selected prefab index to the last index
        }
        UpdateSelectedPrefab(); // Update the selected prefab
    }
    // Selects the next prefab in the list
    private void SelectNextPrefab()
    {
        selectedPrefabIndex++;  // Increment the selected prefab index
        if (selectedPrefabIndex >= selectablePrefabs.Count)  // If the selected prefab index is greater than or equal to the number of prefabs
        {
            selectedPrefabIndex = 0;        // Set the selected prefab index to 0
        }
        UpdateSelectedPrefab();         // Update the selected prefab
    }
    // Updates the selected prefab to the one at the current index
    private void UpdateSelectedPrefab()
    {
        // Highlight the selected prefab visually
        for (int i = 0; i < selectablePrefabs.Count; i++) // Loop through all prefabs
        {
            if (i == selectedPrefabIndex)   // If the current index is the selected index
            {
                selectablePrefabs[i].SetActive(true);       // Activate the prefab
            }
            else
            {
                selectablePrefabs[i].SetActive(false);    // else Deactivate the prefab
            }
        }

        // Update primary press delegate for spawning
        _primaryPressDelegate = () => SpawnSelectedPrefab();
    }
    // Spawns the selected prefab at the anchor placement transform
    private void SpawnSelectedPrefab()
    {
        GameObject prefab = selectablePrefabs[selectedPrefabIndex];     // Get the selected prefab
        Instantiate(prefab, _anchorPlacementTransform.position, _anchorPlacementTransform.rotation); // Instantiate the prefab at the anchor placement transform
    }

    /////////////////////////////////////////////////// Added for Saving Prefabs to Spatial Anchors ////////////////////////////////////////////////
    // public void OnSelectPrefab(GameObject prefab)
    // {
    //     _selectedPrefab = prefab;   // Store selected prefab for later use
    //     selectedPrefabIndex = selectablePrefabs.IndexOf(prefab);    // Get the index of the selected prefab
    //     SpawnPrefab(prefab); // Spawn the selected prefab
    //     /////////////////////////////////// Added for Saving Prefabs to Spatial Anchors ///////////////////////
        
    //     ///////////////////////////////////////////////////////////////////////////////////////////////
    // }

    // // Prefab spawner
    // private void SpawnPrefab(GameObject prefab)
    // {
    //     Instantiate(prefab, _anchorPlacementTransform.position, _anchorPlacementTransform.rotation); // Instantiate the prefab at the anchor placement transform
    //     // SavePrefabToAnchor(prefab, _anchor._spatialAnchor);
    // }

    // //Anchor saving logic
    // public void OnSaveLocalButtonPressed()
    // {   
    //     if (_selectedPrefab != null && _anchor != null)
    //     {
    //         // Save prefab association after saving anchor data
    //         PrefabDataSaver.Instance.SavePrefabToAnchor(_selectedPrefab, _anchor._spatialAnchor);
    //     }
    // }

    // private void SavePrefabToAnchor(GameObject prefab, OVRSpatialAnchor anchor)
    // {
    //     if (prefab == null || anchor == null) return;
        
    //     // Store prefab information in the anchor's UserData
    //     string prefabData = JsonUtility.ToJson(prefab.name);
    //     anchor.UserData = prefabData;
    // }


    ///////////////////////////////////////////////////////////////////////////////////////////////
}
