﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace NineMenMorris.NoneRule
{
    public class ClickPosUI : UIBehavior<ClickPosUI.UIData>
    {

        #region UIData

        public class UIData : NoneRuleInputUI.UIData.Sub
        {

            public VP<int> x;

            public VP<int> y;

            #region Constructor

            public enum Property
            {
                x,
                y
            }

            public UIData() : base()
            {
                this.x = new VP<int>(this, (byte)Property.x, 0);
                this.y = new VP<int>(this, (byte)Property.y, 0);
            }

            #endregion

            public override Type getType()
            {
                return Type.ClickPos;
            }

            public override bool processEvent(Event e)
            {
                bool isProcess = false;
                {
                    // back
                    if (!isProcess)
                    {
                        if (InputEvent.isBackEvent(e))
                        {
                            ClickPosUI clickPosUI = this.findCallBack<ClickPosUI>();
                            if (clickPosUI != null)
                            {
                                clickPosUI.onClickBtnBack();
                                isProcess = true;
                            }
                            else
                            {
                                Debug.LogError("clickPosUI null: " + this);
                            }
                        }
                    }
                    // shortKey
                    if (!isProcess)
                    {
                        if (Setting.get().useShortKey.v)
                        {
                            ClickPosUI clickPosUI = this.findCallBack<ClickPosUI>();
                            if (clickPosUI != null)
                            {
                                isProcess = clickPosUI.useShortKey(e);
                            }
                            else
                            {
                                Debug.LogError("clickPosUI null: " + this);
                            }
                        }
                    }
                }
                return isProcess;
            }

        }

        #endregion

        public override int getStartAllocate()
        {
            return Setting.get().defaultChosenGame.v.getGame() == GameType.Type.NineMenMorris ? 1 : 0;
        }

        #region txt

        public Text lbTitle;

        public Button btnSetPiece;
        public Text tvSetPiece;

        public Button btnMove;
        public Text tvMove;

        public Button btnEndTurn;
        public Text tvEndTurn;

        public Button btnClear;
        public Text tvClear;

        #endregion

        #region Refresh

        public GameObject ivSelect;
        public Transform contentContainer;

        public Button btnBack;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // imgSelect
                    {
                        if (ivSelect != null)
                        {
                            // position
                            ivSelect.transform.localPosition = new Vector2(this.data.x.v - 3, this.data.y.v - 3);
                            // Scale
                            {
                                int playerView = GameDataBoardUI.UIData.getPlayerView(this.data);
                                ivSelect.transform.localScale = (playerView == 0 ? new Vector3(1f, 1f, 1f) : new Vector3(1f, -1f, 1f));
                            }
                        }
                        else
                        {
                            Debug.LogError("imgSelect null: " + this);
                        }
                    }
                    // btnMove
                    {
                        if (btnMove != null)
                        {
                            bool isClickPiece = false;
                            {
                                NoneRuleInputUI.UIData noneRuleInputUIData = this.data.findDataInParent<NoneRuleInputUI.UIData>();
                                if (noneRuleInputUIData != null)
                                {
                                    NineMenMorris nineMenMorris = noneRuleInputUIData.nineMenMorris.v.data;
                                    if (nineMenMorris != null)
                                    {
                                        int square = Common.ConvertXYToPosition(this.data.x.v, this.data.y.v);
                                        Common.SpotStatus piece = (Common.SpotStatus)nineMenMorris.pieceOn(square);
                                        if (piece != Common.SpotStatus.SS_Empty)
                                        {
                                            isClickPiece = true;
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError("chess null: " + this);
                                    }
                                }
                                else
                                {
                                    Debug.LogError("noneRuleInputuIData null: " + this);
                                }
                            }
                            btnMove.gameObject.SetActive(isClickPiece);
                        }
                        else
                        {
                            Debug.LogError("btnMove null: " + this);
                        }
                    }
                    // UI
                    {
                        float deltaY = 0;
                        // header
                        {
                            UIRectTransform.SetButtonTopLeftTransform(btnBack);
                            UIRectTransform.SetTitleTransform(lbTitle);
                            deltaY += Setting.get().getButtonSize() + 10;
                        }
                        // btnSetPiece
                        {
                            if (btnSetPiece != null && btnSetPiece.gameObject.activeSelf)
                            {
                                UIRectTransform.SetPosY((RectTransform)btnSetPiece.transform, deltaY);
                                deltaY += 40;
                            }
                            else
                            {
                                Debug.LogError("btnSetPiece null");
                            }
                        }
                        // btnMove
                        {
                            if (btnMove != null && btnMove.gameObject.activeSelf)
                            {
                                UIRectTransform.SetPosY((RectTransform)btnMove.transform, deltaY);
                                deltaY += 40;
                            }
                            else
                            {
                                Debug.LogError("btnMove null");
                            }
                        }
                        // btnEndTurn
                        {
                            if (btnEndTurn != null && btnEndTurn.gameObject.activeSelf)
                            {
                                UIRectTransform.SetPosY((RectTransform)btnEndTurn.transform, deltaY);
                                deltaY += 40;
                            }
                            else
                            {
                                Debug.LogError("btnEndTurn null");
                            }
                        }
                        // btnClear
                        {
                            if (btnClear != null && btnClear.gameObject.activeSelf)
                            {
                                UIRectTransform.SetPosY((RectTransform)btnClear.transform, deltaY);
                                deltaY += 40;
                            }
                            else
                            {
                                Debug.LogError("btnClear null");
                            }
                        }
                        // set
                        if (contentContainer != null)
                        {
                            UIRectTransform.SetHeight((RectTransform)contentContainer, deltaY);
                        }
                        else
                        {
                            Debug.LogError("contentContainer null");
                        }
                    }
                    // txt
                    {
                        if (lbTitle != null)
                        {
                            lbTitle.text = ClickPosTxt.txtClickPosTitle.get();
                            Setting.get().setTitleTextSize(lbTitle);
                        }
                        else
                        {
                            Debug.LogError("lbTitle null");
                        }
                        if (tvSetPiece != null)
                        {
                            tvSetPiece.text = ClickPosTxt.txtClickPosSetPiece.get();
                        }
                        else
                        {
                            Debug.LogError("tvSetPiece null");
                        }
                        if (tvMove != null)
                        {
                            tvMove.text = ClickPosTxt.txtClickPosMove.get();
                        }
                        else
                        {
                            Debug.LogError("tvMove null");
                        }
                        if (tvEndTurn != null)
                        {
                            tvEndTurn.text = ClickPosTxt.txtClickPosEndTurn.get();
                        }
                        else
                        {
                            Debug.LogError("tvEndTurn null");
                        }
                        if (tvClear != null)
                        {
                            tvClear.text = ClickPosTxt.txtClickPosClear.get();
                        }
                        else
                        {
                            Debug.LogError("tvClear null");
                        }
                    }
                }
                else
                {
                    // Debug.LogError ("data null: " + this);
                }
            }
        }

        public override bool isShouldDisableUpdate()
        {
            return false;
        }

        #endregion

        #region implement callBacks

        private GameDataBoardCheckPerspectiveChange<UIData> perspectiveChange = new GameDataBoardCheckPerspectiveChange<UIData>();
        private NoneRuleInputUI.UIData noneRuleInputUIData = null;

        public override void onAddCallBack<T>(T data)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Setting
                Setting.get().addCallBack(this);
                // CheckChange
                {
                    perspectiveChange.addCallBack(this);
                    perspectiveChange.setData(uiData);
                }
                // Parent
                {
                    DataUtils.addParentCallBack(uiData, this, ref this.noneRuleInputUIData);
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
            // CheckChange
            if (data is GameDataBoardCheckPerspectiveChange<UIData>)
            {
                dirty = true;
                return;
            }
            // Parent
            {
                if (data is NoneRuleInputUI.UIData)
                {
                    NoneRuleInputUI.UIData noneRuleInputUIData = data as NoneRuleInputUI.UIData;
                    // Child
                    {
                        noneRuleInputUIData.nineMenMorris.allAddCallBack(this);
                    }
                    dirty = true;
                    return;
                }
                // Child
                if (data is NineMenMorris)
                {
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
                // CheckChange
                {
                    perspectiveChange.removeCallBack(this);
                    perspectiveChange.setData(null);
                }
                // Parent
                {
                    DataUtils.removeParentCallBack(uiData, this, ref this.noneRuleInputUIData);
                }
                this.setDataNull(uiData);
                return;
            }
            // Setting
            if (data is Setting)
            {
                return;
            }
            // CheckChange
            if (data is GameDataBoardCheckPerspectiveChange<UIData>)
            {
                return;
            }
            // Parent
            {
                if (data is NoneRuleInputUI.UIData)
                {
                    NoneRuleInputUI.UIData noneRuleInputUIData = data as NoneRuleInputUI.UIData;
                    // Child
                    {
                        noneRuleInputUIData.nineMenMorris.allRemoveCallBack(this);
                    }
                    return;
                }
                // Child
                if (data is NineMenMorris)
                {
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
                    case UIData.Property.x:
                        dirty = true;
                        break;
                    case UIData.Property.y:
                        dirty = true;
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
                    case Setting.Property.contentTextSize:
                        dirty = true;
                        break;
                    case Setting.Property.titleTextSize:
                        dirty = true;
                        break;
                    case Setting.Property.labelTextSize:
                        dirty = true;
                        break;
                    case Setting.Property.buttonSize:
                        dirty = true;
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
            // CheckChange
            if (wrapProperty.p is GameDataBoardCheckPerspectiveChange<UIData>)
            {
                dirty = true;
                return;
            }
            // Parent
            {
                if (wrapProperty.p is NoneRuleInputUI.UIData)
                {
                    switch ((NoneRuleInputUI.UIData.Property)wrapProperty.n)
                    {
                        case NoneRuleInputUI.UIData.Property.nineMenMorris:
                            {
                                ValueChangeUtils.replaceCallBack(this, syncs);
                                dirty = true;
                            }
                            break;
                        case NoneRuleInputUI.UIData.Property.sub:
                            break;
                        default:
                            Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                            break;
                    }
                    return;
                }
                // Child
                if (wrapProperty.p is NineMenMorris)
                {
                    switch ((NineMenMorris.Property)wrapProperty.n)
                    {
                        case NineMenMorris.Property.board:
                            dirty = true;
                            break;
                        case NineMenMorris.Property.moved:
                            break;
                        case NineMenMorris.Property.moved_to:
                            break;
                        case NineMenMorris.Property.action:
                            break;
                        case NineMenMorris.Property.mill:
                            break;
                        case NineMenMorris.Property.terminal:
                            break;
                        case NineMenMorris.Property.removed:
                            break;
                        case NineMenMorris.Property.utility:
                            break;
                        case NineMenMorris.Property.turn:
                            break;
                        case NineMenMorris.Property.isCustom:
                            break;
                        default:
                            Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                            break;
                    }
                    return;
                }
            }
            Debug.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

        public override void Awake()
        {
            base.Awake();
            // OnClick
            {
                UIUtils.SetButtonOnClick(btnSetPiece, onClickBtnSetPiece);
                UIUtils.SetButtonOnClick(btnMove, onClickBtnMove);
                UIUtils.SetButtonOnClick(btnEndTurn, onClickBtnEnd);
                UIUtils.SetButtonOnClick(btnClear, onClickBtnClear);
            }
        }

        public bool useShortKey(Event e)
        {
            bool isProcess = false;
            {
                if (e.isKey && e.type == EventType.KeyUp)
                {
                    switch (e.keyCode)
                    {
                        case KeyCode.S:
                            {
                                if (btnSetPiece != null && btnSetPiece.gameObject.activeInHierarchy && btnSetPiece.interactable)
                                {
                                    this.onClickBtnSetPiece();
                                    isProcess = true;
                                }
                                else
                                {
                                    Debug.LogError("cannot click");
                                }
                            }
                            break;
                        case KeyCode.M:
                            {
                                if (btnMove != null && btnMove.gameObject.activeInHierarchy && btnMove.interactable)
                                {
                                    this.onClickBtnMove();
                                    isProcess = true;
                                }
                                else
                                {
                                    Debug.LogError("cannot click");
                                }
                            }
                            break;
                        case KeyCode.E:
                            {
                                if (btnEndTurn != null && btnEndTurn.gameObject.activeInHierarchy && btnEndTurn.interactable)
                                {
                                    this.onClickBtnEnd();
                                    isProcess = true;
                                }
                                else
                                {
                                    Debug.LogError("cannot click");
                                }
                            }
                            break;
                        case KeyCode.Clear:
                            {
                                if (btnClear != null && btnClear.gameObject.activeInHierarchy && btnClear.interactable)
                                {
                                    this.onClickBtnClear();
                                    isProcess = true;
                                }
                                else
                                {
                                    Debug.LogError("cannot click");
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return isProcess;
        }

        [UnityEngine.Scripting.Preserve]
        public void onClickBtnBack()
        {
            if (this.data != null)
            {
                NoneRuleInputUI.UIData noneRuleInputUIData = this.data.findDataInParent<NoneRuleInputUI.UIData>();
                if (noneRuleInputUIData != null)
                {
                    ClickNoneUI.UIData clickNoneUIData = noneRuleInputUIData.sub.newOrOld<ClickNoneUI.UIData>();
                    {

                    }
                    noneRuleInputUIData.sub.v = clickNoneUIData;
                }
                else
                {
                    Debug.LogError("noneRuleInputUIData null: " + this);
                }
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }

        [UnityEngine.Scripting.Preserve]
        public void onClickBtnSetPiece()
        {
            if (this.data != null)
            {
                NoneRuleInputUI.UIData noneRuleInputUIData = this.data.findDataInParent<NoneRuleInputUI.UIData>();
                if (noneRuleInputUIData != null)
                {
                    SetPieceUI.UIData setPieceUIData = new SetPieceUI.UIData();
                    {
                        setPieceUIData.uid = noneRuleInputUIData.sub.makeId();
                        setPieceUIData.x.v = this.data.x.v;
                        setPieceUIData.y.v = this.data.y.v;
                    }
                    noneRuleInputUIData.sub.v = setPieceUIData;
                }
                else
                {
                    Debug.LogError("noneRuleInputUIData null: " + this);
                }
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }

        [UnityEngine.Scripting.Preserve]
        public void onClickBtnMove()
        {
            if (this.data != null)
            {
                NoneRuleInputUI.UIData noneRuleInputUIData = this.data.findDataInParent<NoneRuleInputUI.UIData>();
                if (noneRuleInputUIData != null)
                {
                    ClickMoveUI.UIData clickMoveUIData = new ClickMoveUI.UIData();
                    {
                        clickMoveUIData.uid = noneRuleInputUIData.sub.makeId();
                        clickMoveUIData.x.v = this.data.x.v;
                        clickMoveUIData.y.v = this.data.y.v;
                    }
                    noneRuleInputUIData.sub.v = clickMoveUIData;
                }
                else
                {
                    Debug.LogError("noneRuleInputUIData null: " + this);
                }
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }

        [UnityEngine.Scripting.Preserve]
        public void onClickBtnEnd()
        {
            if (this.data != null)
            {
                ClientInput clientInput = InputUI.UIData.findClientInput(this.data);
                if (clientInput != null)
                {
                    EndTurn endTurn = new EndTurn();
                    {

                    }
                    clientInput.makeSend(endTurn);
                }
                else
                {
                    Debug.LogError("clientInput null: " + this);
                }
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }

        [UnityEngine.Scripting.Preserve]
        public void onClickBtnClear()
        {
            if (this.data != null)
            {
                ClientInput clientInput = InputUI.UIData.findClientInput(this.data);
                if (clientInput != null)
                {
                    Clear clear = new Clear();
                    {

                    }
                    clientInput.makeSend(clear);
                }
                else
                {
                    Debug.LogError("clientInput null: " + this);
                }
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }

    }
}