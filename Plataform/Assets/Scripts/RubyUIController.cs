using TMPro;
using UnityEngine;

public class RubyUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text rubyText;

    private void OnEnable()
    {
        // Se inscreve no canal de rubys
        PlayerObserverManager.OnRubyChanged += UpdateRubyText;
    }

    private void OnDisable()
    {
        // Retira a inscrição no canal de coins
        PlayerObserverManager.OnRubyChanged -= UpdateRubyText;
    }

    //Função usada para tratar a notificação do canal de coins
    private void UpdateRubyText(int newRubyValue)
    {
        rubyText.text = newRubyValue.ToString();
    }
}