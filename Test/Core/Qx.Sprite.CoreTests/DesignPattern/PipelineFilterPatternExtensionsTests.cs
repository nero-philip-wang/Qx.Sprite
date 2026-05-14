using Xunit;
using Qx.Sprite.Core.DesignPattern;
using System.Collections.Generic;

namespace Qx.Sprite.Core.DesignPattern.Tests
{
    public enum TestPredication
    {
        None = 0,
        Create = 1,
        Update = 2,
        Delete = 4
    }

    public class TestPipelineSubjectBase
    {
        public List<string> ExecutionLog { get; } = new List<string>();

        [PipelineFilter<TestPredication>("TestKey", TestPredication.Create, 1)]
        public virtual void FilterMethod2(object arg)
        {
            ExecutionLog.Add($"FilterMethodBase2-{arg}");
        }

        [PipelineFilter<TestPredication>("DifferentKey", TestPredication.Create, 1)]
        public void FilterMethod3(object arg)
        {
            ExecutionLog.Add($"FilterMethod3-{arg}");
        }
    }

    public class TestPipelineSubject: TestPipelineSubjectBase
    {

        [PipelineFilter<TestPredication>("TestKey", TestPredication.Create, 2)]
        private void FilterMethod1(object arg)
        {
            ExecutionLog.Add($"FilterMethod1-{arg}");
        }

        [PipelineFilter<TestPredication>("TestKey", TestPredication.Create, 1)]
        public override void FilterMethod2(object arg)
        {
            ExecutionLog.Add($"FilterMethod2-{arg}");
        }


        [PipelineFilter<TestPredication>("TestKey", TestPredication.Update, 1)]
        public void FilterMethod4(object arg)
        {
            ExecutionLog.Add($"FilterMethod4-{arg}");
        }

        [PipelineFilter<TestPredication>("TestKey", TestPredication.Create | TestPredication.Update, 3)]
        public void FilterMethod5(object arg)
        {
            ExecutionLog.Add($"FilterMethod5-{arg}");
        }

        public void MethodWithoutAttribute(object arg)
        {
            ExecutionLog.Add($"MethodWithoutAttribute-{arg}");
        }
    }

    public class PipelineFilterPatternExtensionsTests
    {
        [Fact]
        public void PipelineFilterRun_ShouldExecuteMethodsInOrder()
        {
            var subject = new TestPipelineSubject();
            subject.PipelineFilterRun("TestKey", TestPredication.Create, "TestArg");

            Assert.Equal(3, subject.ExecutionLog.Count);
            Assert.Equal("FilterMethod2-TestArg", subject.ExecutionLog[0]);
            Assert.Equal("FilterMethod1-TestArg", subject.ExecutionLog[1]);
            Assert.Equal("FilterMethod5-TestArg", subject.ExecutionLog[2]);
        }

        [Fact]
        public void PipelineFilterRun_ShouldFilterByKey()
        {
            var subject = new TestPipelineSubject();
            subject.PipelineFilterRun("DifferentKey", TestPredication.Create, "TestArg");

            Assert.Single(subject.ExecutionLog);
            Assert.Equal("FilterMethod3-TestArg", subject.ExecutionLog[0]);
        }

        [Fact]
        public void PipelineFilterRun_ShouldFilterByPredication()
        {
            var subject = new TestPipelineSubject();
            subject.PipelineFilterRun("TestKey", TestPredication.Update, "TestArg");

            Assert.Equal(2, subject.ExecutionLog.Count);
            Assert.Equal("FilterMethod4-TestArg", subject.ExecutionLog[0]);
            Assert.Equal("FilterMethod5-TestArg", subject.ExecutionLog[1]);
        }

        [Fact]
        public void PipelineFilterRun_ShouldSupportMultiplePredications()
        {
            var subject = new TestPipelineSubject();
            subject.PipelineFilterRun("TestKey", TestPredication.Create | TestPredication.Update, "TestArg");

            Assert.Equal(1, subject.ExecutionLog.Count);
            Assert.Contains("FilterMethod5-TestArg", subject.ExecutionLog);
        }

        [Fact]
        public void PipelineFilterRun_ShouldNotExecuteMethodsWithoutAttribute()
        {
            var subject = new TestPipelineSubject();
            subject.PipelineFilterRun("TestKey", TestPredication.Create, "TestArg");

            Assert.DoesNotContain("MethodWithoutAttribute-TestArg", subject.ExecutionLog);
        }

        [Fact]
        public void PipelineFilterRun_WithNoMatchingMethods_ShouldNotThrow()
        {
            var subject = new TestPipelineSubject();
            var exception = Record.Exception(() => 
            {
                subject.PipelineFilterRun("NonExistentKey", TestPredication.Create, "TestArg");
            });

            Assert.Null(exception);
            Assert.Empty(subject.ExecutionLog);
        }

        [Fact]
        public void PipelineFilterRun_WithNullArg_ShouldExecute()
        {
            var subject = new TestPipelineSubject();
            subject.PipelineFilterRun("TestKey", TestPredication.Create, null);

            Assert.Equal(3, subject.ExecutionLog.Count);
            Assert.Equal("FilterMethod2-", subject.ExecutionLog[0]);
        }

        [Fact]
        public void PipelineFilterRun_ShouldHandlePrivateMethods()
        {
            var subject = new TestPipelineSubject();
            subject.PipelineFilterRun("TestKey", TestPredication.Create, "TestArg");

            Assert.Contains("FilterMethod1-TestArg", subject.ExecutionLog);
        }
    }
}