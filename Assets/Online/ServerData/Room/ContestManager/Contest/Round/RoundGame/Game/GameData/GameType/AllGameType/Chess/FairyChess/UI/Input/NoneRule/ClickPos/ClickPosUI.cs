﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace FairyChess.NoneRule
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

			public override Type getType ()
			{
				return Type.ClickPos;
			}

			public override bool processEvent (Event e)
			{
				bool isProcess = false;
				{
					// back
					if (!isProcess) {
						if (InputEvent.isBackEvent (e)) {
							ClickPosUI clickPosUI = this.findCallBack<ClickPosUI> ();
							if (clickPosUI != null) {
								clickPosUI.onClickBtnBack ();
							} else {
								Debug.LogError ("clickPosUI null: " + this);
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

		public GameObject ivSelect;
		public Transform contentContainer;

		public Button btnMove;

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					// imgSelect
					{
						if (ivSelect != null) {
							// position
							ivSelect.transform.localPosition = Common.convertXYToLocalPosition (this.data.x.v, this.data.y.v);
							// Scale
							{
								int playerView = GameDataBoardUI.UIData.getPlayerView (this.data);
								ivSelect.transform.localScale = (playerView == 0 ? new Vector3 (1f, 1f, 1f) : new Vector3 (1f, -1f, 1f));
							}
						} else {
							Debug.LogError ("imgSelect null: " + this);
						}
					}
					// Scale
					{
						if (contentContainer != null) {
							int playerView = GameDataBoardUI.UIData.getPlayerView (this.data);
							float scale = 0.015f;
							contentContainer.localScale = (playerView == 0 ? new Vector3 (scale, scale, 1f) : new Vector3 (scale, -scale, 1f));
						} else {
							Debug.LogError ("contentContainer null: " + this);
						}
					}
					// btnMove
					{
						if (btnMove != null) {
							bool isClickPiece = false;
							{
								NoneRuleInputUI.UIData noneRuleInputUIData = this.data.findDataInParent<NoneRuleInputUI.UIData> ();
								if (noneRuleInputUIData != null) {
									FairyChess fairyChess = noneRuleInputUIData.fairyChess.v.data;
									if (fairyChess != null) {
										Common.Square square = Common.make_square ((Common.File)this.data.x.v, (Common.Rank)this.data.y.v);
										Common.Piece piece = fairyChess.piece_on (square);
										if (piece != Common.Piece.NO_PIECE && piece != Common.Piece.PIECE_NB) {
											isClickPiece = true;
										}
									} else {
										Debug.LogError ("fairyChess null: " + this);
									}
								} else {
									Debug.LogError ("noneRuleInputuIData null: " + this);
								}
							}
							btnMove.gameObject.SetActive (isClickPiece);
						} else {
							Debug.LogError ("btnMove null: " + this);
						}
					}
				} else {
					// Debug.LogError ("data null: " + this);
				}
			}
		}

		public override bool isShouldDisableUpdate ()
		{
			return false;
		}

		#endregion

		#region implement callBacks

		private GameDataBoardCheckPerspectiveChange<UIData> perspectiveChange = new GameDataBoardCheckPerspectiveChange<UIData>();
		private NoneRuleInputUI.UIData noneRuleInputUIData = null;

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// CheckChange
				{
					perspectiveChange.addCallBack (this);
					perspectiveChange.setData (uiData);
				}
				// Parent
				{
					DataUtils.addParentCallBack (uiData, this, ref this.noneRuleInputUIData);
				}
				dirty = true;
				return;
			}
			// CheckChange
			if (data is GameDataBoardCheckPerspectiveChange<UIData>) {
				dirty = true;
				return;
			}
			// Parent
			{
				if (data is NoneRuleInputUI.UIData) {
					NoneRuleInputUI.UIData noneRuleInputUIData = data as NoneRuleInputUI.UIData;
					// Child
					{
						noneRuleInputUIData.fairyChess.allAddCallBack (this);
					}
					dirty = true;
					return;
				}
				// Child
				if (data is FairyChess) {
					dirty = true;
					return;
				}
			}
			Debug.LogError ("Don't process: " + data + "; " + this);
		}

		public override void onRemoveCallBack<T> (T data, bool isHide)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
				// CheckChange
				{
					perspectiveChange.removeCallBack (this);
					perspectiveChange.setData (null);
				}
				// Parent
				{
					DataUtils.removeParentCallBack (uiData, this, ref this.noneRuleInputUIData);
				}
				this.setDataNull (uiData);
				return;
			}
			// CheckChange
			if (data is GameDataBoardCheckPerspectiveChange<UIData>) {
				return;
			}
			// Parent
			{
				if (data is NoneRuleInputUI.UIData) {
					NoneRuleInputUI.UIData noneRuleInputUIData = data as NoneRuleInputUI.UIData;
					// Child
					{
						noneRuleInputUIData.fairyChess.allRemoveCallBack (this);
					}
					return;
				}
				// Child
				if (data is FairyChess) {
					return;
				}
			}
			Debug.LogError ("Don't process: " + data + "; " + this);
		}

		public override void onUpdateSync<T> (WrapProperty wrapProperty, List<Sync<T>> syncs)
		{
			if (WrapProperty.checkError (wrapProperty)) {
				return;
			}
			if (wrapProperty.p is UIData) {
				switch ((UIData.Property)wrapProperty.n) {
				case UIData.Property.x:
					dirty = true;
					break;
				case UIData.Property.y:
					dirty = true;
					break;
				default:
					Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
					break;
				}
				return;
			}
			// CheckChange
			if (wrapProperty.p is GameDataBoardCheckPerspectiveChange<UIData>) {
				dirty = true;
				return;
			}
			// Parent
			{
				if (wrapProperty.p is NoneRuleInputUI.UIData) {
					switch ((NoneRuleInputUI.UIData.Property)wrapProperty.n) {
					case NoneRuleInputUI.UIData.Property.fairyChess:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case NoneRuleInputUI.UIData.Property.sub:
						break;
					default:
						Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
						break;
					}
					return;
				}
				// Child
				if (wrapProperty.p is FairyChess) {
					switch ((FairyChess.Property)wrapProperty.n) {
					case FairyChess.Property.board:
						dirty = true;
						break;
					case FairyChess.Property.unpromotedBoard:
						break;
					case FairyChess.Property.byTypeBB:
						break;
					case FairyChess.Property.byColorBB:
						break;
					case FairyChess.Property.pieceCount:
						break;
					case FairyChess.Property.pieceList:
						break;
					case FairyChess.Property.index:
						break;
					case FairyChess.Property.castlingRightsMask:
						break;
					case FairyChess.Property.castlingRookSquare:
						break;
					case FairyChess.Property.castlingPath:
						break;
					case FairyChess.Property.gamePly:
						break;
					case FairyChess.Property.sideToMove:
						break;
					case FairyChess.Property.variantType:
						break;
					case FairyChess.Property.st:
						break;
					case FairyChess.Property.chess960:
						break;
					case FairyChess.Property.pieceCountInHand:
						break;
					case FairyChess.Property.promotedPieces:
						break;
					default:
						Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
						break;
					}
					return;
				}
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion

		public void onClickBtnBack()
		{
			if (this.data != null) {
				NoneRuleInputUI.UIData noneRuleInputUIData = this.data.findDataInParent<NoneRuleInputUI.UIData> ();
				if (noneRuleInputUIData != null) {
					ClickNoneUI.UIData clickNoneUIData = noneRuleInputUIData.sub.newOrOld<ClickNoneUI.UIData> ();
					{

					}
					noneRuleInputUIData.sub.v = clickNoneUIData;
				} else {
					Debug.LogError ("noneRuleInputUIData null: " + this);
				}
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

		public void onClickBtnSetPiece()
		{
			if (this.data != null) {
				NoneRuleInputUI.UIData noneRuleInputUIData = this.data.findDataInParent<NoneRuleInputUI.UIData> ();
				if (noneRuleInputUIData != null) {
					SetPieceUI.UIData setPieceUIData = new SetPieceUI.UIData ();
					{
						setPieceUIData.uid = noneRuleInputUIData.sub.makeId ();
						setPieceUIData.x.v = this.data.x.v;
						setPieceUIData.y.v = this.data.y.v;
					}
					noneRuleInputUIData.sub.v = setPieceUIData;
				} else {
					Debug.LogError ("noneRuleInputUIData null: " + this);
				}
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

		public void onClickBtnMove()
		{
			if (this.data != null) {
				NoneRuleInputUI.UIData noneRuleInputUIData = this.data.findDataInParent<NoneRuleInputUI.UIData> ();
				if (noneRuleInputUIData != null) {
					ClickMoveUI.UIData clickMoveUIData = new ClickMoveUI.UIData ();
					{
						clickMoveUIData.uid = noneRuleInputUIData.sub.makeId ();
						clickMoveUIData.x.v = this.data.x.v;
						clickMoveUIData.y.v = this.data.y.v;
					}
					noneRuleInputUIData.sub.v = clickMoveUIData;
				} else {
					Debug.LogError ("noneRuleInputUIData null: " + this);
				}
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

		public void onClickBtnEnd()
		{
			if (this.data != null) {
				ClientInput clientInput = InputUI.UIData.findClientInput (this.data);
				if (clientInput != null) {
					EndTurn endTurn = new EndTurn ();
					{

					}
					clientInput.makeSend (endTurn);
				} else {
					Debug.LogError ("clientInput null: " + this);
				}
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

		public void onClickBtnClear()
		{
			if (this.data != null) {
				ClientInput clientInput = InputUI.UIData.findClientInput (this.data);
				if (clientInput != null) {
					Clear clear = new Clear ();
					{

					}
					clientInput.makeSend (clear);
				} else {
					Debug.LogError ("clientInput null: " + this);
				}
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

		public void onClickBtnSetHand()
		{
			if (this.data != null) {
				NoneRuleInputUI.UIData noneRuleInputUIData = this.data.findDataInParent<NoneRuleInputUI.UIData> ();
				if (noneRuleInputUIData != null) {
					SetHandUI.UIData setHandUIData = new SetHandUI.UIData ();
					{
						setHandUIData.uid = noneRuleInputUIData.sub.makeId ();
					}
					noneRuleInputUIData.sub.v = setHandUIData;
				} else {
					Debug.LogError ("noneRuleInputUIData null: " + this);
				}
			} else {
				Debug.LogError ("data null: " + this);
			}
		}

	}
}