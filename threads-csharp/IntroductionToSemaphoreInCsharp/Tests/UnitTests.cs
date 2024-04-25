using IntroductionToSemaphoreInCsharp;
using System.Text;

namespace Tests;

[TestClass]
public class UnitTests
{
    private StringBuilder ConsoleOutput { get; } = new StringBuilder();

    [TestInitialize]
    public void Init()
    {
        Console.SetOut(new StringWriter(this.ConsoleOutput));
        this.ConsoleOutput.Clear();
    }

    [TestMethod]
    public void ExampleWithLocks()
    {
        ExampleWithLock.AccessWithLock();
        AssertAllThreadsExecuted();
    }

    [TestMethod]
    public void ExampleWithMutexs()
    {
        ExampleWithMutex.AccessWithMutex();
        AssertAllThreadsExecuted();
    }

    [TestMethod]
    public void ExampleWithSemaphores()
    {
        ExampleWithSemaphore.AccessWithSemaphore();
        AssertAllThreadsExecuted();
    }

    [TestMethod]
    public async Task ExampleWithSemaphoreSlims()
    {
        await ExampleWithSemaphoreSlim.AccessWithSemaphoreSlimAsync();
        AssertAllThreadsExecuted();
    }

    private void AssertAllThreadsExecuted()
    {
        var result = ConsoleOutput.ToString();
        for (int i = 0;i < 10;i++)
        {
            Assert.IsTrue(result.Contains($"Thread {i}"));
        }
    }
}