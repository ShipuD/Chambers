Feature: Document
	In Order to upload,download,delete, re-order pdf documents
	As Publisher 
	
	I want to place list documents to clients app.

@UploadPdfDoc
Scenario: Upload pdf document 1
    #Status 1-active and 0-inactive 3-deleted
	Given I have following PDF document to upload:
			| Name      | Description      | Order | Status | Location   |
			| Test1.pdf | test1 sample pdf | 1     | 1      | C://Test// |
		
	When I call send the Pdf to API
	Then the result message  should be :
		| Message               |
		| Uploaded Successfully |

@UploadPdfDoc2
Scenario: Upload pdf document 2
    #Status 1-active and 0-inactive 3-deleted
	Given I have following PDF document to upload:
			| Name      | Description      | Order | Status | Location   |
			| Test3.pdf | test3 sample pdf | 2     | 1      | C://Test// |
		
	When I call send the Pdf to API
	Then the result message  should be :
		| Message               |
		| Uploaded Successfully |

@UploadPdfDoc3
Scenario: Upload pdf document 3
    #Status 1-active and 0-inactive 3-deleted
	Given I have following PDF document to upload:
			| Name      | Description      | Order | Status | Location   |
			| Test4.pdf | test4 sample pdf | 3     | 1      | C://Test// |
		
	When I call send the Pdf to API
	Then the result message  should be :
		| Message               |
		| Uploaded Successfully |

@UploadExistingPdfDoc
Scenario: Upload pdf document that has been already uploaded
    #Status 1-active and 0-inactive 3-deleted
	Given I have following PDF document to upload:
			| Name      | Description      | Order | Status | Location   |
			| Test1.pdf | test1 sample pdf | 1     | 1      | C://Test// |
		
	When I call send the Pdf to API
	Then the result message  should be :
		| Message                 |
		| Document already exists |

@UploadNonPdfDoc
Scenario: Upload NonPdf document
    #Status 1-active and 0-inactive 3-deleted
	Given I have following Text document to upload:
			| Name      | Description       | Order | Status | Location   |
			| Test2.txt | test2 sample text | 4     | 1      | C://Test// |
		
	When I call send the Pdf to API
	Then the result message  should be :
		| Message            |
		| Not a pdf document |

@MaxPdfLengthOf5MBCheck
Scenario: Upload pdf document that is 5MB
    #Status 1-active and 0-inactive 3-deleted
	Given I have following PDF document to upload:
			| Name           | Description         | Order | Status | Location   |
			| Docwith5MB.pdf | sample pdf with 5MB | 5     | 1      | C://Test// |
		
	When I call send the Pdf to API
	Then the result message  should be :
		| Message                   |
		| Upload < 5MB pdf document |

@ListAlldocuments
Scenario: Get list of documents
	When I request get all docs
	Then the file list should be  :
		| Name      | Location | FileSize |
		| Test1.pdf | C:/Test/ | 78000    |
		| Test3.pdf | C:/Test/ | 78000    |
		| Test4.pdf | C:/Test/ | 78000    |

@DownloadPdfFromName
Scenario: Request location of Pdf from choosen List

Given I have following PDF document to download from list API:
			| Id | Name      | LocationToDownload  |
			| 1  | Test1.pdf | C:/Test/Downloaded/ |
		
	When I request the location for one of the PDF's
	Then the file should exist  :
		| Name      | LocationToDownload  |
		| Test1.pdf | C:/Test/Downloaded/ |

@DeleteExistingPdf
Scenario: Delete Pdf from choosen List thats no longer required

Given I have following PDF document to delete from list 
			| Id | Name      |
			| 2  | Test3.pdf | 
		
	When I request to delete PDF
	And  get all documents
	Then then the following file should not be in the list:
		| Name      | 
		| Test3.pdf | 

@DeleteNonExistingPdf
Scenario: Delete Pdf that does not exists at all

Given I have following PDF document to delete which not from list 
			| Id  | Name             |
			| 100 | DoesNotexist.pdf |
		
	When I request to delete non existant pdf file
	Then the result message  should be :
		| Message                        |
		| File does not exists to delete |

@ReorderExistingPdfList
Scenario: ReOrder existing pdf list

Given I have following PDF list with re order to be applied 
		| Id | Name      | Description      | Order | Status | Location   |
		| 1  | Test1.pdf | test3 sample pdf | 1     | 1      | C://Test// |
		| 3  | Test4.pdf | test3 sample pdf | 3     | 1      | C://Test// |
		
	When I request to re order
	Then the list should be have correct order as:
		| Id | Name      | Description      | Order | Status | Location   |
		| 1  | Test1.pdf | test3 sample pdf | 1     | 1      | C://Test// |
		| 3  | Test4.pdf | test3 sample pdf | 3     | 1      | C://Test// |
		