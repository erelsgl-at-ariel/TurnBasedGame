﻿using UnityEngine;
using System.Collections;

public class Setting : Data
{

	#region Get

	private static Setting instance;

	static Setting()
	{
		instance = new Setting ();
	}

	public static Setting get()
	{
		return instance;
	}

	#endregion

	#region Property

	public VP<Language.Type> language;

    #region style

    public enum Style
    {
        Normal,
        Western
    }

    public VP<Style> style;

    #endregion

    public VP<bool> showLastMove;

	public VP<bool> viewUrlImage;

	public VP<AnimationSetting> animationSetting;

    #endregion


#if UNITY_ANDROID

    public const int DefaultMaxThinkCount = 1;

#else

    public const int DefaultMaxThinkCount = 12;

#endif

    public VP<int> maxThinkCount;

    #region defaultChosenGame

    public VP<DefaultChosenGame> defaultChosenGame;

    public void changeDefaultChosenGameType(DefaultChosenGame.Type newType)
    {
        if (this.defaultChosenGame.v.getType() != newType)
        {
            GameType.Type oldGameType = this.defaultChosenGame.v.getGame();
            // make new
            switch (newType)
            {
                case DefaultChosenGame.Type.Last:
                    {
                        DefaultChosenGameLast defaultChosenGameLast = new DefaultChosenGameLast();
                        {
                            defaultChosenGameLast.uid = this.defaultChosenGame.makeId();
                            defaultChosenGameLast.gameType.v = oldGameType;
                        }
                        this.defaultChosenGame.v = defaultChosenGameLast;
                    }
                    break;
                case DefaultChosenGame.Type.Always:
                    {
                        DefaultChosenGameAlways defaultChosenGameAlways = new DefaultChosenGameAlways();
                        {
                            defaultChosenGameAlways.uid = this.defaultChosenGame.makeId();
                            defaultChosenGameAlways.gameType.v = oldGameType;
                        }
                        this.defaultChosenGame.v = defaultChosenGameAlways;
                    }
                    break;
                default:
                    Debug.LogError("unknown type: " + newType);
                    break;
            }
        }
        else
        {
            Debug.LogError("the same type: " + newType);
        }
    }

    #endregion

    #region defaultChatRoomStyle

    public VP<DefaultChatRoomStyle> defaultChatRoomStyle;

    public void changeDefaultChatRoomStyle(DefaultChatRoomStyle.Type newType)
    {
        if (this.defaultChatRoomStyle.v.getType() != newType)
        {
            ContestManagerBtnChatUI.UIData.Visibility oldVisibility = this.defaultChatRoomStyle.v.getVisibility();
            ContestManagerBtnChatUI.UIData.Style oldStyle = this.defaultChatRoomStyle.v.getStyle();
            // make new
            switch (newType)
            {
                case DefaultChatRoomStyle.Type.Last:
                    {
                        DefaultChatRoomStyleLast defaultChatRoomStyleLast = new DefaultChatRoomStyleLast();
                        {
                            defaultChatRoomStyleLast.uid = this.defaultChatRoomStyle.makeId();
                            defaultChatRoomStyleLast.visibility.v = oldVisibility;
                            defaultChatRoomStyleLast.style.v = oldStyle;
                        }
                        this.defaultChatRoomStyle.v = defaultChatRoomStyleLast;
                    }
                    break;
                case DefaultChatRoomStyle.Type.Always:
                    {
                        DefaultChatRoomStyleAlways defaultChatRoomStyleAlways = new DefaultChatRoomStyleAlways();
                        {
                            defaultChatRoomStyleAlways.uid = this.defaultChatRoomStyle.makeId();
                            defaultChatRoomStyleAlways.visibility.v = oldVisibility;
                            defaultChatRoomStyleAlways.style.v = oldStyle;
                        }
                        this.defaultChatRoomStyle.v = defaultChatRoomStyleAlways;
                    }
                    break;
                default:
                    Debug.LogError("unknown type: " + newType);
                    break;
            }
        }
        else
        {
            Debug.LogError("the same type: " + newType);
        }
    }

    #endregion

    #region Constructor

    public enum Property
	{
		language,
        style,
        showLastMove,
		viewUrlImage,
		animationSetting,
		maxThinkCount,
        defaultChosenGame,
        defaultChatRoomStyle
    }

	public Setting() : base()
	{
		this.language = new VP<Language.Type> (this, (byte)Property.language, Language.Type.en);
        this.style = new VP<Style>(this, (byte)Property.style, Style.Normal);
        this.showLastMove = new VP<bool> (this, (byte)Property.showLastMove, true);
		this.viewUrlImage = new VP<bool> (this, (byte)Property.viewUrlImage, true);
		this.animationSetting = new VP<AnimationSetting> (this, (byte)Property.animationSetting, new AnimationSetting ());
		this.maxThinkCount = new VP<int> (this, (byte)Property.maxThinkCount, DefaultMaxThinkCount);
        this.defaultChosenGame = new VP<DefaultChosenGame>(this, (byte)Property.defaultChosenGame, new DefaultChosenGameLast());
        this.defaultChatRoomStyle = new VP<DefaultChatRoomStyle>(this, (byte)Property.defaultChatRoomStyle, new DefaultChatRoomStyleLast());
    }

	#endregion

}