using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ClientSide_CaseStudy_2_Practise.Models;
using Microsoft.Data.Entity;
using Newtonsoft.Json;

namespace ClientSide_CaseStudy_2_Practise.APIs
{
    [Route("api/[controller]")]
    public class CompaniesController : Controller
    {
				//Every Web API controller class need to have an ApplicationDbContext
				//type property. I have declared one here, called Database.
				public ApplicationDbContext Database { get; }

        public CompaniesController()
        {
						//When the Web Application creates an object from
						//this Web API controller to handle a request, this
						//constructor will execute and the following line will
						//activate the Database property so that it can represent the 
						//actual database.
						Database = new ApplicationDbContext();
        }
        // GET: api/Companies
        [HttpGet]
        public JsonResult Get()
        {
            List<object> companyList = new List<object>();
            var companies = Database.Companies
                 .Where(eachCompanyEntity => eachCompanyEntity.DeletedAt == null).AsNoTracking();
								//After obtaining all the Company entity rows (records) from the database,
								//the companies variable will become an container holding these 
								//Company class entity rows.
								//I need to loop through each  Company instance inside companies
								//to construct a List container of anonymous objects (which has 5 properties).
								//Then use the new JsonResult(companyList) technique to generate the
								//JSON formatted string data which can be sent back to the web browser client.
            foreach (var company in companies)
            {
                companyList.Add(new
                {
                    CompanyId = company.CompanyId,
                    CompanyName = company.CompanyName,
                    Address = company.Address,
                    PostalCode = company.PostalCode,
                    CreatedAt = company.CreatedAt,
                    UpdatedAt = company.UpdatedAt
                });
            }//end of foreach loop which builds the companyList .
            return new JsonResult(companyList);
        }//end of Get() method

        // GET: api/Companies/GetCompaniesGroupByCompanyType
        [HttpGet("GetCompaniesGroupByCompanyType")]
        public JsonResult GetCompaniesGroupByCompanyType()
        {
            //Create a empty List, companyList to hold anonymous objects
            List<object> companyList = new List<object>();
            //The following command will create a nested List.
            //Which means, if there are thre 3 company types, there will be
            //three "sub-List" inside companyGroupQueryResult
            var companyGroupQueryResult = Database.Companies
                 .Where(eachCompany => eachCompany.DeletedAt == null)
                 .Include(eachCompany => eachCompany.CompanyType)
                 .OrderBy(eachCompany => eachCompany.CompanyName)
                 .GroupBy(eachCompany => eachCompany.CompanyTypeId);
            //The following nested foreach loop aims to combine all the
            //company information from each sub-List of company data
            //into a single List. The companyList is used to consolidate
            //all the company data.
            foreach (var companyGroup in companyGroupQueryResult)
            {
                foreach (var eachCompany in companyGroup)
                {
                    companyList.Add(new
                    {
                        CompanyId = eachCompany.CompanyId,
                        CompanyName = eachCompany.CompanyName,
                        CompanyTypeName = eachCompany.CompanyType.TypeName
                    });
                }//end of the inner foreach block
            }//end of outer foreach block
            return new JsonResult(companyList);
        }//End of GetCompaniesGroupByCompanyType()

        [HttpGet("GetCompaniesWithCompanyType")]
        public JsonResult GetCompaniesWithCompanyType()
        {
            List<object> companyList = new List<object>();
            var companies = Database.Companies
                 .Where(eachCompany => eachCompany.DeletedAt == null)
                 .Include(eachCompany => eachCompany.CompanyType)
                 .OrderBy(eachCompany => eachCompany.CompanyName);
            //After obtaining all the Company entity rows (records) from the database,
            //the companies variable will become a container holding these Company entities.
            //I need to loop through each Company instance inside companies
            //to build a companyList which contains anonymous type objects .
            foreach (var oneCompany in companies)
            {
                companyList.Add(new
                {
										CompanyId = oneCompany.CompanyId,
										CompanyName = oneCompany.CompanyName,
										Address = oneCompany.Address,
										PostalCode = oneCompany.PostalCode,
										CompanyTypeName = oneCompany.CompanyType.TypeName
                });
            }//end of foreach loop which builds the companyList .
            return new JsonResult(companyList);
        }//End of GetCompaniesWithCompanyType()


