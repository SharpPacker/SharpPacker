using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SharpPacker.Tests.BoxPackerTests
{
    public class PackerTest
    {
        [Fact]
        public void TestPackThreeItemsOneDoesntFitInAnyBox()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestPackWithoutBox()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Test weight distribution getter/setter.
        /// </summary>
        [Fact]
        public void TestCanSetMaxBoxesToWeightBalance()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Test that weight redistribution activates (or not) correctly based on the current limit.
        /// </summary>
        [Fact]
        public void TestWeightRedistributionActivatesOrNot()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Test used width calculations on a case where it used to fail.
        /// </summary>
        [Fact]
        public void TestIssue52A()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Test used width calculations on a case where it used to fail.
        /// </summary>
        [Fact]
        public void TestIssue52B()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Test used width calculations on a case where it used to fail.
        /// </summary>
        [Fact]
        public void TestIssue52C()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Test case where last item algorithm picks a slightly inefficient box.
        /// </summary>
        [Fact]
        public void TestIssue117()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Where 2 perfectly filled boxes are a choice, need to ensure we pick the larger one or there is a cascading
        /// failure of many small boxes instead of a few larger ones.
        /// </summary>
        [Fact]
        public void TestIssue38()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// From issue #168.
        /// </summary>
        [Fact]
        public void TestIssue168()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// From issue #170.
        /// </summary>
        [Fact]
        public void TestIssue170()
        {
            throw new NotImplementedException();
        }
    }
}
