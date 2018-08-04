using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Model;
using PG.Manager;
using PG.Manager.Enum;

public class LoginModel : BaseModel {
    

    public override void Init()
    {
        GameObject TempLogin = Resources.Load<GameObject>("UI/Prefabs/Login");
        WindowManager.GetManager().AddPath(WindowEnum.UILogin, TempLogin);

        //base.Init();
    }
    public override void Open()
    {
        GameObject Temp_View = WindowManager.GetManager().Open_Windows(WindowEnum.UILogin);
        this.m_View = Temp_View.GetComponent<LoginView>();

        base.Open();
    }
    public override void Close()
    {
        base.Close();
    }
    public void Login_Detection(string varAccount, string varPassWord)
    {
        uint UserID = 0;
        byte code = SQLManager.GetManager().User_Detection_AccountAndPassWord(varAccount, varPassWord, out UserID);
        if (code == 0)
        {
            //登陆成功
            Debug.LogError("登陆成功");
        }
        else
        {
            string msg = SQLManager.GetManager().GetErrorCode(code);
            Debug.LogError(msg);
        }
    }
    public void SignIn_Detection(string varAccount, string varPassWord,string Temp_ConfirmPassWord, byte varSex, string varNickName)
    {
        byte code = Account_Detection(varAccount);
        if (code != 0)
        {
            string msg = SQLManager.GetManager().GetErrorCode(code);
            Debug.LogError(msg);
            return;
        }
        code = PassWord_Detection(varPassWord);
        if (code != 0)
        {
            string msg = SQLManager.GetManager().GetErrorCode(code);
            Debug.LogError(msg);
            return;
        }
        code = SQLManager.GetManager().User_Detection_Sigin(varAccount, varPassWord, varSex, varNickName);
        if (code == 0)
        {
            //注册成功
            Debug.LogError("注册成功");
        }
        else
        {
            string msg = SQLManager.GetManager().GetErrorCode(code);
            Debug.LogError(msg);
            return;
        }
    }


    /// <summary>
    /// 账号检测
    /// 账号长度必须大于10，且只能有字母、数字构成
    /// </summary>
    /// <param name="varAccount"></param>
    /// <returns></returns>
    protected byte Account_Detection(string varAccount)
    {
        if (string.IsNullOrEmpty(varAccount))
            return 8;           //账号不能为空
        if (varAccount.Length < 5)
            return 11;          //账号的长度不足
        char[] TempAccount = varAccount.ToCharArray();

        string DetectionStr = "1234567890ABCDEFGHIJKLMNOPQLSTUVWXYZabcdefghijklmnopqlstuvwxyz";

        for (int i = 0; i < TempAccount.Length; i++)
        {
            char varchar = TempAccount[i];
            if (DetectionStr.Contains(varchar.ToString()))
                continue;
            return 12;          //账号只能有数字和字母构成
        }
        return 0;
    }
    /// <summary>
    /// 密码检测
    /// 密码长度必须大于6，且只能有字母、数字构成
    /// </summary>
    /// <param name="varPassWord"></param>
    /// <returns></returns>
    protected byte PassWord_Detection(string varPassWord)
    {
        if (string.IsNullOrEmpty(varPassWord))
            return 9;           //密码不能为空
        if (varPassWord.Length < 6)
            return 13;          //密码长度不足
        string DetectionStr = "1234567890ABCDEFGHIJKLMNOPQLSTUVWXYZabcdefghijklmnopqlstuvwxyz";
        for (int i = 0; i < varPassWord.Length; i++)
        {
            char varchar = varPassWord[i];
            if (DetectionStr.Contains(varchar.ToString()))
                continue;
            return 14;          //密码只能有数字和字母构成
        }
        return 0;
    }
}
