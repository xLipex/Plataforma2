using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RubyUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text rubyText;

    private void OnEnable()
    {
        // Se inscreve no canal de rubys
        PlayerObserverManager.OnRubysChanged += UpdateRubyText;
    }

    private void OnDisable()
    {
        // Retira a inscrição no canal de coins
        PlayerObserverManager.OnRubysChanged -= UpdateRubyText;
    }

    //Função usada para tratar a notificação do canal de coins
    private void UpdateRubyText(int newRubysValue)
    {
        rubyText.text = newRubysValue.ToString();
    }
}