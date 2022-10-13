
using Autofac.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TrainerCalenderAPI.Models.Dto;

namespace ApiTrainerTesting
{
    public class TrainerApiTesting
    {
        //private readonly TrainerController _controller;
        //private readonly Mock<ITrainerRepository> _repository;
        private readonly Mock<ITrainerRepository> trainerRepository;

        public TrainerApiTesting()
        {
            //_repository = new Mock<ITrainerRepository>();
            //_controller = new TrainerController( _repository.Object);
            trainerRepository = new Mock<ITrainerRepository>();
        }


        [Fact]
        public async void GetTrainers_WhenCalled_IsItSuccessed()
        {
            // var okResult = await _controller.GetTrainers();

            //Assert.True(okResult.IsSuccess);

            var trainerList = await GetTrainersData();
            trainerRepository.Setup(x => x.GetAllTrainers())
                .Returns( GetTrainersData());

            var trainerController = new TrainerController(trainerRepository.Object);
            //act
            var controller = new TrainerController(trainerRepository.Object);
            var trainerResult =  controller.GetTrainers();
            //var result = ((IEnumerable)trainerResult);

            //assert
            Assert.NotNull(trainerResult);
            Assert.Equal(trainerList.Count(), trainerResult.Result);
            Assert.Equal(trainerList , trainerResult.Result);
          //  Assert.True(trainerList.Equals(trainerResult));
        }

        //[Fact]
        //public async Task GetTrainerById_NonExistingIdPassed_ReturnNotFound()
        //{
        //    var result = await _controller.GetTrainerById("1");

        //    Assert.IsType<NotFoundResult>(result);
        //}

        [Fact]
        public async void GetTrainerById_TrainerObject_TrainerwithSpecificeIdExists()
        {
            //arrange
            var trainers =  GetTrainersData();
            var firstTrainer =  trainers.Result.FirstOrDefault("1");
            trainerRepository.Setup(x => x.GetTrainersById((string)"1"))
                .Returns((Task<Object>)firstTrainer);
            var controller = new TrainerController(trainerRepository.Object);

            //act
            var actionResult = await controller.GetTrainerById((string)"1");
            var result = actionResult.Result as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(result);

           // result.Value.Equals().BeEquivalentTo(firstTrainer);
        }


        private async Task<IEnumerable<object>> GetTrainersData()
        {
            IEnumerable<TrainerModelDto> trainersData =  new List<TrainerModelDto>
            {
                new TrainerModelDto
                {
                    EmpId = "1",
                    Name = "Bhavesh",
                    Email = "b@gmail.com",
                    PhoneNum = "9238230312",
                    Skills = new List<SkillModelDto> { new SkillModelDto { Id = 1, Name = "Java" }, new SkillModelDto { Id = 2, Name = ".Net" } }
                },
                new TrainerModelDto
                {
                    EmpId = "2",
                    Name = "Nitin",
                    Email = "n@gmail.com",
                    PhoneNum = "23423423423",
                    Skills = new List<SkillModelDto> {  new SkillModelDto { Id = 2, Name = ".Net" } }
                },
                new TrainerModelDto
                {
                    EmpId = "3",
                    Name = "Abhi",
                    Email = "a@gmail.com",
                    PhoneNum = "4423231221",
                    Skills = new List<SkillModelDto> { new SkillModelDto { Id = 1, Name = "Java" } }
                },
            };
            return  trainersData;
        }
    }
}