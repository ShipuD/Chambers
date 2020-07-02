# Chambers
All tests are completed.
Run the Test in order inside Documents Feature 

1) The data store used is SQL DB
2) Add migration using powershell(Package manager command) commands
       Add-Migration InitialCreate
	   Update-Database
	   
3)The above migration created ChambersDB DB with Scema using entity framework code first

4)SpecFlow for .netCore ,visula stuido 2019 is used for BDD 



6) On one Visual studio Run Chamber project a5 main usng IIS locally
7) On another Visual studio Run Chambers.BDDSpecFlowUnitTest test explorer

8) Document.Feature contains all test cases
9) Before you start testing :
   Create folder
		 a) C://Test 
         b) C:/Test/Downloaded/
10)AppJsonSettings update connection string but keep DB name as "ChambersDB"
		Make sure you can access DB
11) Add  some sample files  under C://Test folder for test
      Test1.pdf,
	  Test2.txt,(Text file)
	  Test3.pdf
	  Test4.pdf
	  Docwith5MB.pdf ( pdf which is more equal to 5MB)

12) Location field  stored in database is redundant as files are stored in as varbinary in the table
	  
		 
		  
