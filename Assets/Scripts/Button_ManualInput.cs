﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_ManualInput : MonoBehaviour {

    public GameObject panel_Input;
    public GameObject sourceButton; //打開此面板的button， 在button record傳入
    public string sourceButtonUnit; //打開此面板的button unit， 在button record傳入
    public int sourceButtonType; //打開此面板的button type, 用於顯示對應的manual input， 在button record傳入
    public GameObject recordsPanel;

    private Panel_Input pI;

    public void OnClick()
    {
        panel_Input.SetActive(true);

        pI = panel_Input.GetComponent<Panel_Input>();

        pI.sourceButton = sourceButton;
        pI.sourceButtonType = sourceButtonType;//把召喚此頁面的button Type傳入panel input中


        pI.inputField_1.gameObject.SetActive(true);
        pI.inputField_1.Select();

        SetManualInputPanel(sourceButtonType);
        recordsPanel.GetComponent<Records_Panel>().CloseRecordsPanel(); //關閉record面板好讓推出輸入時record界面更新最新加入的record        
    }

    void SetManualInputPanel(int buttonType)
    {
        switch (buttonType)
        {
            case 0:
                {
                    pI.text_Title_1.text = "Input start time";
                    pI.text_Title_2.text = "Input end time";
                    pI.text_Placeholder_1.text = "hhmm e.g. 1800 for 6pm";
                    pI.text_Placeholder_2.text = "hhmm e.g. 1900 for 7pm";

                    pI.inputField_1.GetComponent<InputField>().characterLimit = 4;
                    pI.inputField_2.GetComponent<InputField>().characterLimit = 4;
                    pI.inputField_2.GetComponent<InputField>().contentType = InputField.ContentType.IntegerNumber;

                    if (!sourceButton.GetComponent<Button_Entry>().timing)
                    {
                        pI.button_Ongoing.SetActive(true);
                        pI.text_Title_2.text = "Input end time or tap Ongoing";
                    }
                }
                break;

            case 1:
                {
                    pI.manualInputDateTime = true;

                    pI.text_Title_1.text = "Input end time";
                    pI.text_Title_2.text = "Input number";
                    pI.text_Placeholder_1.text = "hhmm e.g. 1800 for 6pm";
                    pI.text_Placeholder_2.text = "unit: " + sourceButtonUnit;

                    pI.inputField_1.GetComponent<InputField>().characterLimit = 4;
                    pI.inputField_2.GetComponent<InputField>().characterLimit = 4;
                    pI.inputField_2.GetComponent<InputField>().contentType = InputField.ContentType.IntegerNumber;
                }
                break;

            case 2:
                {
                    pI.manualInputDateTime = true;

                    pI.inputField_1.gameObject.SetActive(false);
                    pI.inputField_2.Select();
                    pI.text_Title_2.text = "Input end time";
                    pI.text_Placeholder_2.text = "hhmm e.g. 1800 for 6pm";

                    pI.inputField_2.GetComponent<InputField>().characterLimit = 4;
                    pI.inputField_2.GetComponent<InputField>().contentType = InputField.ContentType.IntegerNumber;
                }
                break;

            case 3:
                {
                    pI.manualInputDateTime = true;

                    pI.text_Title_1.text = "Input end time";
                    pI.text_Title_2.text = "Input solids type";
                    pI.text_Placeholder_1.text = "hhmm e.g. 1800 for 6pm";
                    pI.text_Placeholder_2.text = "for example : blueberry ";
                    

                    pI.inputField_1.GetComponent<InputField>().characterLimit = 4;
                    pI.inputField_2.GetComponent<InputField>().characterLimit = 30;
                    pI.inputField_2.GetComponent<InputField>().contentType = InputField.ContentType.Standard;
                }
                break;
        }
    }
}
