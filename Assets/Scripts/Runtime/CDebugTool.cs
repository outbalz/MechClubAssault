using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDebugTool : MonoBehaviour
{
    private CGameProgressManager _gameProgressManager;

    [SerializeField] private float _fundCheat;
    [SerializeField] private float _reputationCheat;

    [SerializeField] private CClubMeetingStateManager _clubMeetingStateManager;
    [SerializeField] private Renderer _renderer;

    [Space]
    [SerializeField] private Color _color = Color.white;

    private void Start()
    {
        _gameProgressManager = CGameProgressManager.Instance;
    }

    [ContextMenu("Apply Cheat")]
    private void ApplyCheat()
    {
        if (_gameProgressManager != null)
        {
            _gameProgressManager.Fund += _fundCheat;
            _gameProgressManager.Reputation += _reputationCheat;
        }

        if (_clubMeetingStateManager != null)
        {
            _clubMeetingStateManager.UpdateFundText();
        }
    }

    [ContextMenu("Apply Color")]
    private void ApplyColor()
    {
        if (_renderer != null)
        {
            _renderer.material.color = _color;
        }
    }

}
