﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BtnSaveGameUI : UIBehavior<BtnSaveGameUI.UIData>
{

    #region UIData

    public class UIData : Data
    {

        public VP<ReferenceData<Game>> game;

        #region Constructor

        public enum Property
        {
            game
        }

        public UIData() : base()
        {
            this.game = new VP<ReferenceData<Game>>(this, (byte)Property.game, new ReferenceData<Game>(null));
        }

        #endregion

    }

    #endregion

    #region txt

    public Text lbTitle;
    private static readonly TxtLanguage txtTitle = new TxtLanguage();

    static BtnSaveGameUI()
    {
        txtTitle.add(Language.Type.vi, "Lưu Trữ");
    }

    #endregion

    #region Refresh

    public override void refresh()
    {
        if (dirty)
        {
            dirty = false;
            if (this.data != null)
            {
                Game game = this.data.game.v.data;
                if (game != null)
                {
                    // txt
                    {
                        if (lbTitle != null)
                        {
                            lbTitle.text = txtTitle.get("Save");
                        }
                        else
                        {
                            Debug.LogError("lbTitle null");
                        }
                    }
                }
                else
                {
                    Debug.LogError("game null");
                }
            }
            else
            {
                Debug.LogError("data null");
            }
        }
    }

    public override bool isShouldDisableUpdate()
    {
        return true;
    }

    #endregion

    #region implement callBacks

    public override void onAddCallBack<T>(T data)
    {
        if(data is UIData)
        {
            // Setting
            Setting.get().addCallBack(this);
            return;
        }
        // Setting
        if(data is Setting)
        {
            dirty = true;
            return;
        }
        Debug.LogError("Don't process: " + data + "; " + this);
    }

    public override void onRemoveCallBack<T>(T data, bool isHide)
    {
        if(data is UIData)
        {
            UIData uiData = data as UIData;
            // Setting
            Setting.get().removeCallBack(this);
            this.setDataNull(uiData);
            return;
        }
        // Setting
        if (data is Setting)
        {
            return;
        }
        Debug.LogError("Don't process: " + data + "; " + this);
    }

    public override void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
    {
        if (WrapProperty.checkError(wrapProperty))
        {
            return;
        }
        if(wrapProperty.p is UIData)
        {
            switch ((UIData.Property)wrapProperty.n)
            {
                case UIData.Property.game:
                    dirty = true;
                    break;
                default:
                    Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                    break;
            }
            return;
        }
        // Setting
        if(wrapProperty.p is Setting)
        {
            switch ((Setting.Property)wrapProperty.n)
            {
                case Setting.Property.language:
                    dirty = true;
                    break;
                case Setting.Property.style:
                    break;
                case Setting.Property.showLastMove:
                    break;
                case Setting.Property.viewUrlImage:
                    break;
                case Setting.Property.animationSetting:
                    break;
                case Setting.Property.maxThinkCount:
                    break;
                default:
                    Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                    break;
            }
            return;
        }
        Debug.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
    }

    #endregion

    public void onClickBtnSave()
    {
        if (this.data != null)
        {
            GameUI.UIData gameUIData = this.data.findDataInParent<GameUI.UIData>();
            if (gameUIData != null)
            {
                SaveUI.UIData saveUIData = gameUIData.saveUIData.newOrOld<SaveUI.UIData>();
                {

                }
                gameUIData.saveUIData.v = saveUIData;
            }
            else
            {
                Debug.LogError("gameUIData null");
            }
        }
        else
        {
            Debug.LogError("data null");
        }
    }

}