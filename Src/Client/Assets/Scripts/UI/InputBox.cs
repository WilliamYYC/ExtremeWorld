using UnityEngine;

class InputBox {
    static Object cacheObject = null;

    public static UIInputBox show(string title, string message, string btnOK = "", string btnCancel = "", string emptyTips = "")
    {
        if (cacheObject == null)
        {
            cacheObject = Resloader.Load<Object>("UI/UIInputBox");
        }

        GameObject go = (GameObject)GameObject.Instantiate(cacheObject);
        UIInputBox inputBox = go.GetComponent<UIInputBox>();
        inputBox.Init(title, message, btnOK, btnCancel, emptyTips);
        return inputBox;
    }
}
