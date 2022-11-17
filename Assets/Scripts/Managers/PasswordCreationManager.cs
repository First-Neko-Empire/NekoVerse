using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordCreationManager : Singleton<PasswordCreationManager>
{
	[SerializeField]
	private TMP_InputField if_enterPassword;
	[SerializeField]
	private TMP_InputField if_confirmPassword;
	[SerializeField]
	private Button btn_Done;

	private string userPassword;

	public string UserPassword
    {
		get { return userPassword; }
		private set { userPassword = value; }
	}

	private void Start()
	{
	}
	public void OnPasswordInputValueChanged()
	{
		btn_Done.interactable = if_enterPassword.text == if_confirmPassword.text;
		userPassword = if_enterPassword.text;
	}

}
