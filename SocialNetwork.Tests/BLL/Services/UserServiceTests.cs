using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.Services.Tests
{
    [TestClass()]
    public class UserServiceTests
    {
        UserService userService;

        static string userTestFirstName = "Ivan";
        static string userTestLastName = "Ivanov";
        static string userTestPassword = "12345678";
        static string userTestEmail = "Ivan@gmail.com";

        [TestMethod("Регистрация при пароле короче 8 символов")]
        public void Register_ShortPassword_MustThrowArgumentNullException()
        {
            var userRegistrationData = new UserRegistrationData()
            {
                FirstName = userTestFirstName,
                LastName = userTestLastName,
                Password = "12345",
                Email = userTestEmail
            };
            userService = new UserService(new UserRepository());
            Assert.ThrowsException<ArgumentNullException>(() => userService.Register(userRegistrationData));
        }
        [TestMethod("Регистрация при некорректной почте")]
        public void Register_IncorrectEmail_MustThrowArgumentNullException()
        {
            var userRegistrationData = new UserRegistrationData()
            {
                FirstName = userTestFirstName,
                LastName = userTestLastName,
                Password = userTestPassword,
                Email = "Ivangmail.com"
            };
            userService = new UserService(new UserRepository());
            Assert.ThrowsException<ArgumentNullException>(() => userService.Register(userRegistrationData));
        }

        [TestMethod("Поиск по email при корректном значении")]
        public void FindByEmail_CorrectEmail_MustReturnUser()
        {
            var findUserEntity = new UserEntity()
            {
                firstname = userTestFirstName,
                lastname = userTestLastName,
                password = userTestPassword,
                email = userTestEmail
            };
            var mockUserService = new Mock<IUserRepository>();
            mockUserService.Setup(p => p.FindByEmail(userTestEmail)).Returns(findUserEntity);
            userService = new UserService(mockUserService.Object);
            var findByEmailTest = userService.FindByEmail(userTestEmail);
            Assert.AreEqual(findByEmailTest.Email, findUserEntity.email);
            Assert.IsNotNull(userService.FindByEmail(userTestEmail));
        }

        [TestMethod("Поиск по email при пустом значении")]
        public void FindByEmail_EmptyEmail_MustThrowUserNotFoundException()
        {
            userService = new UserService(new UserRepository());
            Assert.ThrowsException<UserNotFoundException>(() => userService.FindByEmail(""));
        }

        [TestMethod("Аутентификация при корректных данных")]
        public void Authenticate_CorrectData_MustReturnUser()
        {
            var userAuthenticationData = new UserAuthenticationData()
            {
                Email = userTestEmail,
                Password = userTestPassword
            };
            var findUserEntity = new UserEntity()
            {
                firstname = userTestFirstName,
                lastname = userTestLastName,
                password = userTestPassword,
                email = userTestEmail
            };
            var mockUserService = new Mock<IUserRepository>();
            mockUserService.Setup(p => p.FindByEmail(userTestEmail)).Returns(findUserEntity);
            userService = new UserService(mockUserService.Object);
            var authenticateTest = userService.Authenticate(userAuthenticationData);
            Assert.AreEqual(authenticateTest.Email, findUserEntity.email);
        }

        [TestMethod("Аутентификация при некорректной почте")]
        public void Authenticate_IncorrectEmail_MustThrowUserNotFoundException()
        {
            var userAuthenticationData = new UserAuthenticationData()
            {
                Email = "Oleg@gmail.com",
                Password = userTestPassword
            };
            var findUserEntity = new UserEntity()
            {
                firstname = userTestFirstName,
                lastname = userTestLastName,
                password = userTestPassword,
                email = userTestEmail
            };
            var mockUserService = new Mock<IUserRepository>();
            mockUserService.Setup(p => p.FindByEmail(userTestEmail)).Returns(findUserEntity);
            userService = new UserService(mockUserService.Object);
            Assert.ThrowsException<UserNotFoundException>(() => userService.Authenticate(userAuthenticationData));
        }

        [TestMethod("Аутентификация при некорректном пароле")]
        public void Authenticate_IncorrectPassword_MustThrowWrongPasswordException()
        {
            var userAuthenticationData = new UserAuthenticationData()
            {
                Email = userTestEmail,
                Password = "123"
            };
            var findUserEntity = new UserEntity()
            {
                firstname = userTestFirstName,
                lastname = userTestLastName,
                password = userTestPassword,
                email = userTestEmail
            };
            var mockUserService = new Mock<IUserRepository>();
            mockUserService.Setup(p => p.FindByEmail(userTestEmail)).Returns(findUserEntity);
            userService = new UserService(mockUserService.Object);
            Assert.ThrowsException<WrongPasswordException>(() => userService.Authenticate(userAuthenticationData));
        }
    }
}