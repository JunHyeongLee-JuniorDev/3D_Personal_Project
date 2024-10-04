using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

/// <summary>
/// 화톳불 쉬기 순간이동 / Json 데이터로 활성화 되었는지를 저장
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class BornFire : MonoBehaviour
{
    //Name : In Inspector
    [SerializeField] private string fireName;
    [Range(0.1f,5.0f)]
    [SerializeField] private float interactionRad;

    //UnityComponents
    private ParticleSystem[] particles;
    private Light fireLight;
    private SphereCollider m_collider;
    //PopUp UI
    private InGamePopUp popUp;

    //Data
    private BornFireData data;

    private void Start()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
        fireLight = GetComponentInChildren<Light>();
        m_collider = GetComponent<SphereCollider>();
        m_collider.enabled = true;
        m_collider.isTrigger = true;
        m_collider.radius = interactionRad;

        if (Managers.Instance != null)
        {
            popUp = Managers.Instance.Game.itemCanvas;
            Debug.Log("팝업 메뉴 null check : " + popUp.name);
            data = Managers.Instance.Data.AssignBornFireData(fireName);
        }

        if (data.isOn)
            LightFire();

        else
            OffFire();
    }

    private void LightFire()
    {
        foreach (var particle in particles)
        {
            particle.gameObject.SetActive(true);
            particle.Play();
        }
            fireLight.gameObject.SetActive(true);
    }

    private void OffFire()
    {
        foreach (var particle in particles)
        {
            particle.Stop();
            particle.gameObject.SetActive(false);
        }
            fireLight.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        popUp.gameObject.SetActive(true);
        popUp.TurnOffIcon();

        if (data.isOn)
        {
            popUp.ChangeText("화톳불에서 쉬기");
            Managers.Instance.Game.playerInput.actions["Interaction"].started -= RestAction;
            Managers.Instance.Game.playerInput.actions["Interaction"].started += RestAction;
        }

        else
        {
            popUp.ChangeText("화톳불 활성화");
            Managers.Instance.Game.playerInput.actions["Interaction"].started -= SetOnFireAction;
            Managers.Instance.Game.playerInput.actions["Interaction"].started += SetOnFireAction;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        popUp.gameObject.SetActive(false);

        if (data.isOn)
            Managers.Instance.Game.playerInput.actions["Interaction"].started -= RestAction;

        else
            Managers.Instance.Game.playerInput.actions["Interaction"].started -= SetOnFireAction;
    }

    private void RestAction(InputAction.CallbackContext context)
    {
        Managers.Instance.Game.ResetGame();
    }

    private void SetOnFireAction(InputAction.CallbackContext context)
    {
        data.isOn = true;
        LightFire();
        Managers.Instance.Game.OnFirePopUp();
        Managers.Instance.Data.SaveGame(Managers.Instance.Data.currentSaveIndex);
    }
}
