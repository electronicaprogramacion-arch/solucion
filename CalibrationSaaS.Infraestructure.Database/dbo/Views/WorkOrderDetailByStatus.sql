CREATE VIEW [dbo].[WorkOrderDetailByStatus]
AS
SELECT        dbo.WorkOrderDetail.WorkOrderDetailID, et.Manufacturer, et.Model, dbo.PieceOfEquipment.SerialNumber, et.EquipmentType, dbo.WorkOrderDetail.WorkOderID AS WorkOrderId, 
                         dbo.WorkOrder.WorkOrderDate AS WorkOrderReceiveDate, dbo.Status.Name AS Status, et.EquipmentTypeID, dbo.WorkOrderDetail.CurrentStatusID AS StatusId, dbo.WorkOrderDetail.StatusDate, '' AS Name, '' AS Description, 
                         dbo.Customer.Name AS Company
FROM            dbo.WorkOrderDetail INNER JOIN
                         dbo.PieceOfEquipment ON dbo.PieceOfEquipment.PieceOfEquipmentID = dbo.WorkOrderDetail.PieceOfEquipmentId INNER JOIN
                             (SELECT        PieceOfEquipment_1.PieceOfEquipmentID, dbo.EquipmentTemplate.Model, dbo.EquipmentType.Name AS EquipmentType, dbo.EquipmentTemplate.EquipmentTypeID, 
                                                         dbo.Manufacturer.Name AS Manufacturer
                               FROM            dbo.PieceOfEquipment AS PieceOfEquipment_1 INNER JOIN
                                                         dbo.EquipmentTemplate ON PieceOfEquipment_1.EquipmentTemplateId = dbo.EquipmentTemplate.EquipmentTemplateID INNER JOIN
                                                         dbo.EquipmentType ON dbo.EquipmentType.EquipmentTypeID = dbo.EquipmentTemplate.EquipmentTypeID INNER JOIN
                                                         dbo.Manufacturer ON dbo.Manufacturer.ManufacturerID = dbo.EquipmentTemplate.ManufacturerID) AS et ON et.PieceOfEquipmentID = dbo.WorkOrderDetail.PieceOfEquipmentId INNER JOIN
                         dbo.WorkOrder ON dbo.WorkOrder.WorkOrderId = dbo.WorkOrderDetail.WorkOderID INNER JOIN
                         dbo.Status ON dbo.Status.StatusId = dbo.WorkOrderDetail.CurrentStatusID INNER JOIN
                         dbo.Customer ON dbo.Customer.CustomerID = dbo.PieceOfEquipment.CustomerId
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[27] 4[17] 2[30] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = -910
      End
      Begin Tables = 
         Begin Table = "WorkOrderDetail"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 209
               Right = 272
            End
            DisplayFlags = 280
            TopColumn = 31
         End
         Begin Table = "PieceOfEquipment"
            Begin Extent = 
               Top = 260
               Left = 51
               Bottom = 390
               Right = 297
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "et"
            Begin Extent = 
               Top = 6
               Left = 521
               Bottom = 136
               Right = 720
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WorkOrder"
            Begin Extent = 
               Top = 6
               Left = 310
               Bottom = 210
               Right = 483
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Status"
            Begin Extent = 
               Top = 400
               Left = 431
               Bottom = 590
               Right = 619
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "Customer"
            Begin Extent = 
               Top = 138
               Left = 521
               Bottom = 268
               Right = 691
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'WorkOrderDetailByStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'WorkOrderDetailByStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'WorkOrderDetailByStatus';

