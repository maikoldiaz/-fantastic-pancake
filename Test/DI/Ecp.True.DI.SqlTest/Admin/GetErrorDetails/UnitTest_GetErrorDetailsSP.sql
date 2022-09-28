
-- 1. ******************************************************************************
-- GetErrorDetailsSP - This SP gets the PendingTransactionError details based on the given ErrorId.
--	Input to SP -> ErrorId
------------------------------------------------------------------------------------



DECLARE @TransactionId INT;


DECLARE @ErrorId1 INT;
DECLARE @ErrorId2 INT;

INSERT [Admin].[PendingTransaction] ([MessageTypeId], [BlobName], [MessageId], [ErrorJson], [SourceNodeId], [DestinationNodeId], [SourceProductId], [DestinationProductId], [ActionTypeId], [Volume], [Units], [StartDate], [EndDate], [TicketId], [SystemTypeId], [SystemName], [OwnerId], [SegmentId], [TypeId], [Identifier], [CreatedBy]) VALUES (4, N'/true/sinoper/xml/inventory/QU1RIEVBSTAyLkQuUU0gIF1kCeIhZpQQ', N'QU1RIEVBSTAyLkQuUU0gIF1kCeIhZpQQ', N'{"ErrorInfo":[{"Code":"4020","Message":"El identificador del inventario ya existe en el sistema​"}],"IsSuccess":false}', 83, 84, '10000002372', '10000002372', 1, 426871.82, 31, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'System')
INSERT [Admin].[PendingTransaction] ([MessageTypeId], [BlobName], [MessageId], [ErrorJson], [SourceNodeId], [DestinationNodeId], [SourceProductId], [DestinationProductId], [ActionTypeId], [Volume], [Units], [StartDate], [EndDate], [TicketId], [SystemTypeId], [SystemName], [OwnerId], [SegmentId], [TypeId], [Identifier], [CreatedBy]) VALUES (4, N'/true/sinoper/json/inventory/intnew1.json', N't67c57b6-nunu-426e-912a-22707fb75r55', N'{"ErrorInfo":[{"Code":"4025","Message":"La fecha del inventario debe ser menor a la fecha actual"}],"IsSuccess":false}', 83, 84, '10000002372', '10000002372', 1, 12345.69, 31, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'System')

SELECT @TransactionId = SCOPE_IDENTITY();
INSERT [Admin].[PendingTransactionError] ([TransactionId], [ErrorMessage], [Comment], [CreatedBy]) VALUES (@TransactionId, N'No existe una conexión para los nodos origen y destino recibidos', N'a', N'System')
SELECT @ErrorId1 = SCOPE_IDENTITY();

INSERT [Admin].[PendingTransaction] ([MessageTypeId], [BlobName], [MessageId], [ErrorJson], [SourceNodeId], [DestinationNodeId], [SourceProductId], [DestinationProductId], [ActionTypeId], [Volume], [Units], [StartDate], [EndDate], [TicketId], [SystemTypeId], [SystemName], [OwnerId], [SegmentId], [TypeId], [Identifier], [CreatedBy]) VALUES (4, N'/true/sinoper/json/inventory/intnew1.json', N't67c57b6-nunu-426e-912a-22707fb75r55', N'{"ErrorInfo":[{"Code":"4025","Message":"La fecha del inventario debe ser menor a la fecha actual"}],"IsSuccess":false}', 84, 83, '10000002318', '10000002318', 2, 636571.82, 31, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'System')

SELECT @TransactionId = SCOPE_IDENTITY();
INSERT [Admin].[PendingTransactionError] ([TransactionId], [ErrorMessage], [Comment], [CreatedBy]) VALUES (@TransactionId, N'Debe ser de tipo: ProductTypeId', N'a', N'System')
SELECT @ErrorId2 = SCOPE_IDENTITY();

INSERT [Admin].[PendingTransaction] ([MessageTypeId], [BlobName], [MessageId], [ErrorJson], [SourceNodeId], [DestinationNodeId], [SourceProductId], [DestinationProductId], [ActionTypeId], [Volume], [Units], [StartDate], [EndDate], [TicketId], [SystemTypeId], [SystemName], [OwnerId], [SegmentId], [TypeId], [Identifier], [CreatedBy]) VALUES (4, N'/true/sinoper/json/inventory/intnew1.json', N't67c57b6-nunu-426e-912a-22707fb75r55', N'{"ErrorInfo":[{"Code":"4026","Message":"No es posible registrar un inventario de meses anteriores"}],"IsSuccess":false}', 84, 83, '10000002318', '10000002318', 2, 8745.25, 31, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'System')



EXEC [Admin].[usp_GetErrorDetails] @ErrorId1
EXEC [Admin].[usp_GetErrorDetails] @ErrorId2



