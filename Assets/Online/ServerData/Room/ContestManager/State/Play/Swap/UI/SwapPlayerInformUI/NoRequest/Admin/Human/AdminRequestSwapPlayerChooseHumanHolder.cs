﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using frame8.Logic.Misc.Visual.UI.ScrollRectItemsAdapter;

namespace GameManager.Match.Swap
{
    public class AdminRequestSwapPlayerChooseHumanHolder : SriaHolderBehavior<AdminRequestSwapPlayerChooseHumanHolder.UIData>
    {

        #region UIData

        public class UIData : BaseItemViewsHolder
        {

            public VP<ReferenceData<Human>> human;

            public VP<AccountAvatarUI.UIData> avatar;

            #region Constructor

            public enum Property
            {
                human,
                avatar
            }

            public UIData() : base()
            {
                this.human = new VP<ReferenceData<Human>>(this, (byte)Property.human, new ReferenceData<Human>(null));
                this.avatar = new VP<AccountAvatarUI.UIData>(this, (byte)Property.avatar, new AccountAvatarUI.UIData());
            }

            #endregion

            public void updateView(AdminRequestSwapPlayerChooseHumanAdapter.UIData myParams)
            {
                // Find Human
                Human human = null;
                {
                    if (ItemIndex >= 0 && ItemIndex < myParams.humans.Count)
                    {
                        human = myParams.humans[ItemIndex];
                    }
                    else
                    {
                        Debug.LogError("ItemIndex error: " + this);
                    }
                }
                // Update
                this.human.v = new ReferenceData<Human>(human);
            }

        }

        #endregion

        #region txt

        public Text tvChoose;
        private static readonly TxtLanguage txtChoose = new TxtLanguage("Choose");

        static AdminRequestSwapPlayerChooseHumanHolder()
        {
            // txt
            txtChoose.add(Language.Type.vi, "Chọn");
            // rect
            {
                // avatarRect
                {
                    // anchoredPosition: (8.0, 0.0); anchorMin: (0.0, 0.5); anchorMax: (0.0, 0.5); pivot: (0.0, 0.5);
                    // offsetMin: (8.0, -20.0); offsetMax: (48.0, 20.0); sizeDelta: (40.0, 40.0);
                    avatarRect.anchoredPosition = new Vector3(8.0f, 0.0f, 0.0f);
                    avatarRect.anchorMin = new Vector2(0.0f, 0.5f);
                    avatarRect.anchorMax = new Vector2(0.0f, 0.5f);
                    avatarRect.pivot = new Vector2(0.0f, 0.5f);
                    avatarRect.offsetMin = new Vector2(8.0f, -20.0f);
                    avatarRect.offsetMax = new Vector2(48.0f, 20.0f);
                    avatarRect.sizeDelta = new Vector2(40.0f, 40.0f);
                }
            }
        }

        #endregion

        #region Refresh

        public Text tvName;

        public override void refresh()
        {
            base.refresh();
            if (this.data != null)
            {
                Human human = this.data.human.v.data;
                if (human != null)
                {
                    // tvName
                    {
                        if (tvName != null)
                        {
                            tvName.text = human.getPlayerName();
                        }
                        else
                        {
                            Debug.LogError("tvName null: " + this);
                        }
                    }
                    // avatar
                    {
                        AccountAvatarUI.UIData avatar = this.data.avatar.v;
                        if (avatar != null)
                        {
                            avatar.account.v = new ReferenceData<Account>(human.account.v);
                        }
                        else
                        {
                            Debug.LogError("avatar null: " + this);
                        }
                    }
                    // txt
                    {
                        if (tvChoose != null)
                        {
                            tvChoose.text = txtChoose.get();
                        }
                        else
                        {
                            Debug.LogError("tvChoose null");
                        }
                    }
                }
                else
                {
                    Debug.LogError("human null: " + this);
                }
            }
            else
            {
                // Debug.LogError ("data null: " + this);
            }
        }

        #endregion

        #region implement callBacks

        public AccountAvatarUI avatarPrefab;
        private static readonly UIRectTransform avatarRect = new UIRectTransform();

