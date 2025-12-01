CREATE TABLE [dbo].[ViewPropertyBase] (
    [ViewPropertyID]          INT            NOT NULL,
    [IsHide]                  BIT            NOT NULL,
    [IsVisible]               BIT            NULL,
    [IsDisabled]              BIT            NULL,
    [Comment]                 VARCHAR (500)  NULL,
    [IsValid]                 BIT            NULL,
    [ErrorMesage]             VARCHAR (100)  NULL,
    [Display]                 VARCHAR (100)  NULL,
    [ToolTipMessage]          VARCHAR (100)  NULL,
    [ControlType]             VARCHAR (100)  NULL,
    [ReGenerate]              BIT            NOT NULL,
    [SelectShowDefaultOption] BIT            NOT NULL,
    [CSSClass]                VARCHAR (100)  NULL,
    [DynamicPropertyID]       INT            NOT NULL,
    [DecimalNumbers]          INT            CONSTRAINT [DF__ViewPrope__Decim__0EE3280B] DEFAULT ((0)) NULL,
    [DecimalRoundType]        INT            CONSTRAINT [DF__ViewPrope__Decim__0FD74C44] DEFAULT ((0)) NULL,
    [EnableToastMessage]      BIT            CONSTRAINT [DF__ViewPrope__Enabl__10CB707D] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [ID]                      VARCHAR (200)  NULL,
    [LabelCSS]                VARCHAR (100)  NULL,
    [Max]                     INT            CONSTRAINT [DF__ViewPropert__Max__11BF94B6] DEFAULT ((0)) NULL,
    [Min]                     INT            CONSTRAINT [DF__ViewPropert__Min__12B3B8EF] DEFAULT ((0)) NULL,
    [OnChange]                BIT            CONSTRAINT [DF__ViewPrope__OnCha__13A7DD28] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [ShowControl]             BIT            CONSTRAINT [DF__ViewPrope__ShowC__3138400F] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [ShowLabel]               BIT            CONSTRAINT [DF__ViewPrope__ShowL__30441BD6] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [StepResol]               VARCHAR (100)  NULL,
    [ToastMessage]            VARCHAR (500)  NULL,
    [RuleSet]                 NVARCHAR (MAX) NULL,
    [CSSCol]                  VARCHAR (200)  NULL,
    [Version]                 INT            CONSTRAINT [DF__ViewPrope__Versi__168449D3] DEFAULT ((0)) NOT NULL,
    [HasHeader]               BIT            NOT NULL,
    [ChangeBackground]        BIT            CONSTRAINT [DF__ViewPrope__Chang__36470DEF] DEFAULT ((0)) NOT NULL,
    [SelectOptions]           VARCHAR (500)  NULL,
    [FormulaProperty]         VARCHAR (500)  NULL,
    [ExtendedObject]          BIT            CONSTRAINT [DF__ViewPrope__Exten__1A9EF37A] DEFAULT ((0)) NOT NULL,
    [RowCSSCol]               VARCHAR (200)  NULL,
    [ColGroup]                VARCHAR (200)  NULL,
    [ColGroupTitle]           VARCHAR (200)  NULL,
    [ColGroupCSS]             VARCHAR (200)  NULL,
    [JSONConfiguration]       VARCHAR (2000) NULL,
    CONSTRAINT [PK_ViewPropertyBase] PRIMARY KEY CLUSTERED ([ViewPropertyID] ASC)
);



























