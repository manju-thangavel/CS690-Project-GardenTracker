using System;
using System.IO;
using Xunit;
using GardenTracker;

namespace GardenTracker.Tests
{
    public class UserLoginTests
    {
        private const string TestUserFilePath = "test_user_details_db.txt";

        [Fact]
        public void Login_ShouldSucceed_WhenValidUSerDetails()
        {
            File.WriteAllLines(TestUserFilePath, new string[] { "testuser", "testpassword" });
            var consoleInput = new StringReader("testuser\ntestpassword\n");
            Console.SetIn(consoleInput);

            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            var user = new UserWrapper(TestUserFilePath);

            bool loginResult = user.Login();

            Assert.True(loginResult);
            Assert.Contains("Login successful!", consoleOutput.ToString());

            File.Delete(TestUserFilePath);
        }

        [Fact]
        public void Login_ShouldFail_WhenInValidUSerDetails()
        {
            File.WriteAllLines(TestUserFilePath, new string[] { "testuser", "testpassword" });
            var consoleInput = new StringReader("wronguser\nwrongpassword\n");
            Console.SetIn(consoleInput);

            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            var user = new UserWrapper(TestUserFilePath);

            bool loginResult = user.Login();

            Assert.False(loginResult);
            Assert.Contains("Invalid username or password.", consoleOutput.ToString());

            File.Delete(TestUserFilePath);
        }

        [Fact]
        public void Register_ShouldCreateUserFile_WhenFileDoesNotExist()
        {
            if (File.Exists(TestUserFilePath))
            {
                File.Delete(TestUserFilePath);
            }

            var consoleInput = new StringReader("newuser\nnewpassword\n");
            Console.SetIn(consoleInput);

            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            var user = new UserWrapper(TestUserFilePath);

            bool loginResult = user.Login(); 

            Assert.False(loginResult); 
            Assert.Contains("User not found. Please register.", consoleOutput.ToString());
            Assert.Contains("Registration success! Please login now.", consoleOutput.ToString());
            Assert.True(File.Exists(TestUserFilePath));

            string[] userDetails = File.ReadAllLines(TestUserFilePath);
            Assert.Equal("newuser", userDetails[0]);
            Assert.Equal("newpassword", userDetails[1]);

            File.Delete(TestUserFilePath);
        }

        public class UserWrapper : User
        {
            private readonly string _testUserFilePath;

            public UserWrapper(string testUserFilePath)
            {
                _testUserFilePath = testUserFilePath;
            }
            protected override string UserFilePath => _testUserFilePath;
        }
    }
}