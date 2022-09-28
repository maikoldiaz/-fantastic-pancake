INSERT INTO admin.OwnershipNode (Ticketid, NodeId, Status, OwnershipStatusId, CreatedBy)
VALUES (159,30841, 0, 2, 'trueadmin')

INSERT INTO admin.OwnershipNode (Ticketid, NodeId, Status, OwnershipStatusId, CreatedBy)
VALUES (185,30841, 0, 2, 'trueadmin')

update  admin.Node
set SendToSAP=1
where nodeid=30841

EXEC Admin.[usp_GetLogisticNodeValidation] 10
										   ,'2020-04-08'
										   ,'2020-04-15'
										   ,30841



--NodeId	NodeName	OperationDate	NodeStatus
--30841		MS NODO 0	2020-04-15		Propiedad
--30841		MS NODO 0	2020-04-14		Propiedad
--30841		MS NODO 0	2020-04-13		Propiedad
--30841		MS NODO 0	2020-04-12		Propiedad
--30841		MS NODO 0	2020-04-11		Propiedad
--30841		MS NODO 0	2020-04-10		Propiedad
--30841		MS NODO 0	2020-04-09		Propiedad
--30841		MS NODO 0	2020-04-08		Propiedad