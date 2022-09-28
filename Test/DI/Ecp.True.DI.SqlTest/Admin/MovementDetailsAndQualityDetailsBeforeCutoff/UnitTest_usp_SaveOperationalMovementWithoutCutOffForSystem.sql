/******************************************************************************
-- Type = Execute stored procedure by using the given paramters and load the data into [Admin].[OpertionalMovement]
-- INSERT --> Insert into [Admin].[OpertionalMovement] table.
Scenario: Category  -------> Sistema
           Element  -------> Automation_tirme_System
		   NodeName -------> Automation_ksp7o
		   StartDate-------> '2020-02-01'
		   EndDate  -------> '2020-02-04'

Expectation: Stored procedure has to show Movement deatils based on given parameters
             "Sistema" , Nodename  and element, and load data into operational table
********************************************************************************/


EXEC [Admin].[usp_SaveOperationalMovementWithoutCutOffForSegment] 'Sistema','Automation_tirme_System','Automation_ksp7o', '2020-02-01'  , '2020-02-04','738BCF0F-5F50-4EB8-A6C9-C27C99AA5A00'