		    // GET api/Companies/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try {
                var foundCompany = Database.Companies
                     .Where(item => item.CompanyId == id).Single();
                var response = new
                {
										CompanyId = foundCompany.CompanyId,
										CompanyName = foundCompany.CompanyName,
										Address = foundCompany.Address,
										PostalCode = foundCompany.PostalCode,
										CompanyTypeId = foundCompany.CompanyTypeId,
										CreatedAt = foundCompany.CreatedAt,
										UpdatedAt = foundCompany.UpdatedAt
                };//end of creation of the response object
                return new JsonResult(response);
            } catch (Exception exceptionObject)
            {
                //Create a fail message anonymous object
                //This anonymous object only has one Message property 
                //which contains a simple string message
                object httpFailRequestResultMessage = 
					                new { Message = "Unable to obtain company information." };
                //Return a bad http response message to the client
                return HttpBadRequest(httpFailRequestResultMessage);
            }
        }//End of Get(id) Web API method

				// POST api/Companies
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
			      string customMessage = "";
            //Reconstruct a useful object from the input string value. 
            dynamic companyNewInput = JsonConvert.DeserializeObject<dynamic>(value);
            Company newCompany = new Company();
            try
            {
				        //Copy out all the company data into the new Company instance,
				        //newCompany.
				        newCompany.CompanyName = companyNewInput.CompanyName.Value;
				        newCompany.Address = companyNewInput.Address.Value;
				        newCompany.PostalCode = companyNewInput.PostalCode.Value;
				        newCompany.CompanyTypeId = Int32.Parse(companyNewInput.CompanyTypeId.Value);
				        Database.Companies.Add(newCompany);
				        Database.SaveChanges();//Telling the database model to save the changes
			}
			catch (Exception exceptionObject)
            {
				          if (exceptionObject.InnerException.Message
									.Contains("Company_CompanyName_UniqueConstraint") == true)
				          {
					           customMessage = "Unable to save company record due " +
					                         "to another record having the same name as : " +
					                         companyNewInput.CompanyName.Value;
					//Create a fail message anonymous object that has one property, Message.
					//This anonymous object's Message property contains a simple string message
					object httpFailRequestResultMessage = new { Message = customMessage };
					//Return a bad http request message to the client
					return HttpBadRequest(httpFailRequestResultMessage);
				}
			}//End of Try..Catch block

			//If there is no runtime error in the try catch block, the code execution
			//should reach here. Sending success message back to the client.

			//******************************************************
			//Construct a custom message for the client
			//Create a success message anonymous object which has a 
			//Message member variable (property)
			var successRequestResultMessage = new
			{
				Message = "Saved company record"
			};

			//Create a HttpOkObjectResult class instance, httpOkResult.
			//When creating the object, provide the previous message object into it.
			HttpOkObjectResult httpOkResult =
						new HttpOkObjectResult(successRequestResultMessage);
			//Send the HttpOkObjectResult class object back to the client.
			return httpOkResult;

		}//End of Post() method
 // PUT api/Companies/5
