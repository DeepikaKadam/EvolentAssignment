using Contact.Api.Repository.Helper;
using Contact.Repository;
using ContactManagementApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Contact.Api.UnitTest
{
    public class ContactApiTest
    {
        private readonly ContactRepository contactRepository;
        private DbContextOptions<DatabaseContext> DbContextOptions { get; }
        private readonly string connectionString = "Server=tcp:evolent-assignment.database.windows.net,1433;Initial Catalog=EvolentAssignmentDb;Persist Security Info=False;User ID=evolent;Password=Evol@2020;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public ContactApiTest()
        {
            DbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
               .UseSqlServer(connectionString)
               .Options;

            var context = new DatabaseContext(DbContextOptions);

            contactRepository = new ContactRepository(context);

        }

        [Fact]
        public async void GetAllContacts_Test_Ok()
        {
            var contactController = new ContactController(contactRepository);
            var data = await contactController.Get();

            Assert.IsType<OkObjectResult>(data);
        }


        [Fact]
        public async void AddContact_Test_Ok()
        {
            var contactController = new ContactController(contactRepository);
            var data = await contactController.Add(new Models.Contact()
            { FirstName = "Deepika", LastName = "K", EmailId = "dr@j.com", PhoneNumber = "54354-343" }, 11);

            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void AddContact_Test_NoFirstName_Fail()
        {
            var contactController = new ContactController(contactRepository);
            var data = await contactController.Add(new Models.Contact()
            { FirstName = "", LastName = "K", EmailId = "dr@j.com", PhoneNumber = "54354-343" }, 11);

            Assert.IsType<ObjectResult>(data);
        }
    }
}
