/******************************************************************************
-- Type = Execute stored procedure by using the given paramters and load the data into [Admin].[Opertional]
-- INSERT --> Insert into [Admin].[Operational] table.
Scenario: Category  -------> Segement
           Element  -------> Automation_xdttv
		   NodeName -------> ALL
		   StartDate-------> '2019-12-13'
		   EndDate  -------> '2019-12-17'

Expectation: Stored procedure has to calculate input,output,intitalinventory,finalinvenetoyand unabalance based on given parameters
             "Segemento" , all nodes (Since Node name parameter value is "ALL")  and element, and load data into operational table
********************************************************************************/


EXEC  [Admin].[usp_SaveOperationalDataWithoutCutOffForSegment] 'Segmento','Automation_xdttv','ALL','2019-12-13','2019-12-17','738BCF0F-5F50-4EB8-A6C9-C27C99AA5A37'