        public override void onAddCallBack<T>(T data)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Setting
                Setting.get().addCallBack(this);
                // Child
                {
                    uiData.human.allAddCallBack(this);
                    uiData.avatar.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Setting
            if (data is Setting)
            {
                dirty = true;
                return;
            }
            // Child
            {
                // Human
                {
                    if (data is Human)
                    {
                        Human human = data as Human;
                        // Child
                        {
                            human.account.allAddCallBack(this);
                        }
                        dirty = true;
                        return;
                    }
                    // Child
                    if (data is Account)
                    {
                        dirty = true;
                        return;
                    }
                }
                if (data is AccountAvatarUI.UIData)
                {
                    AccountAvatarUI.UIData avatar = data as AccountAvatarUI.UIData;
                    // UI
                    {
                        UIUtils.Instantiate(avatar, avatarPrefab, this.transform, avatarRect);
                    }
                    dirty = true;
                    return;
                }
            }
            Debug.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Setting
                Setting.get().removeCallBack(this);
                // Child
                {
                    uiData.human.allRemoveCallBack(this);
                    uiData.avatar.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Setting
            if (data is Setting)
            {
                return;
            }
            // Child
            {
                // Human
                {
                    if (data is Human)
                    {
                        Human human = data as Human;
                        // Child
                        {
                            human.account.allRemoveCallBack(this);
                        }
                        return;
                    }
                    // Child
                    if (data is Account)
                    {
                        return;
                    }
                }
                if (data is AccountAvatarUI.UIData)
                {
                    AccountAvatarUI.UIData avatar = data as AccountAvatarUI.UIData;
                    // UI
                    {
                        avatar.removeCallBackAndDestroy(typeof(AccountAvatarUI));
                    }
                    return;
                }
            }
            Debug.LogError("Don't process: " + data + "; " + this);
        }

        public override void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
        {
            if (WrapProperty.checkError(wrapProperty))
            {
                return;
            }
            if (wrapProperty.p is UIData)
            {
                switch ((UIData.Property)wrapProperty.n)
                {
                    case UIData.Property.human:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.avatar:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    default:
                        Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            // Setting
            if (wrapProperty.p is Setting)
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
            // Child
            {
                // Human
                {
                    if (wrapProperty.p is Human)
                    {
                        switch ((Human.Property)wrapProperty.n)
                        {
                            case Human.Property.playerId:
                                break;
                            case Human.Property.account:
                                {
                                    ValueChangeUtils.replaceCallBack(this, syncs);
                                    dirty = true;
                                }
                                break;
                            case Human.Property.state:
                                break;
                            case Human.Property.email:
                                break;
                            case Human.Property.phoneNumber:
                                break;
                            case Human.Property.status:
                                break;
                            case Human.Property.birthday:
                                break;
                            case Human.Property.sex:
                                break;
                            case Human.Property.connection:
                                break;
                            case Human.Property.ban:
                                break;
                            default:
                                Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                                break;
                        }
                        return;
                    }
                    // Child
                    if (wrapProperty.p is Account)
                    {
                        Account.OnUpdateSyncAccount(wrapProperty, this);
                        return;
                    }
                }
                if (wrapProperty.p is AccountAvatarUI.UIData)
                {
                    return;
                }
            }
            Debug.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

        [UnityEngine.Scripting.Preserve]
        public void onClickBtnChoose()
        {
            if (this.data != null)
            {
                Human human = this.data.human.v.data;
                if (human != null)
                {
                    AdminRequestSwapPlayerHumanUI.UIData adminRequestSwapPlayerHumanUIData = this.data.findDataInParent<AdminRequestSwapPlayerHumanUI.UIData>();
                    if (adminRequestSwapPlayerHumanUIData != null)
                    {
                        if (adminRequestSwapPlayerHumanUIData.state.v is AdminRequestSwapPlayerHumanUI.UIData.StateNone)
                        {
                            AdminRequestSwapPlayerHumanUI.UIData.StateRequest stateRequest = new AdminRequestSwapPlayerHumanUI.UIData.StateRequest();
                            {
                                stateRequest.uid = adminRequestSwapPlayerHumanUIData.state.makeId();
                                stateRequest.humanId.v = human.playerId.v;
                            }
                            adminRequestSwapPlayerHumanUIData.state.v = stateRequest;
                        }
                        else
                        {
                            Debug.LogError("you are requesting: " + this);
                        }
                    }
                    else
                    {
                        Debug.LogError("adminRequestSwapPlayerHumanUIData null: " + this);
                    }
                }
                else
                {
                    Debug.LogError("human null: " + this);
                }
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }

    }
}