To scaffold configuration and entities, open the package manager console and run the below command:-
Scaffold-DbContext '<connstr>' Microsoft.EntityFrameworkCore.SqlServer -Context AppContext -ContextDir Context -OutputDir Entities -Force

After running the above command, entites and context would be regenerated.
Plesae esnusre you delete onConfiguring method in AppContext.cs as it will contain the connection string.