[HttpPut("{id}")]
public IActionResult Put(int id, [FromBody]string value)
{
    string customMessage = "";
    var companyChangeInput = JsonConvert.DeserializeObject<dynamic>(value);
			//To obtain the company name information:
			//use companyChangeInput.CompanyName.value
			//To obtain the address information:
			//use companyChangeInput.Address.value
			try
			{
				//Find the Company Entity through the Companies Entity Set
				//by calling the Single() method.
				//I learnt Single() method from this online reference:
				//http://geekswithblogs.net/BlackRabbitCoder/archive/2011/04/14/c.net-little-wonders-first-and-single---similar-yet-different.aspx
	var foundOneCompany = Database.Companies
                        .Single(item => item.CompanyId == id);
						foundOneCompany.CompanyName = companyChangeInput.CompanyName;
						foundOneCompany.Address = companyChangeInput.Address.Value;
						foundOneCompany.PostalCode = companyChangeInput.PostalCode.Value;
						foundOneCompany.CompanyTypeId = Int32.Parse(companyChangeInput.CompanyTypeId.Value);
						foundOneCompany.UpdatedAt = DateTime.Now;

            //Tell the database model to commit/persist the changes to the database, 
            //I use the following command.
            Database.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message
					 .Contains("Company_CompanyName_UniqueConstraint") == true)
                {
                    customMessage = "Unable to save company record due " +
						              "to another record having the same name as : " +
                    companyChangeInput.CompanyName.Value;
                    //Create a fail message anonymous object that has one property, Message.
                    //This anonymous object's Message property contains a simple string message
                    object httpFailRequestResultMessage = new { Message = customMessage };
                    //Return a bad http request message to the client
                    return HttpBadRequest(httpFailRequestResultMessage);
                }
            }//End of try .. catch block on saving data
					 //Construct a custom message for the client
					 //Create a success message anonymous object which has a 
			     //Message member variable (property)
            var successRequestResultMessage = new
            {
                Message = "Saved company record"
            };

            //Create a HttpOkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            HttpOkObjectResult httpOkResult = 
				   new HttpOkObjectResult(successRequestResultMessage);
            //Send the HttpOkObjectResult class object back to the client.
            return httpOkResult;
        }//End of Put() Web API method      (// PUT api/Companies/5)

				// DELETE api/Companies/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string customMessage = "";

            //The following command should not be used. Although can work too.
            // var existingOneCompany = Database.Companies
            //    .Where(item => item.CompanyId == id).FirstOrDefault();
            //--------------------------------------------------------------------------------------------
            try
            {
                var foundOneCompany = Database.Companies
                           .Single(item => item.CompanyId == id);
                foundOneCompany.DeletedAt = DateTime.Now;

                //Update the database model
                Database.Update(foundOneCompany);
                //Tell the db model to commit/persist the changes to the database, 
                //I use the following command.
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                customMessage = "Unable to delete company record.";
                object httpFailRequestResultMessage = new { Message = customMessage };
                //Return a bad http request message to the client
                return HttpBadRequest(httpFailRequestResultMessage);
            }//End of try .. catch block on manage data

			//Build a custom message for the client
			//Create a success message anonymous object which has a 
			//Message member variable (property)
            var successRequestResultMessage = new
            {
                Message = "Deleted company record"
            };
            
						//Create a HttpOkObjectResult class instance, httpOkResult.
						//When creating the object, provide the previous message object into it.
            HttpOkObjectResult httpOkResult = 
				                            new HttpOkObjectResult(successRequestResultMessage);
            //Send the HttpOkObjectResult class object back to the client.
            return httpOkResult;
        }//end of Delete() Web API method with /apis/values/digit route

        // DELETE api/bulk/5,6,2,1
        [HttpDelete("bulk")]
        public async Task<IActionResult> Delete([FromQuery]string value)
        {

            string customMessage = "";
            var listOfId = value.Split(',').ToList();

            try
            {
                var companyList = Database.Companies.Where(x => listOfId.Contains(x.CompanyId.ToString()));
                await companyList.ForEachAsync(a => a.DeletedAt = DateTime.Now);

                //Update the database model
                Database.UpdateRange(companyList);
                //Tell the db model to commit/persist the changes to the database, 
                //I use the following command.
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                customMessage = "Unable to delete company record(s).";
                object httpFailRequestResultMessage = new { Message = customMessage };
                //Return a bad http request message to the client
                return HttpBadRequest(httpFailRequestResultMessage);
            }//End of try .. catch block on saving data
            //Construct a custom message for the client
            //Create a success message anonymous object which has a Message member variable (property)
            var successRequestResultMessage = new
            {
                Message = "Deleted company record(s)"
            };
            //Create a HttpOkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            HttpOkObjectResult httpOkResult = new HttpOkObjectResult(successRequestResultMessage);
            //Send the HttpOkObjectResult class object back to the client.
            return httpOkResult;
        }//End of bulk delete method

    }//End of Web API class definition

}//End of name space
