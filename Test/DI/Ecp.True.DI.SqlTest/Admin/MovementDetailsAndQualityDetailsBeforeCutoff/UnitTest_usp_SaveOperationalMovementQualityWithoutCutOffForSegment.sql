/******************************************************************************
-- Type = Execute stored procedure by using the given paramters and load the data into [Admin].[OpertionalMovementQuality]
-- INSERT --> Insert into [Admin].[OpertionalMovementQuality] table.
Scenario: Category  -------> Segmento
           Element  -------> Automation_1l697
		   NodeName -------> Automation_retqt
		   StartDate-------> '2020-01-30'
		   EndDate  -------> '2020-02-03'

Expectation: Stored procedure has to show Movement quality deatils based on given parameters
             "Segemento" , all nodes (Since Node name parameter value is "ALL")  and element, and load data into operational table
********************************************************************************/


EXEC [Admin].[usp_SaveOperationalMovementQualityWithoutCutOffForSegment] 'Segmento','Automation_1l697','Automation_retqt', '2020-01-30'  , '2020-02-03','738BCF0F-5F50-4EB8-A6C9-C27C99AA5A00'
