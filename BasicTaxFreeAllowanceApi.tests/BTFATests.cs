using BasicTaxFreeAllowanceApi.Models;
using System;
using Xunit;

namespace BasicTaxFreeAllowanceApi.Tests
{
    public class BTFARecordTests
    {
        private static BTFARecord testRecord = new BTFARecord();
        private static BTFARepository testRepo = new BTFARepository();

        [Fact]
        public void IdName_BtfaRecord()
        {
            string expected = "BESK2022_1";
            
            string actual = testRecord.Id;
            
            Assert.Equal(expected, actual);
        }
       

        [Theory]
        [InlineData(10000, 10000)]
        [InlineData(58705, 22700)]
        [InlineData(90200, 29000)]
        [InlineData(122205, 35400)]
        [InlineData(162200, 36000)]
        [InlineData(203205, 31900)]
        [InlineData(274200, 24800)]
        [InlineData(311205, 21100)]
        [InlineData(379200, 14300)]
        [InlineData(1000000, 14200)]
        public void ValueBtfa_BtfaRecord(double input, double expected)
        {
            testRecord.Income = input;
            double actual = testRecord.Btfa;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(10000, 10000)]
        [InlineData(58705, 58705)]
        [InlineData(90200, 90200)]
        public void ValueIncome_BtfaRecord(double input, double expected)
        {
            testRecord.Income = input;
            double actual = testRecord.Income;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Create_BtfaRepository()
        {
            int expected = 1;
            
            testRepo.Create(testRecord);
            int actual = testRepo.GetAll().Count;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetById_BtfaRepository()
        {
            BTFARecord expected = testRecord;

            BTFARecord actual = testRepo.GetById("BESK2022_1");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetByIdFail_BtfaRepository()
        {
            BTFARecord actual = testRepo.GetById("BESK2022_2");

            Assert.Null(actual);
        }
    }
}