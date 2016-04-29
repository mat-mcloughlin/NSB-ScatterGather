# Saga workflow and scatter gather example
## Instuctions
This sample includes both a v5 and v6 implementation

1. Download the sample.
2. Move the `codepo_gb.zip` to a suitable, temporary location.
3. Update the following line to reflect the location of the file and where you want it extracting to 
  https://github.com/mat-mcloughlin/NSB-ScatterGather/blob/master/NSBv6/Monitor/Program.cs#L44.
4.Choose either RavenDB or NHibernate by commenting out the relevant regions in the following file
  https://github.com/mat-mcloughlin/NSB-ScatterGather/blob/master/NSBv6/Monitor/Program.cs.
5. Set the solution to start up the `Monitor`, `Processor` and `Processor-2` projects
6. Run the application
  
## NHibernate specific instructions
1. Update the following line to your connection string
  https://github.com/mat-mcloughlin/NSB-ScatterGather/blob/master/NSBv6/Monitor/Program.cs.
2. If the sample doesn't run first time you may need to make a modification to the database to get around the fact that NHibernate maps a `string` as `nvarchar(255)`:
    - Open up SQL Management studio at the correct database.
    - Modify the `PackageHandlerData` table and change the data type of `FilesToProcess` to `varchar(MAX)`.
    - Re run the application.

## RavenDB specific instructions
This application uses the embedded RavenDB installation so you need to run Visual Studio with admin privledges
