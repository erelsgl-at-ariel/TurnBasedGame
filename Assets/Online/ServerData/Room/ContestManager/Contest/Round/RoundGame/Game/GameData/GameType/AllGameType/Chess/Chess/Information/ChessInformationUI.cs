﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Chess
{
    public class ChessInformationUI : UIHaveTransformDataBehavior<ChessInformationUI.UIData>
    {

        #region UIData

        public class UIData : GameTypeInformationUI.UIData.Sub
        {

            public VP<ReferenceData<Chess>> chess;

            public VP<UIRectTransform.ShowType> showType;

            public VP<ChessFenUI.UIData> chessFenUIData;

            #region Constructor

            public enum Property
            {
                chess,
                showType,
                chessFenUIData
            }

            public UIData() : base()
            {
                this.chess = new VP<ReferenceData<Chess>>(this, (byte)Property.chess, new ReferenceData<Chess>(null));
                this.showType = new VP<UIRectTransform.ShowType>(this, (byte)Property.showType, UIRectTransform.ShowType.Normal);
                this.chessFenUIData = new VP<ChessFenUI.UIData>(this, (byte)Property.chessFenUIData, new ChessFenUI.UIData());
            }

            #endregion

            public override GameType.Type getType()
            {
                return GameType.Type.CHESS;
            }

            public override bool processEvent(Event e)
            {
                bool isProcess = false;
                {
                    // shortKey
                    if (!isProcess)
                    {
                        if (Setting.get().useShortKey.v)
                        {
                            ChessInformationUI chessInformationUI = this.findCallBack<ChessInformationUI>();
                            if (chessInformationUI != null)
                            {
                                isProcess = chessInformationUI.useShortKey(e);
                            }
                            else
                            {
                                Debug.LogError("chessInformationUI null: " + this);
                            }
                        }
                    }
                }
                return isProcess;
            }

        }

        #endregion

        #region txt, rect

        public Text lbTitle;

        public Text tvMessage;
        private static readonly TxtLanguage txtMessage = new TxtLanguage("https://en.wikipedia.org/wiki/Chess");

        static ChessInformationUI()
        {
            txtMessage.add(Language.Type.vi, "https://vi.wikipedia.org/wiki/C%E1%BB%9D_vua");
        }

        #endregion

        #region Refresh

        public Text lbFen;
        public Button btnCopyFen;

        public Text lbChess960;
        public Toggle tgChess960;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Chess chess = this.data.chess.v.data;
                    if (chess != null)
                    {
                        // fen
                        {
                            ChessFenUI.UIData chessFenUIData = this.data.chessFenUIData.v;
                            if (chessFenUIData != null)
                            {
                                chessFenUIData.chess.v = new ReferenceData<Chess>(chess);
                            }
                            else
                            {
                                Debug.LogError("chessFenUIData null");
                            }
                        }
                        // tgChess960
                        {
                            if (tgChess960 != null)
                            {
                                tgChess960.interactable = false;
                                tgChess960.isOn = chess.chess960.v;
                            }
                            else
                            {
                                Debug.LogError("tgChess960 null");
                            }
                        }
                        // UI
                        {
                            float deltaY = 0;
                            // header
                            UIUtils.SetHeaderPosition(lbTitle, this.data.showType.v, ref deltaY);
                            // tvMessage
                            {
                                if (tvMessage != null)
                                {
                                    UIRectTransform.SetPosY(tvMessage.rectTransform, deltaY);
                                    deltaY += 30;
                                }
                                else
                                {
                                    Debug.LogError("tvMessage null");
                                }
                            }
                            // fen
                            {
                                if (this.data.chessFenUIData.v != null)
                                {
                                    if (lbFen != null)
                                    {
                                        lbFen.gameObject.SetActive(true);
                                        UIRectTransform.SetPosY(lbFen.rectTransform, deltaY);
                                    }
                                    else
                                    {
                                        Debug.LogError("lbFen null");
                                    }
                                    if (btnCopyFen != null)
                                    {
                                        btnCopyFen.gameObject.SetActive(true);
                                        UIRectTransform.SetPosY((RectTransform)btnCopyFen.transform, deltaY + (UIConstants.ItemHeight - 30) / 2);
                                    }
                                    else
                                    {
                                        Debug.LogError("btnCopyFen null");
                                    }
                                    UIRectTransform.SetPosY(this.data.chessFenUIData.v, deltaY);
                                    deltaY += UIConstants.ItemHeight;
                                }
                                else
                                {
                                    if (lbFen != null)
                                    {
                                        lbFen.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        Debug.LogError("lbFen null");
                                    }
                                    if (btnCopyFen != null)
                                    {
                                        btnCopyFen.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        Debug.LogError("btnCopyFen null");
                                    }
                                }
                            }
                            // chess960
                            {
                                if (lbChess960 != null)
                                {
                                    lbChess960.gameObject.SetActive(true);
                                    UIRectTransform.SetPosY(lbChess960.rectTransform, deltaY);
                                }
                                else
                                {
                                    Debug.LogError("lbChess960 null");
                                }
                                if (tgChess960 != null)
                                {
                                    UIRectTransform.SetPosY((RectTransform)tgChess960.transform, deltaY + (UIConstants.ItemHeight - UIRectTransform.CommonToggleHeight) / 2);
                                }
                                else
                                {
                                    Debug.LogError("tgChess960 null");
                                }
                                deltaY += UIConstants.ItemHeight;
                            }
                            // set
                            UIRectTransform.SetHeight((RectTransform)this.transform, deltaY);
                        }
                        // txt
                        {
                            if (lbTitle != null)
                            {
                                lbTitle.text = GameType.GetStrGameType(GameType.Type.CHESS);
                                Setting.get().setTitleTextSize(lbTitle);
                            }
                            else
                            {
                                Debug.LogError("lbTitle null");
                            }
                            if (tvMessage != null)
                            {
                                tvMessage.text = txtMessage.get();
                                Setting.get().setContentTextSize(tvMessage);
                            }
                            else
                            {
                                Debug.LogError("tvMessage null");
                            }
                            if (lbFen != null)
                            {
                                Setting.get().setLabelTextSize(lbFen);
                            }
                            else
                            {
                                Debug.LogError("lbFen null");
                            }
                            if (lbChess960 != null)
                            {
                                Setting.get().setLabelTextSize(lbChess960);
                            }
                            else
                            {
                                Debug.LogError("lbChess960 null");
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("chess null");
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

        public ChessFenUI chessFenPrefab;
        private static readonly UIRectTransform chessFenRect = UIRectTransform.createRequestRect(90, 50, 60);

        public override void onAddCallBack<T>(T data)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Setting
                Setting.get().addCallBack(this);
                // Child
                {
                    uiData.chess.allAddCallBack(this);
                    uiData.chessFenUIData.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Setting
            if(data is Setting)
            {
                dirty = true;
                return;
            }
            // Child
            {
                if(data is Chess)
                {
                    dirty = true;
                    return;
                }
                if(data is ChessFenUI.UIData)
                {
                    ChessFenUI.UIData chessFenUIData = data as ChessFenUI.UIData;
                    // UI
                    {
                        UIUtils.Instantiate(chessFenUIData, chessFenPrefab, this.transform, chessFenRect);
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
                    uiData.chess.allRemoveCallBack(this);
                    uiData.chessFenUIData.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Setting
            if(data is Setting)
            {
                return;
            }
            // Child
            {
                if (data is Chess)
                {
                    return;
                }
                if (data is ChessFenUI.UIData)
                {
                    ChessFenUI.UIData chessFenUIData = data as ChessFenUI.UIData;
                    // UI
                    {
                        chessFenUIData.removeCallBackAndDestroy(typeof(ChessFenUI));
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
                    case UIData.Property.chess:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.showType:
                        dirty = true;
                        break;
                    case UIData.Property.chessFenUIData:
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
            if(wrapProperty.p is Setting)
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
                    case Setting.Property.defaultChosenGame:
                        break;
                    case Setting.Property.defaultRoomName:
                        break;
                    case Setting.Property.defaultChatRoomStyle:
                        break;
                    default:
                        Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            // Child
            {
                if (wrapProperty.p is Chess)
                {
                    switch ((Chess.Property)wrapProperty.n)
                    {
                        case Chess.Property.board:
                            break;
                        case Chess.Property.byTypeBB:
                            break;
                        case Chess.Property.byColorBB:
                            break;
                        case Chess.Property.pieceCount:
                            break;
                        case Chess.Property.pieceList:
                            break;
                        case Chess.Property.index:
                            break;
                        case Chess.Property.castlingRightsMask:
                            break;
                        case Chess.Property.castlingRookSquare:
                            break;
                        case Chess.Property.castlingPath:
                            break;
                        case Chess.Property.gamePly:
                            break;
                        case Chess.Property.sideToMove:
                            break;
                        case Chess.Property.st:
                            break;
                        case Chess.Property.chess960:
                            dirty = true;
                            break;
                        case Chess.Property.isCustom:
                            break;
                        default:
                            Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                            break;
                    }
                    return;
                }
                if (wrapProperty.p is ChessFenUI.UIData)
                {
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
                UIUtils.SetButtonOnClick(btnCopyFen, onClickBtnCopyFen);
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
                        case KeyCode.C:
                            {
                                if (btnCopyFen != null && btnCopyFen.gameObject.activeInHierarchy && btnCopyFen.interactable)
                                {
                                    this.onClickBtnCopyFen();
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
        public void onClickBtnCopyFen()
        {
            if (this.data != null)
            {
                ChessFenUI.UIData chessFenUIData = this.data.chessFenUIData.v;
                if (chessFenUIData != null)
                {
                    ChessFenUI chessFenUI = chessFenUIData.findCallBack<ChessFenUI>();
                    if (chessFenUI != null)
                    {
                        Text tvFen = chessFenUI.tvFen;
                        if (tvFen != null)
                        {
                            string fen = tvFen.text;
                            UniClipboard.SetText(fen);
                            Toast.showMessage("Copy Fen: " + fen);
                        }
                        else
                        {
                            Debug.LogError("tvFen null");
                        }
                    }
                    else
                    {
                        Debug.LogError("chessFenUI null");
                    }
                }
                else
                {
                    Debug.LogError("chessFenUIData null");
                }
            }
            else
            {
                Debug.LogError("data null");
            }
        }

    }
}