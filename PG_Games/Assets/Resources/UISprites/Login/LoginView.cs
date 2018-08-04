using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Model;
using PG.Manager;
using PG.Helper;

public class LoginView : BaseView {


    #region 注册组件
    [Header("SignIn")]
    public InputField m_SignIn_Account;         //账号
    public InputField m_SignIn_Password;        //密码    
    public InputField m_SignIn_ConfirmPassword; //密码
    public InputField m_SignIn_NickName;        //昵称

    #endregion

    #region 登陆组件
    [Header("Login")]
    public InputField m_Login_Account;        //账号
    public InputField m_Login_Password;       //密码  

    #endregion

    public override void Init()
    {
        FindTran();
        SettingCompent();
    }
    protected int CaretWidth = 5;

    protected void FindTran()
    {
        GameObject Temp_Register = Helper.FindGame(this.transform, "Root/Register");
        if (Temp_Register != null)
            Temp_Register.SetActive(false);
        GameObject Temp_Login = Helper.FindGame(this.transform, "Root/Login");
        if (Temp_Login != null)
            Temp_Login.SetActive(true);


        Button Login_Login_Btn = Helper.GetComponent<Button>(this.transform, "Root/Login/Login");
        Button Login_Register_Btn = Helper.GetComponent<Button>(this.transform, "Root/Login/Register");

        Login_Login_Btn.onClick.AddListener(Btn_Login);
        Login_Register_Btn.onClick.AddListener(()=> {
            Temp_Register.SetActive(true);
            Temp_Login.SetActive(false);
        });

        Button Register_Register_Btn = Helper.GetComponent<Button>(this.transform, "Root/Register/Register");
        Button Register_Cancel_Btn = Helper.GetComponent<Button>(this.transform, "Root/Register/Cancel");

        Register_Register_Btn.onClick.AddListener(Btn_SignIn);
        Register_Cancel_Btn.onClick.AddListener(() => {
            Temp_Register.SetActive(false);
            Temp_Login.SetActive(true);
        });
    }


    /// <summary>
    /// 设置账号密码的输入类型，和最大输入长度
    /// </summary>
    protected void SettingCompent()
    {
        SettingInputField(m_SignIn_Account, InputField.ContentType.Alphanumeric, Constant.LEN_Account);
        SettingInputField(m_SignIn_Password, InputField.ContentType.Password, Constant.LEN_PassWord);
        SettingInputField(m_SignIn_ConfirmPassword, InputField.ContentType.Password, Constant.LEN_PassWord);

        SettingInputField(m_Login_Account, InputField.ContentType.Alphanumeric, Constant.LEN_Account);
        SettingInputField(m_Login_Password, InputField.ContentType.Password, Constant.LEN_PassWord);

    }
    /// <summary>
    /// 设置输入框信息
    /// </summary>
    /// <param name="inputField"></param>
    /// <param name="contentType"></param>
    /// <param name="characterLimit"></param>
    private void SettingInputField(InputField inputField, InputField.ContentType contentType, int characterLimit)
    {
        inputField.contentType = contentType;
        inputField.caretWidth = Constant.LEN_InputWith;
        inputField.characterLimit = characterLimit;
    }

    protected void Btn_Login()
    {
        string Temp_Account = m_Login_Account.text;
        string Temp_PassWord = m_Login_Password.text;
        
        LoginModel model = ModelManager.GetManager().GetModel<LoginModel>();

        model.Login_Detection(Temp_Account, Temp_PassWord);
    }
    protected void Btn_SignIn()
    {
        string Temp_Account = m_SignIn_Account.text;
        string Temp_PassWord = m_SignIn_Password.text;
        string Temp_ConfirmPassWord = m_SignIn_ConfirmPassword.text;
        string Temp_Name = m_SignIn_NickName.text;
        
        

        LoginModel model = ModelManager.GetManager().GetModel<LoginModel>();

        model.SignIn_Detection(Temp_Account, Temp_PassWord, Temp_ConfirmPassWord, 0, Temp_Name);
    }

}
