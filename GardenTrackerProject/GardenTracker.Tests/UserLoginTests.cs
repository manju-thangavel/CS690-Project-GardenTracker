using System;
using System.IO;
using Xunit;
using GardenTracker;

namespace GardenTracker.Tests
{
    public class UserLoginTests : IDisposable
    {
        private const string TestUserFilePath = "test_user_details_db.txt";
        private readonly TextReader _originalConsoleIn = Console.In;
        private readonly TextWriter _originalConsoleOut = Console.Out;

        public UserLoginTests()
        {
            if (File.Exists(TestUserFilePath))
            {
                File.Delete(TestUserFilePath);
            }
        }

        [Fact]
        public void Login_ShouldSucceed_WhenValidUserDetailsProvided()
        {
            File.WriteAllLines(TestUserFilePath, new string[] { "testuser", "testpassword" });
            var consoleInput = new StringReader("testuser\ntestpassword\n");
            Console.SetIn(consoleInput);

            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            var user = new UserWrapper(TestUserFilePath);

            bool loginResult = user.Login();

            Assert.True(loginResult, "Login should succeed with valid credentials.");
            Assert.Contains("Login successful!", consoleOutput.ToString());
        }

        [Fact]
        public void Login_ShouldFail_WhenInvalidUserDetailsProvided()
        {
            File.WriteAllLines(TestUserFilePath, new string[] { "testuser", "testpassword" });
            var consoleInput = new StringReader("wronguser\nwrongpassword\n");
            Console.SetIn(consoleInput);

            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            var user = new UserWrapper(TestUserFilePath);

            bool loginResult = user.Login();

            Assert.False(loginResult, "Login should fail with invalid credentials.");
            Assert.Contains("Invalid username or password.", consoleOutput.ToString());
        }

        [Fact]
        public void Register_ShouldCreateUserFile_WhenFileDoesNotExist()
        {
            var consoleInput = new StringReader("newuser\nnewpassword\n");
            Console.SetIn(consoleInput);

            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            var user = new UserWrapper(TestUserFilePath);

            bool loginResult = user.Login();

            Assert.False(loginResult, "Login should fail initially since the user file does not exist.");
            Assert.Contains("User not found. Please register.", consoleOutput.ToString());
            Assert.Contains("Registration success!", consoleOutput.ToString());
            Assert.True(File.Exists(TestUserFilePath), "User file should be created after registration.");

            string[] userDetails = File.ReadAllLines(TestUserFilePath);
            Assert.Equal("newuser", userDetails[0]);
            Assert.Equal("newpassword", userDetails[1]);
        }

        public void Dispose()
        {
            Console.SetIn(_originalConsoleIn);
            Console.SetOut(_originalConsoleOut);

            if (File.Exists(TestUserFilePath))
            {
                File.Delete(TestUserFilePath);
            }
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
