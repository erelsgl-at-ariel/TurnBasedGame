﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LanClientMenuUI : UIBehavior<LanClientMenuUI.UIData>
{

    #region UIData

    public class UIData : LanClientUI.UIData.Sub
    {

        public VP<DiscoveredServers> discoveredServers;

        #region State

        public enum State
        {
            Start,
            Scanning
        }

        public VP<State> state;

        #endregion

        #region Constructor

        public enum Property
        {
            discoveredServers,
            state
        }

        public UIData() : base()
        {
            this.discoveredServers = new VP<DiscoveredServers>(this, (byte)Property.discoveredServers, null);
            this.state = new VP<State>(this, (byte)Property.state, State.Start);
        }

        #endregion

        public override LanClientUI.UIData.Sub.Type getType()
        {
            return LanClientUI.UIData.Sub.Type.Menu;
        }

        #region Join

        public void onClickJoin(DiscoveredServer discoveredServer)
        {
            // Debug.LogError ("onClickJoin: " + discoveredServer);
            if (discoveredServer != null)
            {
                if (discoveredServer.version.v == Global.VersionCode)
                {
                    LanClientUI.UIData lanClientUIData = this.findDataInParent<LanClientUI.UIData>();
                    if (lanClientUIData != null)
                    {
                        LanClientPlayUI.UIData newUIData = new LanClientPlayUI.UIData();
                        {
                            Server server = newUIData.serverManager.v.server.v.data;
                            if (server != null)
                            {
                                server.serverConfig.v.address.v = discoveredServer.ipAddress.v;
                                server.serverConfig.v.port.v = discoveredServer.port.v;
                            }
                            else
                            {
                                Debug.LogError("server null");
                            }
                        }
                        lanClientUIData.sub.v = newUIData;
                    }
                    else
                    {
                        Debug.LogError("Cannot find lanClientUIData");
                    }
                }
                else
                {
                    Debug.LogError("not correct version code: " + discoveredServer);
                }
            }
            else
            {
                Debug.LogError("discoveredServer null");
            }
        }

        #endregion

        public override bool processEvent(Event e)
        {
            bool isProcess = false;
            {
                // back
                if (!isProcess)
                {
                    if (InputEvent.isBackEvent(e))
                    {
                        LanClientMenuUI lanClientMenuUI = this.findCallBack<LanClientMenuUI>();
                        if (lanClientMenuUI != null)
                        {
                            lanClientMenuUI.onClickBtnBack();
                        }
                        else
                        {
                            Debug.LogError("lanClientMenuUI null: " + this);
                        }
                        isProcess = true;
                    }
                }
            }
            return isProcess;
        }

    }

    #endregion

    #region Refresh

    private float time = 0;
    private bool alreadyInit = false;

    public Text tvState;
    private static readonly TxtLanguage txtStart = new TxtLanguage();
    private static readonly TxtLanguage txtScan = new TxtLanguage();

    public Text lbTitle;
    private static readonly TxtLanguage txtTitle = new TxtLanguage();

    public Text tvBack;
    private static readonly TxtLanguage txtBack = new TxtLanguage();

    public Text tvNoLan;
    private static readonly TxtLanguage txtNoLan = new TxtLanguage();

    static LanClientMenuUI()
    {
        // txt
        {
            txtStart.add(Language.Type.vi, "Đang bắt đầu");
            txtScan.add(Language.Type.vi, "Đang quét");
            txtTitle.add(Language.Type.vi, "Chọn mạng LAN");
            txtBack.add(Language.Type.vi, "Quay lại");
            txtNoLan.add(Language.Type.vi, "Không có server LAN nào cả");
        }
    }

    public override void refresh()
    {
        if (dirty)
        {
            dirty = false;
            if (this.data != null)
            {
                // tvState
                if (tvState != null)
                {
                    switch (this.data.state.v)
                    {
                        case UIData.State.Start:
                            tvState.text = txtStart.get("Starting...");
                            break;
                        case UIData.State.Scanning:
                            tvState.text = txtScan.get("Scanning...");
                            break;
                        default:
                            Debug.LogError("unknown state: " + this.data.state.v);
                            break;
                    }
                }
                else
                {
                    Debug.LogError("tvState null: " + this);
                }
                // lbTitle
                if (lbTitle != null)
                {
                    lbTitle.text = txtTitle.get("Choose LAN server");
                }
                else
                {
                    Debug.LogError("lbTitle null: " + this);
                }
                // tvBack
                if (tvBack != null)
                {
                    tvBack.text = txtBack.get("Back");
                }
                else
                {
                    Debug.LogError("tvBack null: " + this);
                }
                // tvNoLan
                if (tvNoLan != null)
                {
                    tvNoLan.text = txtNoLan.get("Don't have any LAN servers");
                    bool haveAnyLan = false;
                    {
                        DiscoveredServers discoveredServers = this.data.discoveredServers.v;
                        if (discoveredServers != null)
                        {
                            haveAnyLan = discoveredServers.servers.vs.Count > 0;
                        }
                        else
                        {
                            Debug.LogError("disceveredServers null");
                        }
                    }
                    tvNoLan.gameObject.SetActive(!haveAnyLan);
                }
                else
                {
                    Debug.LogError("tvNoLan null");
                }
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }
        if (!alreadyInit)
        {
            time += Time.fixedDeltaTime;
            if (time >= 1f)
            {
                alreadyInit = true;
                onClickBtnJoin();
            }
        }
    }

    public override bool isShouldDisableUpdate()
    {
        return false;
    }

    #endregion

    #region implement callBacks

    public override void onAddCallBack<T>(T data)
    {
        if (data is UIData)
        {
            UIData uiData = data as UIData;
            // Setting
            Setting.get().addCallBack(this);
            // DiscoveredServerListUI
            {
                DiscoveredServerListUI discoveredServerListUI = this.GetComponentInChildren<DiscoveredServerListUI>();
                if (discoveredServerListUI != null)
                {
                    // set data
                    {
                        ClientNetworkDiscovery clientNetworkDiscovery = this.GetComponentInChildren<ClientNetworkDiscovery>();
                        if (clientNetworkDiscovery != null)
                        {
                            discoveredServerListUI.setData(clientNetworkDiscovery.discoveredServers);
                        }
                        else
                        {
                            Debug.LogError("clientNetworkDiscovery null: " + this);
                        }
                    }
                }
                uiData.discoveredServers.v = discoveredServerListUI.data;
            }
            // Child
            {
                uiData.discoveredServers.allAddCallBack(this);
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
        if(data is DiscoveredServers)
        {
            dirty = true;
            return;
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
            // DiscoveredServerListUI
            {
                DiscoveredServerListUI discoveredServerListUI = this.GetComponentInChildren<DiscoveredServerListUI>();
                if (discoveredServerListUI != null)
                {
                    // set data
                    {
                        ClientNetworkDiscovery clientNetworkDiscovery = this.GetComponentInChildren<ClientNetworkDiscovery>();
                        if (clientNetworkDiscovery != null)
                        {
                            discoveredServerListUI.setData(null);
                        }
                        else
                        {
                            Debug.LogError("clientNetworkDiscovery null: " + this);
                        }
                    }
                }
            }
            // Child
            {
                uiData.discoveredServers.allRemoveCallBack(this);
            }
            return;
        }
        // Setting
        if (data is Setting)
        {
            return;
        }
        // Child
        if(data is DiscoveredServers)
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
        if (wrapProperty.p is UIData)
        {
            switch ((UIData.Property)wrapProperty.n)
            {
                case UIData.Property.discoveredServers:
                    {
                        ValueChangeUtils.replaceCallBack(this, syncs);
                        dirty = true;
                    }
                    break;
                case UIData.Property.state:
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
        if(wrapProperty.p is DiscoveredServers)
        {
            switch ((DiscoveredServers.Property)wrapProperty.n)
            {
                case DiscoveredServers.Property.servers:
                    dirty = true;
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

    public void onClickBtnJoin()
    {
        // Debug.LogError ("onClickBtnJoin");
        ClientNetworkDiscovery clientNetworkDiscovery = FindObjectOfType<ClientNetworkDiscovery>();
        if (clientNetworkDiscovery != null)
        {
            clientNetworkDiscovery.StartJoining();

            // change state
            if (this.data != null)
            {
                this.data.state.v = UIData.State.Scanning;
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }
        else
        {
            // Debug.LogError ("clientNetworkDiscovery null");
        }
    }

    public void onClickBtnBack()
    {
        // Debug.LogError ("onClickBtnBack");
        if (this.data != null)
        {
            LanUI.UIData lanUIData = this.data.findDataInParent<LanUI.UIData>();
            if (lanUIData != null)
            {
                if (lanUIData.sub.v.getType() != LanUI.UIData.Sub.Type.Menu)
                {
                    lanUIData.sub.v = new LanMenuUI.UIData();
                }
                else
                {
                    Debug.LogError("Why already menu");
                }
            }
            else
            {
                Debug.LogError("lanUIData null");
            }
        }
        else
        {
            Debug.LogError("uiData null");
        }
    }
}