using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Chambers.Models;
using NUnit.Framework;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Bson;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

namespace Chambers.BDDSpecFlownUnitTest
{
    [Binding]
    public class DocumentSteps
    {
        public readonly Document documents;
        public readonly DocumentContext documentContext;
        public readonly HttpClient httpClient;
        public string baseWebApiUrl = "https://localhost:44378/";

        public DocumentSteps()
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseWebApiUrl)
            };
        }

        #region Upload Pdf
        [Given(@"I have following PDF document to upload:")]
        public void GivenIHaveFollowingPDFDocumentToUpload(Table table)
        {
            Document document = table.CreateInstance<Document>();
            string pdfFilePath = document.Location + document.Name;
            byte[] bytes = System.IO.File.ReadAllBytes(pdfFilePath);
            //string s = System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            document.Data = bytes;
            ScenarioContext.Current.Add("Doc",document);

        }

        [When(@"I call send the Pdf to API")]
        public async Task WhenICallSendThePdfToAPI()
        {
            var document = ScenarioContext.Current.Get<Document>("Doc");
            Error retError = new Error { Status = ErrorStatus.UnknownError, Message = UserMessages.UnknowError };

            string jSonData = JsonConvert.SerializeObject(document);
            HttpContent content = new StringContent(jSonData, Encoding.UTF8,
                                            "application/json");

            HttpResponseMessage response = await httpClient.PostAsync("api/Documents", content);
          
            string Result = await response.Content.ReadAsStringAsync();
            retError = JsonConvert.DeserializeObject<Error>(Result);

            ScenarioContext.Current.Add("returnMessage", retError.Message);
        }

        [Then(@"the result message  should be :")]
        public void ThenTheResultMessageShouldBe(Table table)
        {
            var expectedMessage = table.Rows[0]["Message"];
            var actualMessage = ScenarioContext.Current.Get<string>("returnMessage");
            Assert.AreEqual(expectedMessage, actualMessage);
        }
        #endregion Upload Pdf


        #region Upload NonPdf file
        [Given(@"I have following Text document to upload:")]
        public void GivenIHaveFollowingTextDocumentToUpload(Table table)
        {
            GivenIHaveFollowingPDFDocumentToUpload(table);
        }
        #endregion Upload Text file


        #region GetAll List
        [When(@"I request get all docs")]
        public async Task WhenIRequestGetAllDocs()
        {
            List<GetDocumentObj> docs;           
            Error retError = new Error { Status = ErrorStatus.UnknownError, Message = UserMessages.UnknowError };

            HttpResponseMessage response = await httpClient.GetAsync("api/Documents");

            string Result = await response.Content.ReadAsStringAsync();
            docs = JsonConvert.DeserializeObject<List<GetDocumentObj>>(Result);

            ScenarioContext.Current.Add("docs", docs);
        }

        [Then(@"the file list should be  :")]
        public void ThenTheFileListShouldBe(Table expectedList)
        {
            var actualDoclist = ScenarioContext.Current.Get<List<GetDocumentObj>>("docs");
            //Check if the list has same no of items.
            //TODO check each item in the list to eb exact
             Assert.AreEqual(actualDoclist.Count,expectedList.RowCount);
        }
        #endregion GetAll List
        #region Download pdf
        [Given(@"I have following PDF document to download from list API:")]
        public void GivenIHaveFollowingPDFDocumentToDownloadFromListAPI(Table table)
        {
            Document document = table.CreateInstance<Document>();
            ScenarioContext.Current.Add("Doc", document);
        }

        [When(@"I request the location for one of the PDF's")]
        public async Task WhenIRequestTheLocationForOneOfThePDFS()
        {
            var document = ScenarioContext.Current.Get<Document>("Doc");
            HttpResponseMessage response = await httpClient.GetAsync("api/Documents/"+document.Id.ToString());

            string Result = await response.Content.ReadAsStringAsync();
            Document doc = JsonConvert.DeserializeObject<Document>(Result);

            ScenarioContext.Current.Add("actualDoc", doc);

        }

        [Then(@"the file should exist  :")]
        public void ThenTheFileShouldExist(Table table)
        {
            string locationToDownload = table.Rows[0]["LocationToDownload"];
            var actualDoc = ScenarioContext.Current.Get<Document>("actualDoc");            
            File.WriteAllBytes(locationToDownload + "\\" +actualDoc.Name, actualDoc.Data);
            //Check for Same and file exists
            Assert.AreEqual(actualDoc.Name, table.Rows[0]["Name"]); 
            Assert.IsTrue(File.Exists(locationToDownload + "\\" + actualDoc.Name));
        }
        #endregion Download pdf

        #region Delete pdf
        [Given(@"I have following PDF document to delete from list")]
        public void GivenIHaveFollowingPDFDocumentToDeleteFromList(Table table)
        {
            Document docToDelete = table.CreateInstance<Document>();
            ScenarioContext.Current.Add("docToDelete", docToDelete);
        }

        [When(@"I request to delete PDF")]
        public async Task WhenIRequestToDeletePDF()
        {
            var docToDelete = ScenarioContext.Current.Get<Document>("docToDelete");
            HttpResponseMessage response = await httpClient.DeleteAsync("api/Documents/" + docToDelete.Id.ToString());

        }

        [When(@"get all documents")]
        public async Task WhenGetAllDocuments()
        {
            List<GetDocumentObj> docs;
            Error retError = new Error { Status = ErrorStatus.UnknownError, Message = UserMessages.UnknowError };

            HttpResponseMessage response = await httpClient.GetAsync("api/Documents");

            string Result = await response.Content.ReadAsStringAsync();
            docs = JsonConvert.DeserializeObject<List<GetDocumentObj>>(Result);

            ScenarioContext.Current.Add("docs", docs);
        }

        [Then(@"then the following file should not be in the list:")]
        public void ThenThenTheFollowingFileShouldNotBeInTheList(Table table)
        {
            var fileShouldNotExist = table.Rows[0]["Name"];
            var actualDoclist = ScenarioContext.Current.Get<List<GetDocumentObj>>("docs");
            //Check if the list has same no of items.
            //TODO check each item in the list to eb exact
            Assert.IsFalse(actualDoclist.Exists(r => r.Name == fileShouldNotExist));
        }

        #endregion Delete pdf

        #region delete non-existant pdf
        [Given(@"I have following PDF document to delete which not from list")]
        public void GivenIHaveFollowingPDFDocumentToDeleteWhichNotFromList(Table table)
        {
            Document docToDelete = table.CreateInstance<Document>();
            ScenarioContext.Current.Add("docToDelete", docToDelete);
        }

        [When(@"I request to delete non existant pdf file")]
        public async Task WhenIRequestToDeleteNonExistantPdfFile()
        {
            Error retError = new Error { Status = ErrorStatus.UnknownError, Message = UserMessages.UnknowError };
            var docToDelete = ScenarioContext.Current.Get<Document>("docToDelete");
            HttpResponseMessage response = await httpClient.DeleteAsync("api/Documents/" + docToDelete.Id.ToString());            
            string Result = await response.Content.ReadAsStringAsync();
            retError = JsonConvert.DeserializeObject<Error>(Result);

            ScenarioContext.Current.Add("returnMessage", retError.Message);
        }

        #endregion

        #region re order pdf documents
        [Given(@"I have following PDF list with re order to be applied")]
        public void GivenIHaveFollowingPDFListWithReOrderToBeApplied(Table table)
        {
            IEnumerable<Document> givenOrderedDocList = table.CreateSet<Document>();

            ScenarioContext.Current.Add("givenOrderedDocList", givenOrderedDocList);
        }


        [When(@"I request to re order")]
        public async Task WhenIRequestToReOrder()
        {
            List<Document> orderedDocList;
            Error retError = new Error { Status = ErrorStatus.UnknownError, Message = UserMessages.UnknowError };

            IEnumerable<Document> givenOrderedDocList = ScenarioContext.Current.Get<IEnumerable<Document>>("givenOrderedDocList");
            HttpResponseMessage response = null;
            foreach (var doc in givenOrderedDocList)
            {
                response = await httpClient.GetAsync("api/Documents/" + doc.Id.ToString());

                string Result = await response.Content.ReadAsStringAsync();
                Document dbDocument = JsonConvert.DeserializeObject<Document>(Result);
                //Updat the Order field given bu user
                dbDocument.Order = doc.Order;                
                var content = new StringContent(JsonConvert.SerializeObject(dbDocument), Encoding.UTF8, "application/json");
                response = await httpClient.PutAsync("api/Documents/" + doc.Id.ToString(), content);

            }
        }

        [Then(@"the list should be have correct order as:")]
        public async Task ThenTheListShouldBeHaveCorrectOrderAs(Table table)
        {
            IEnumerable<Document> expectedOrderedDocList = table.CreateSet<Document>();
            HttpResponseMessage response = null;
            foreach (var doc in expectedOrderedDocList)
            {
               //get the document form Db
                response = await httpClient.GetAsync("api/Documents/" + doc.Id.ToString());

                string Result = await response.Content.ReadAsStringAsync();
                Document dbDocument = JsonConvert.DeserializeObject<Document>(Result);
                //Check if the Db is has been updated with expected value of Order
                Assert.IsTrue(dbDocument.Order == doc.Order);
            }
        }


        #endregion
    }
}
