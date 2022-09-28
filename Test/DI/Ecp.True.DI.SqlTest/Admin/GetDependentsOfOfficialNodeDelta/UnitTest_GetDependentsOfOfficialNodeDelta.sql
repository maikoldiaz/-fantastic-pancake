--procedure to return dependent successor nodes for inpur deltanodeid

DECLARE
	@DeltaNodeID INT = 661

EXEC [Admin].[usp_GetDependentsOfOfficialNodeDelta] @DeltaNodeID

--Segment			NodeName			DeltaNodeId
--Automation_s9zcq	Automation_67079	664