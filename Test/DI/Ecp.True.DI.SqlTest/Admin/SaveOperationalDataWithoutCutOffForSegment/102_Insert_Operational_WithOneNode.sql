/******************************************************************************
-- Type = Execute stored procedure by using the given paramters and load the data into [Admin].[Opertional]
-- INSERT --> Insert into [Admin].[Operational] table.
Scenario: Category  -------> Segement
           Element  -------> Automation_xdttv
		   NodeName -------> Automation_q6b3w_Genérico
		   StartDate-------> '2019-12-13'
		   EndDate  -------> '2019-12-17'

Expectation: Stored procedure has to calculate input,output,intitalinventory,finalinvenetoyand unabalance based on given parameters
             "Segemento" , given node and element and load into Operational table
********************************************************************************/


EXEC  [Admin].[usp_SaveOperationalDataWithoutCutOffForSegment] 'Segmento','Automation_xdttv','Automation_q6b3w_Genérico','2019-12-13','2019-12-17','738BCF0F-5F50-4EB8-A6C9-C27C99AA5A37'
