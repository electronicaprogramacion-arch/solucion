CREATE TABLE [dbo].[CalibrationSubTypeView] (
    [CalibrationSubTypeViewID] INT            NOT NULL,
    [CalibrationSubTypeId]     INT            NOT NULL,
    [EnabledDrag]              BIT            CONSTRAINT [DF_CalibrationSubTypeView_EnabledDrag] DEFAULT ((0)) NOT NULL,
    [EnabledDelete]            BIT            CONSTRAINT [DF_CalibrationSubTypeView_EnabledDelete] DEFAULT ((0)) NOT NULL,
    [EnabledDuplicate]         BIT            CONSTRAINT [DF_CalibrationSubTypeView_EnabledDuplicate] DEFAULT ((0)) NOT NULL,
    [EnabledSelect]            BIT            CONSTRAINT [DF__Calibrati__Enabl__7849DB76] DEFAULT ((0)) NOT NULL,
    [EnabledNew]               BIT            CONSTRAINT [DF__Calibrati__Enabl__793DFFAF] DEFAULT ((0)) NOT NULL,
    [PageSize]                 INT            NOT NULL,
    [CSSRow]                   VARCHAR (500)  NULL,
    [CSSRowSeparator]          VARCHAR (500)  NULL,
    [EnableButtonBar]          BIT            CONSTRAINT [DF__Calibrati__Enabl__7C1A6C5A] DEFAULT ((0)) NOT NULL,
    [ColButtonActionCSS]       VARCHAR (500)  NULL,
    [ColActionCSS]             VARCHAR (500)  NULL,
    [AlingActionCSS]           VARCHAR (500)  NULL,
    [Key]                      VARCHAR (500)  NULL,
    [NoDataMessage]            VARCHAR (500)  NULL,
    [CSSGrid]                  VARCHAR (500)  NULL,
    [SortField]                VARCHAR (500)  NULL,
    [BlockIfInvalid]           BIT            NULL,
    [FilterField]              VARCHAR (500)  NULL,
    [FilterValue]              VARCHAR (500)  NULL,
    [ShowHeader]               BIT            CONSTRAINT [DF_CalibrationSubTypeView_ShowHeader] DEFAULT ((0)) NOT NULL,
    [CSSForm]                  VARCHAR (500)  NULL,
    [CSSRowHeader]             VARCHAR (500)  NULL,
    [IsVisible]                BIT            NULL,
    [SaveButton]               BIT            NULL,
    [CloseButton]              BIT            NULL,
    [Style]                    VARCHAR (100)  NOT NULL,
    [Component]                VARCHAR (500)  NULL,
    [IsCollection]             BIT            CONSTRAINT [DF__Calibrati__IsCol__0BB1B5A5] DEFAULT ((1)) NOT NULL,
    [UseResult]                BIT            NULL,
    [ShowCardButton]           BIT            NOT NULL,
    [EnableEdit]               BIT            CONSTRAINT [DF_CalibrationSubTypeView_EnableEdit] DEFAULT ((0)) NOT NULL,
    [ColFormActionCSS]         VARCHAR (500)  NULL,
    [IsRowView]                BIT            CONSTRAINT [DF__Calibrati__IsRow__467D75B8] DEFAULT ((0)) NOT NULL,
    [NewButtonTitle]           VARCHAR (500)  NULL,
    [NewButtonCSS]             VARCHAR (500)  NULL,
    [EnableSelect2]            BIT            NULL,
    [SelectTitle]              VARCHAR (500)  NULL,
    [Select2Title]             VARCHAR (500)  NULL,
    [HasDeleteSubType]         BIT            CONSTRAINT [DF_CalibrationSubTypeView_HasDeleteSubType] DEFAULT ((0)) NOT NULL,
    [HasNewButton]             BIT            CONSTRAINT [DF_CalibrationSubTypeView_HasNewButton] DEFAULT ((0)) NOT NULL,
    [JSONConfiguration]        VARCHAR (5000) NULL,
    [CSVValidator]             VARCHAR (MAX)  NULL,
    [UseContext]               BIT            NULL,
    [CSSWidth]                 VARCHAR (1000) NULL,
    [DefaultDecimalNumber]     INT            NOT NULL,
    CONSTRAINT [PK_CalibrationSubTypeView] PRIMARY KEY CLUSTERED ([CalibrationSubTypeViewID] ASC)
);

















































