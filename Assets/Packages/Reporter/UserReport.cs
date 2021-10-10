using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserReport : MonoBehaviour {

    protected Text input, sendingText, resultText;
    protected Button sendBtn, closeBtn;
    protected GameObject form, resultPanel;

	void Start () {
        input = transform.Find("Form/InputField/Text").GetComponent<Text>();
        sendingText = transform.Find("Form/SendingText").GetComponent<Text>();
        resultText = transform.Find("Result/Text").GetComponent<Text>();

        sendBtn = transform.Find("Form/SendBtn").GetComponent<Button>();
        closeBtn = transform.Find("Form/CloseBtn").GetComponent<Button>();

        form = transform.Find("Form").gameObject;
        resultPanel = transform.Find("Result").gameObject;

        resultPanel.SetActive(false);
        sendingText.gameObject.SetActive(false);

        sendBtn.onClick.AddListener(sendBtnClicked);
        closeBtn.onClick.AddListener(closeBtnClicked);
    }

    private void closeBtnClicked()
    {
        DestroyImmediate(gameObject);
    }

    private void sendBtnClicked()
    {
        sendBtn.enabled = sendBtn.interactable = false;
        input.enabled = false;
        sendingText.gameObject.SetActive(true);
        Reporter.SendUserReport("User Report", input.text, "", OnSendingReportComplete);
    }

    private void OnSendingReportComplete(bool result)
    {
        if (result)
        {
            resultText.text = "We received your report.\n Thanks for your feedback.";
        }else
        {
            resultText.text = "An error accured while sending your report.\n Please try again later.";
        }
        form.SetActive(false);
        resultPanel.SetActive(true);

        StartCoroutine(Close(5f));
    }

    IEnumerator Close(float time)
    {
        yield return new WaitForSeconds(time);

        DestroyImmediate(gameObject);
    }
}
